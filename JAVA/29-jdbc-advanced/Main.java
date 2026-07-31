import java.sql.*;
import java.util.*;
import java.util.concurrent.atomic.*;

public class Main {

    record User(long id, String name, int balance) {}

    // Repository 패턴: 인터페이스로 추상화
    interface UserRepository {
        Optional<User> findById(long id);
        List<User> findAll();
        User save(User user);
        void deleteById(long id);
    }

    // 인메모리 구현 (실행 가능한 버전 - DB 불필요)
    static class InMemoryUserRepository implements UserRepository {
        private final Map<Long, User> store = new HashMap<>();
        private final AtomicLong seq = new AtomicLong(100);

        InMemoryUserRepository() {
            store.put(1L, new User(1, "김철수", 10_000));
            store.put(2L, new User(2, "이영희", 5_000));
        }

        @Override public Optional<User> findById(long id) { return Optional.ofNullable(store.get(id)); }
        @Override public List<User> findAll() { return new ArrayList<>(store.values()); }

        @Override public User save(User user) {
            User saved = new User(user.id() == 0 ? seq.getAndIncrement() : user.id(),
                user.name(), user.balance());
            store.put(saved.id(), saved);
            return saved;
        }

        @Override public void deleteById(long id) { store.remove(id); }
    }

    public static void main(String[] args) {
        System.out.println("=== Repository 패턴 (인메모리) ===");

        UserRepository repo = new InMemoryUserRepository();

        // 조회
        repo.findById(1L).ifPresent(u -> System.out.println("  조회: " + u));
        System.out.println("  전체: " + repo.findAll());

        // 저장 / 삭제
        User saved = repo.save(new User(0, "박민준", 20_000));
        System.out.println("  저장: " + saved);
        repo.deleteById(saved.id());
        System.out.println("  삭제 후 전체: " + repo.findAll());

        System.out.println("\n=== JDBC 트랜잭션 데모 ===");

        // 실제 JDBC 코드는 드라이버가 있을 때 동작합니다.
        // 드라이버가 없으면 "데이터베이스에 연결되지 않아 데모를 생략합니다" 출력
        runJdbcDemo();

        System.out.println("\n=== JDBC 배치 (가상 시나리오) ===");

        // 배치 삽입 흐름을 시뮬레이션
        int batchSize = 100;
        int totalRows = 0;
        for (int i = 0; i < 5; i++) {
            System.out.println("  배치 " + (i + 1) + "회 실행: " + batchSize + "건 삽입");
            totalRows += batchSize;
        }
        System.out.println("  배치 완료, 총 삽입: " + totalRows + "건");
    }

    // 실제 JDBC 트랜잭션/배치 예제 (드라이버 없는 환경에서도 컴파일 가능)
    static void runJdbcDemo() {
        String url = "jdbc:h2:mem:bank;DB_CLOSE_DELAY=-1";   // 예: H2 인메모리 DB
        String user = "sa";
        String pass = "";

        try (Connection conn = DriverManager.getConnection(url, user, pass)) {
            conn.setAutoCommit(false);

            // 1. 계좌 이체: 두 UPDATE 를 하나의 트랜잭션으로
            try (PreparedStatement ps = conn.prepareStatement(
                    "UPDATE account SET balance = balance - ? WHERE id = ?")) {
                ps.setInt(1, 500);   ps.setLong(2, 1L);   ps.addBatch();
                ps.setInt(1, 500);   ps.setLong(2, 2L);   ps.addBatch();
                int[] results = ps.executeBatch();
                conn.commit();
                System.out.println("  트랜잭션 커밋 성공, 배치 실행 결과: " + Arrays.toString(results));
            } catch (SQLException e) {
                conn.rollback();
                System.out.println("  트랜잭션 롤백: " + e.getMessage());
            }
        } catch (SQLException e) {
            System.out.println("  데이터베이스에 연결되지 않아 데모를 생략합니다: " +
                (e.getMessage() == null ? e.getClass().getSimpleName() : e.getMessage()));
            System.out.println("  (pom.xml에 H2 또는 MySQL 드라이버를 추가하면 실제로 동작합니다)");
        }
    }

    /*
    // 실제 프로젝트의 배치 예제 (주석 - 드라이버 필요)
    int insertBatch(Connection conn, List<User> users) throws SQLException {
        String sql = "INSERT INTO users(name, balance) VALUES (?, ?)";
        try (PreparedStatement ps = conn.prepareStatement(sql)) {
            for (User u : users) {
                ps.setString(1, u.name());
                ps.setInt(2, u.balance());
                ps.addBatch();
            }
            int[] results = ps.executeBatch();
            return results.length;
        }
    }
    */
}
