import java.lang.annotation.*;
import java.lang.reflect.*;
import java.util.*;

public class Main {

    // 1. 커스텀 어노테이션: 작업 지시
    @Retention(RetentionPolicy.RUNTIME)
    @Target(ElementType.METHOD)
    @interface Todo {
        String value() default "작업 없음";
        int priority() default 5;      // 1 = 긴급, 5 = 보통, 10 = 낮음
    }

    // 2. 커스텀 어노테이션: 필드 검증용
    @Retention(RetentionPolicy.RUNTIME)
    @Target(ElementType.FIELD)
    @interface NotEmpty {
        String message() default "빈 값이면 안 됩니다";
    }

    @Retention(RetentionPolicy.RUNTIME)
    @Target(ElementType.FIELD)
    @interface Range {
        int min() default 0;
        int max() default 100;
    }

    // 3. 클래스 레벨 어노테이션
    @Retention(RetentionPolicy.RUNTIME)
    @Target(ElementType.TYPE)
    @interface Table {
        String name();
    }

    // 어노테이션 적용 예
    static class UserService {
        @Todo(value = "비밀번호 암호화 추가", priority = 1)
        public void signup() {
            System.out.println("  회원가입 로직 (TODO 잔존)");
        }

        @Todo("로그인 실패 로그 추가")
        public void login() {
            System.out.println("  로그인 로직");
        }

        public void done() {
            System.out.println("  완료된 메서드 (어노테이션 없음)");
        }
    }

    @Table(name = "users")
    static class User {
        @NotEmpty(message = "이름은 필수입니다")
        @Range(min = 2, max = 10)
        String name;

        @Range(min = 0, max = 150, message = "나이 범위가 잘못되었습니다")
        int age;
    }

    public static void main(String[] args) throws Exception {
        System.out.println("=== @Todo 어노테이션 리플렉션 조회 ===");

        // 메서드에 붙은 어노테이션 읽기
        for (Method m : UserService.class.getDeclaredMethods()) {
            Todo todo = m.getAnnotation(Todo.class);
            if (todo != null) {
                System.out.println("  미완료 작업: " + m.getName() +
                    "() 우선순위 " + todo.priority() + " - " + todo.value());
            } else {
                System.out.println("  (어노테이션 없음): " + m.getName());
            }
        }

        System.out.println("\n=== 검증 프레임워크 시뮬레이션 ===");

        User valid = new User();
        valid.name = "김철수";
        valid.age = 30;
        validate(valid);
        System.out.println("  valid 검증 통과!");

        User invalid = new User();
        invalid.name = "";
        invalid.age = 200;
        validate(invalid);
        System.out.println("  invalid 검증 결과: 위 오류 발견");

        System.out.println("\n=== 클래스 레벨 어노테이션 (테이블 매핑) ===");

        Table table = User.class.getAnnotation(Table.class);
        System.out.println("  클래스 User는 테이블 'users'에 매핑됨: " + table.name());

        System.out.println("\n=== Retention 정책 확인 ===");

        Retention retention = Todo.class.getAnnotation(Retention.class);
        System.out.println("  @Todo 유지 정책: " + retention.value());
        System.out.println("  @Todo 대상: " + Arrays.toString(Todo.class.getAnnotation(Target.class).value()));
    }

    // 리플렉션으로 @NotEmpty, @Range 검증을 수행하는 미니 검증기
    static void validate(Object obj) throws IllegalAccessException {
        for (Field f : obj.getClass().getDeclaredFields()) {
            f.setAccessible(true);
            NotEmpty notEmpty = f.getAnnotation(NotEmpty.class);
            if (notEmpty != null) {
                String value = (String) f.get(obj);
                if (value == null || value.isBlank()) {
                    System.out.println("  [검증 오류] " + f.getName() + ": " + notEmpty.message());
                }
            }
            Range range = f.getAnnotation(Range.class);
            if (range != null) {
                int value = f.getInt(obj);
                if (value < range.min() || value > range.max()) {
                    System.out.println("  [검증 오류] " + f.getName() +
                        ": " + range.min() + "~" + range.max() + " 범위를 벗어남 (실제: " + value + ")");
                }
            }
        }
    }
}
