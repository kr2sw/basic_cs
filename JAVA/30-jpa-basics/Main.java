import java.util.*;
import java.util.function.*;

public class Main {

    // --- JPA 어노테이션을 흉내 낸 메타 어노테이션 ---
    @interface Entity {}
    @interface Id {}
    @interface GeneratedValue {}
    @interface Column { String name() default ""; }

    // --- 엔티티 클래스 ---
    @Entity
    static class User {
        @Id @GeneratedValue Long id;
        @Column(name = "user_name") String name;
        int age;

        User(String name, int age) { this.name = name; this.age = age; }
        User(Long id, String name, int age) { this.id = id; this.name = name; this.age = age; }

        @Override public String toString() {
            return "User{id=" + id + ", name='" + name + "', age=" + age + "}";
        }
    }

    // --- 영속성 컨텍스트 (1차 캐시 + dirty checking + 쓰기 지연) 시뮬레이션 ---
    static class SimulatedEntityManager {
        private final Map<Long, User> cache = new HashMap<>();     // 1차 캐시
        private final Map<Long, User> snapshot = new HashMap<>();  // flush 시점 스냅샷
        private final Map<Long, User> database = new HashMap<>();  // "DB" 테이블
        private long seq = 1;
        private final List<String> sqlLog = new ArrayList<>();

        SimulatedEntityManager() {
            database.put(1L, new User(1L, "김철수", 30));
            database.put(2L, new User(2L, "이영희", 28));
            seq = 3;
        }

        // find: 1차 캐시에 있으면 캐시 반환, 없으면 DB에서 조회 후 캐시에 저장
        User find(Class<User> type, Long id) {
            if (cache.containsKey(id)) {
                System.out.println("  [1차 캐시 히트] id=" + id);
                return cache.get(id);
            }
            User u = database.get(id);
            if (u != null) {
                cache.put(id, u);
                snapshot.put(id, copy(u));
                sqlLog.add("SELECT * FROM users WHERE id = " + id);
            }
            return u;
        }

        // persist: INSERT 예약 (쓰기 지연)
        void persist(User u) {
            u.id = seq++;
            cache.put(u.id, u);
            snapshot.put(u.id, copy(u));
            sqlLog.add("INSERT INTO users(name, age) VALUES ('" + u.name + "', " + u.age + ")  [예약]");
        }

        // flush: 변경 감지 후 스냅샷과 다르면 UPDATE, 예약된 INSERT 반영
        void flush() {
            List<Long> dirty = new ArrayList<>();
            for (Map.Entry<Long, User> e : cache.entrySet()) {
                if (!copy(e.getValue()).equals(snapshot.get(e.getKey()))) dirty.add(e.getKey());
            }
            for (Long id : dirty) {
                User u = cache.get(id);
                sqlLog.add("UPDATE users SET name='" + u.name + "', age=" + u.age + " WHERE id=" + id);
                System.out.println("  [Dirty Checking] 변경 감지: id=" + id + " -> UPDATE 예약");
            }
            // 모든 변경을 DB에 반영
            for (Long id : dirty) database.put(id, copy(cache.get(id)));
            System.out.println("  [Flush] 예약된 SQL 을 DB 에 반영");
            cache.forEach((k, v) -> database.put(k, copy(v)));
            cache.forEach((k, v) -> snapshot.put(k, copy(v)));
        }

        // JPQL 유사 쿼리: 엔티티 필드 기준 필터링
        List<User> createQuerySelect(String where, Predicate<User> filter) {
            List<User> result = new ArrayList<>();
            database.values().stream().filter(filter).forEach(u -> result.add(u));
            sqlLog.add("JPQL: SELECT u FROM User u WHERE " + where);
            return result;
        }

        void printSqlLog() {
            System.out.println("  [SQL 로그]");
            for (String sql : sqlLog) System.out.println("    " + sql);
        }

        static User copy(User u) { return new User(u.id, u.name, u.age); }
    }

    public static void main(String[] args) {
        SimulatedEntityManager em = new SimulatedEntityManager();

        System.out.println("=== 1차 캐시 (Identity Map) ===");

        User first = em.find(User.class, 1L);   // DB 조회 후 캐시
        User second = em.find(User.class, 1L);  // 1차 캐시 히트
        System.out.println("  같은 인스턴스? " + (first == second));

        System.out.println("\n=== persist + 쓰기 지연 + flush ===");

        em.persist(new User("박민준", 35));     // INSERT 예약
        em.flush();                            // 예약 SQL 반영

        System.out.println("\n=== Dirty Checking ===");

        User managed = em.find(User.class, 2L);
        System.out.println("  원래 이름: " + managed.name);
        managed.name = "이영희(개명)";           // 엔티티 변경만으로 UPDATE 자동 처리
        managed.age = 29;
        em.flush();

        System.out.println("\n=== 다시 조회 (변경 반영 확인) ===");

        User reloaded = em.find(User.class, 2L);
        System.out.println("  변경 후: " + reloaded);

        System.out.println("\n=== JPQL 유사 쿼리 ===");

        // 실제 JPA: em.createQuery("SELECT u FROM User u WHERE u.age > :age", User.class)
        List<User> adults = em.createQuerySelect("u.age >= 30", u -> u.age >= 30);
        System.out.println("  나이 30 이상: " + adults);

        List<User> kim = em.createQuerySelect("u.name LIKE '%김%'", u -> u.name.contains("김"));
        System.out.println("  이름에 '김' 포함: " + kim);

        System.out.println("\n=== 실행된 SQL 로그 ===");

        em.printSqlLog();

        System.out.println("\n=== 실제 JPA 코드 형태 (주석) ===");

        /*
        // 실제 Hibernate/JPA 코드 (강의자료용 참고)
        EntityManagerFactory emf = Persistence.createEntityManagerFactory("unit");
        EntityManager entityManager = emf.createEntityManager();
        EntityTransaction tx = entityManager.getTransaction();

        tx.begin();
        User user = entityManager.find(User.class, 1L);   // SELECT
        user.setName("김철수(수정)");                       // dirty checking
        entityManager.persist(new User("새사용자", 25));    // INSERT 예약
        tx.commit();                                      // flush + commit

        List<User> result = entityManager
            .createQuery("SELECT u FROM User u WHERE u.age > :age", User.class)
            .setParameter("age", 30)
            .getResultList();
        entityManager.close();
        */
    }
}
