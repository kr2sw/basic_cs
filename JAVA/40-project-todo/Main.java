import java.time.LocalDateTime;
import java.time.format.DateTimeFormatter;
import java.util.*;
import java.util.stream.*;

public class Main {

    // --- 도메인 ---
    enum Priority { HIGH, MEDIUM, LOW }
    enum Category { WORK, PERSONAL, STUDY }

    record Todo(long id, String title, Category category, Priority priority,
                boolean done, LocalDateTime createdAt) {

        Todo withDone(boolean done) {
            return new Todo(id, title, category, priority, done, createdAt);
        }

        @Override public String toString() {
            String status = done ? "[완료]" : "[진행]";
            return "#" + id + " " + status + " " + title +
                " (" + category + "/" + priority + "/" +
                createdAt.format(DateTimeFormatter.ofPattern("MM-dd HH:mm")) + ")";
        }
    }

    // --- Repository 계층 (데이터 저장) ---
    static class TodoRepository {
        private final Map<Long, Todo> store = new LinkedHashMap<>();
        private long seq = 1;

        Todo save(Todo todo) {
            Todo saved = new Todo(todo.id() == 0 ? seq++ : todo.id(),
                todo.title(), todo.category(), todo.priority(), todo.done(), todo.createdAt());
            store.put(saved.id(), saved);
            return saved;
        }

        Optional<Todo> findById(long id) { return Optional.ofNullable(store.get(id)); }
        List<Todo> findAll() { return new ArrayList<>(store.values()); }
        boolean delete(long id) { return store.remove(id) != null; }
    }

    // --- Service 계층 (비즈니스 로직) ---
    static class TodoService {
        private final TodoRepository repo = new TodoRepository();

        Todo add(String title, Category category, Priority priority) {
            return repo.save(new Todo(0, title, category, priority, false, LocalDateTime.now()));
        }

        Optional<Todo> complete(long id) {
            Optional<Todo> todo = repo.findById(id);
            todo.ifPresent(t -> repo.save(t.withDone(true)));
            return todo;
        }

        boolean delete(long id) { return repo.delete(id); }

        List<Todo> list() {
            return repo.findAll();
        }

        // 정렬 + 필터 (Comparator, Stream)
        List<Todo> list(boolean onlyUndone, Priority priority, String keyword) {
            Stream<Todo> stream = repo.findAll().stream();
            if (onlyUndone) stream = stream.filter(t -> !t.done());
            if (priority != null) stream = stream.filter(t -> t.priority() == priority);
            if (keyword != null && !keyword.isBlank()) stream = stream.filter(t -> t.title().contains(keyword));
            return stream.sorted(Comparator.comparing(Todo::priority)
                    .thenComparing(Todo::id))
                .collect(Collectors.toList());
        }

        long count() { return repo.findAll().size(); }
        long doneCount() { return repo.findAll().stream().filter(Todo::done).count(); }
    }

    // --- Controller 계층 (메뉴/입출력) ---
    static class TodoApp {
        private final TodoService service = new TodoService();
        private final Scanner scanner = new Scanner(System.in);

        void demo() {
            System.out.println("=== 자동 시연 모드 ===");
            service.add("Java 21 가상 스레드 복습", Category.STUDY, Priority.HIGH);
            service.add("스프링 프로젝트 회의", Category.WORK, Priority.MEDIUM);
            service.add("운동 30분 하기", Category.PERSONAL, Priority.LOW);
            service.complete(1);
            service.add("JPQL 쿼리 정리", Category.STUDY, Priority.HIGH);
            service.complete(2);

            System.out.println("[전체 목록]");
            service.list().forEach(t -> System.out.println("  " + t));

            System.out.println("\n[미완료 + HIGH 우선순위]");
            service.list(true, Priority.HIGH, null).forEach(t -> System.out.println("  " + t));

            System.out.println("\n[검색: '정리']");
            service.list(false, null, "정리").forEach(t -> System.out.println("  " + t));

            System.out.println("\n[통계]");
            System.out.println("  총 " + service.count() + "건, 완료 " + service.doneCount() + "건");

            System.out.println("\n[완료 처리 #4 → 목록]");
            service.complete(4);
            service.list().forEach(t -> System.out.println("  " + t));
        }

        void run() {
            System.out.println("=== 할일 관리 앱 ===");
            while (true) {
                System.out.println("\n1. 추가  2. 목록  3. 완료  4. 삭제  5. 종료");
                System.out.print("메뉴 선택: ");
                String input = scanner.nextLine();
                switch (input) {
                    case "1" -> addMenu();
                    case "2" -> service.list().forEach(t -> System.out.println("  " + t));
                    case "3" -> completeMenu();
                    case "4" -> deleteMenu();
                    case "5" -> {
                        System.out.println("종료합니다.");
                        return;
                    }
                    default -> System.out.println("잘못된 입력입니다.");
                }
            }
        }

        private void addMenu() {
            System.out.print("제목: ");
            String title = scanner.nextLine();
            System.out.print("카테고리 (WORK/PERSONAL/STUDY): ");
            Category category = Category.valueOf(scanner.nextLine().trim().toUpperCase());
            System.out.print("우선순위 (HIGH/MEDIUM/LOW): ");
            Priority priority = Priority.valueOf(scanner.nextLine().trim().toUpperCase());
            Todo saved = service.add(title, category, priority);
            System.out.println("추가됨: " + saved);
        }

        private void completeMenu() {
            System.out.print("완료할 id: ");
            long id = Long.parseLong(scanner.nextLine().trim());
            service.complete(id)
                .ifPresentOrElse(t -> System.out.println("완료 처리: " + t),
                    () -> System.out.println("해당 id가 없습니다."));
        }

        private void deleteMenu() {
            System.out.print("삭제할 id: ");
            long id = Long.parseLong(scanner.nextLine().trim());
            System.out.println(service.delete(id) ? "삭제됨" : "해당 id가 없습니다.");
        }
    }

    public static void main(String[] args) {
        TodoApp app = new TodoApp();
        if (args.length > 0 && "demo".equals(args[0])) {
            app.demo();   // 자동 시연
        } else {
            app.run();    // 인터렉티브 모드
        }
    }
}
