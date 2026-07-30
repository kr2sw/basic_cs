import java.io.*;
import java.nio.file.*;
import java.util.*;

public class Main {
    public static void main(String[] args) throws IOException {
        // 임시 파일 경로
        String filePath = "sample.txt";
        String copyPath = "sample_copy.txt";

        // 1. FileWriter / PrintWriter (문자 출력)
        try (PrintWriter pw = new PrintWriter(new FileWriter(filePath))) {
            pw.println("Hello, World!");
            pw.println("Java I/O 예제입니다.");
            pw.println("여러 줄을 파일에 씁니다.");
            pw.printf("숫자: %d, 문자열: %s%n", 100, "test");
        }
        System.out.println("파일 작성 완료: " + filePath);

        // 2. BufferedReader (문자 입력, 한 줄씩)
        try (BufferedReader br = new BufferedReader(new FileReader(filePath))) {
            System.out.println("\n=== BufferedReader 읽기 ===");
            String line;
            while ((line = br.readLine()) != null) {
                System.out.println(line);
            }
        }

        // 3. FileInputStream / FileOutputStream (바이트 단위)
        try (FileInputStream fis = new FileInputStream(filePath);
             FileOutputStream fos = new FileOutputStream(copyPath)) {
            byte[] buffer = new byte[1024];
            int bytesRead;
            while ((bytesRead = fis.read(buffer)) != -1) {
                fos.write(buffer, 0, bytesRead);
            }
        }
        System.out.println("파일 복사 완료: " + copyPath);

        // 4. NIO Files API (Java 7+)
        Path path = Paths.get(filePath);
        List<String> lines = Files.readAllLines(path);
        System.out.println("\n=== NIO Files.readAllLines ===");
        lines.forEach(System.out::println);

        // 5. Files.write
        List<String> newLines = Arrays.asList("NIO로 작성", "두 번째 줄", "세 번째 줄");
        Path newPath = Paths.get("nio_sample.txt");
        Files.write(newPath, newLines);
        System.out.println("NIO 파일 작성 완료");

        // 6. 파일 정보
        File file = new File(filePath);
        System.out.println("\n=== 파일 정보 ===");
        System.out.println("파일명: " + file.getName());
        System.out.println("크기: " + file.length() + " bytes");
        System.out.println("수정 시간: " + new Date(file.lastModified()));
        System.out.println("절대 경로: " + file.getAbsolutePath());

        // 7. 디렉토리 목록
        System.out.println("\n=== 현재 디렉토리 ===");
        Files.list(Paths.get("."))
            .limit(10)
            .forEach(p -> System.out.println(p.getFileName()));

        // 정리
        Files.deleteIfExists(path);
        Files.deleteIfExists(Paths.get(copyPath));
        Files.deleteIfExists(newPath);
        System.out.println("\n임시 파일 정리 완료");
    }
}
