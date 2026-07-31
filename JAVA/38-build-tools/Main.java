import java.util.*;

public class Main {

    // --- 빌드 태스크 ---
    record Task(String name, List<String> dependsOn, Runnable action) {}

    // --- 미니 빌드 엔진: 태스크 의존성 그래프를 위상 정렬해 실행 ---
    static class BuildEngine {
        private final Map<String, Task> tasks = new LinkedHashMap<>();
        private final Set<String> executed = new HashSet<>();
        private final Deque<String> inProgress = new ArrayDeque<>();

        void register(Task task) {
            tasks.put(task.name(), task);
        }

        // 태스크 실행: 먼저 의존 태스크를 재귀적으로 실행 (Gradle 의 task graph 흉내)
        void run(String taskName) {
            Task task = tasks.get(taskName);
            if (task == null) throw new IllegalArgumentException("존재하지 않는 태스크: " + taskName);
            if (executed.contains(taskName)) return;   // 이미 실행됨 (증분 빌드 스킵)

            for (String dep : task.dependsOn()) run(dep);   // 의존성 먼저

            System.out.println("  > 실행: " + taskName);
            task.action().run();
            executed.add(taskName);
        }
    }

    // 그룹/아티팩트/버전을 가지는 의존성 좌표 (GAV)
    record Dependency(String groupId, String artifactId, String version) {
        @Override public String toString() {
            return groupId + ":" + artifactId + ":" + version;
        }
    }

    public static void main(String[] args) {
        System.out.println("=== 표준 디렉터리 구조 ===");

        List<String> structure = List.of(
            "my-project/",
            "  ├─ pom.xml",
            "  └─ src/",
            "      ├─ main/java/com/example/Application.java",
            "      ├─ main/resources/application.yml",
            "      ├─ test/java/com/example/ApplicationTest.java",
            "      └─ test/resources/application-test.yml"
        );
        structure.forEach(s -> System.out.println("  " + s));

        System.out.println("\n=== 미니 빌드 엔진 (Maven/Gradle 흉내) ===");

        BuildEngine engine = new BuildEngine();
        engine.register(new Task("clean", List.of(), () -> System.out.println("  [clean] 빌드 산출물 삭제")));
        engine.register(new Task("compile", List.of("clean"), () -> System.out.println("  [compile] .java -> .class 컴파일")));
        engine.register(new Task("test", List.of("compile"), () -> System.out.println("  [test] 단위 테스트 실행")));
        engine.register(new Task("package", List.of("test"), () -> System.out.println("  [package] jar 패키징 생성")));
        engine.register(new Task("deploy", List.of("package"), () -> System.out.println("  [deploy] 저장소에 배포")));

        System.out.println("--- gradle build 실행 ---");
        engine.run("package");
        engine.run("deploy");

        System.out.println("\n=== 의존성 관리 (Maven Central 개념) ===");

        List<Dependency> deps = List.of(
            new Dependency("org.springframework.boot", "spring-boot-starter-web", "3.2.0"),
            new Dependency("org.junit.jupiter", "junit-jupiter", "5.10.0"),
            new Dependency("com.h2database", "h2", "2.2.224")
        );
        System.out.println("  프로젝트 의존성 " + deps.size() + "개:");
        deps.forEach(d -> System.out.println("    - " + d));

        System.out.println("\n=== 빌드 라이프사이클 ===");

        String[] lifecycle = {"clean", "compile", "test", "package", "install", "deploy"};
        System.out.println("  " + String.join(" -> ", lifecycle));

        System.out.println("\n=== 실제 Maven/Gradle 설정 (주석) ===");

        /*
        <!-- pom.xml (Maven) -->
        <project xmlns="http://maven.apache.org/POM/4.0.0">
            <groupId>com.example</groupId>
            <artifactId>my-project</artifactId>
            <version>1.0.0</version>
            <dependencies>
                <dependency>
                    <groupId>org.junit.jupiter</groupId>
                    <artifactId>junit-jupiter</artifactId>
                    <version>5.10.0</version>
                    <scope>test</scope>
                </dependency>
            </dependencies>
        </project>

        // build.gradle (Gradle)
        plugins {
            id 'java'
        }
        repositories { mavenCentral() }
        dependencies {
            testImplementation 'org.junit.jupiter:junit-jupiter:5.10.0'
        }
        */
    }
}
