import java.io.*;
import java.net.*;

public class Main {
    public static void main(String[] args) throws Exception {
        // 1. InetAddress
        InetAddress local = InetAddress.getLocalHost();
        System.out.println("로컬 호스트: " + local.getHostName());
        System.out.println("로컬 IP: " + local.getHostAddress());

        InetAddress google = InetAddress.getByName("google.com");
        System.out.println("구글 IP: " + google.getHostAddress());
        System.out.println("구글 호스트: " + google.getHostName());

        // 2. 서버 소켓 시작 (별도 스레드)
        Thread serverThread = new Thread(() -> {
            try (ServerSocket serverSocket = new ServerSocket(9999)) {
                System.out.println("\n서버 시작 (포트 9999)");
                try (Socket clientSocket = serverSocket.accept();
                     BufferedReader in = new BufferedReader(
                         new InputStreamReader(clientSocket.getInputStream()));
                     PrintWriter out = new PrintWriter(
                         clientSocket.getOutputStream(), true)) {

                    String msg = in.readLine();
                    System.out.println("서버 수신: " + msg);
                    out.println("서버 응답: " + msg.toUpperCase());
                }
            } catch (IOException e) {
                e.printStackTrace();
            }
        });
        serverThread.start();

        Thread.sleep(500); // 서버 시작 대기

        // 3. 클라이언트 소켓
        try (Socket socket = new Socket("localhost", 9999);
             PrintWriter out = new PrintWriter(
                 socket.getOutputStream(), true);
             BufferedReader in = new BufferedReader(
                 new InputStreamReader(socket.getInputStream()))) {

            out.println("Hello Server!");
            String response = in.readLine();
            System.out.println("클라이언트 수신: " + response);
        }

        serverThread.join();

        // 4. URL (HTTP)
        System.out.println("\n=== URL ===");
        URL url = new URL("https://httpbin.org/get");
        System.out.println("프로토콜: " + url.getProtocol());
        System.out.println("호스트: " + url.getHost());
        System.out.println("포트: " + url.getPort()); // -1 (기본값)

        // 5. URLConnection
        System.out.println("\n=== HTTP GET ===");
        HttpURLConnection httpConn = (HttpURLConnection)
            new URL("https://httpbin.org/get").openConnection();
        httpConn.setRequestMethod("GET");
        httpConn.setConnectTimeout(3000);
        httpConn.setReadTimeout(3000);

        int responseCode = httpConn.getResponseCode();
        System.out.println("응답 코드: " + responseCode);

        try (BufferedReader br = new BufferedReader(
                new InputStreamReader(httpConn.getInputStream()))) {
            String line;
            System.out.println("응답 본문:");
            while ((line = br.readLine()) != null) {
                System.out.println(line);
                break; // 첫 줄만 출력
            }
        }

        System.out.println("\n네트워킹 예제 완료");
    }
}
