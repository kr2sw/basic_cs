import java.sql.*;

public class Main {
    public static void main(String[] args) {
        // H2 인메모리 데이터베이스 사용 (별도 설치 불필요)
        String url = "jdbc:h2:mem:testdb";
        String user = "sa";
        String password = "";

        try (Connection conn = DriverManager.getConnection(url, user, password)) {
            System.out.println("DB 연결 성공!");

            // 테이블 생성
            String createTable = """
                CREATE TABLE users (
                    id INT AUTO_INCREMENT PRIMARY KEY,
                    name VARCHAR(100) NOT NULL,
                    email VARCHAR(100) UNIQUE,
                    age INT
                )
                """;
            try (Statement stmt = conn.createStatement()) {
                stmt.execute(createTable);
                System.out.println("테이블 생성 완료");
            }

            // INSERT - PreparedStatement
            String insertSQL = "INSERT INTO users (name, email, age) VALUES (?, ?, ?)";
            try (PreparedStatement pstmt = conn.prepareStatement(insertSQL)) {
                // 데이터 1
                pstmt.setString(1, "Alice");
                pstmt.setString(2, "alice@example.com");
                pstmt.setInt(3, 25);
                pstmt.executeUpdate();

                // 데이터 2
                pstmt.setString(1, "Bob");
                pstmt.setString(2, "bob@example.com");
                pstmt.setInt(3, 30);
                pstmt.executeUpdate();

                // 데이터 3
                pstmt.setString(1, "Charlie");
                pstmt.setString(2, "charlie@example.com");
                pstmt.setInt(3, 35);
                pstmt.executeUpdate();

                System.out.println("데이터 삽입 완료");
            }

            // SELECT
            String selectSQL = "SELECT * FROM users";
            try (Statement stmt = conn.createStatement();
                 ResultSet rs = stmt.executeQuery(selectSQL)) {

                System.out.println("\n=== 사용자 목록 ===");
                while (rs.next()) {
                    int id = rs.getInt("id");
                    String name = rs.getString("name");
                    String email = rs.getString("email");
                    int age = rs.getInt("age");
                    System.out.printf("%d | %s | %s | %d%n", id, name, email, age);
                }
            }

            // UPDATE
            String updateSQL = "UPDATE users SET age = ? WHERE name = ?";
            try (PreparedStatement pstmt = conn.prepareStatement(updateSQL)) {
                pstmt.setInt(1, 26);
                pstmt.setString(2, "Alice");
                int updated = pstmt.executeUpdate();
                System.out.println("\n업데이트된 행: " + updated);
            }

            // 조건 SELECT
            String selectParamSQL = "SELECT * FROM users WHERE age > ?";
            try (PreparedStatement pstmt = conn.prepareStatement(selectParamSQL)) {
                pstmt.setInt(1, 28);
                try (ResultSet rs = pstmt.executeQuery()) {
                    System.out.println("\n=== 나이 28 초과 ===");
                    while (rs.next()) {
                        System.out.println(rs.getString("name") + " (" + rs.getInt("age") + ")");
                    }
                }
            }

            // DELETE
            String deleteSQL = "DELETE FROM users WHERE name = ?";
            try (PreparedStatement pstmt = conn.prepareStatement(deleteSQL)) {
                pstmt.setString(1, "Charlie");
                int deleted = pstmt.executeUpdate();
                System.out.println("\n삭제된 행: " + deleted);
            }

            // 트랜잭션
            conn.setAutoCommit(false);
            try (PreparedStatement pstmt = conn.prepareStatement(
                    "INSERT INTO users (name, email, age) VALUES (?, ?, ?)")) {
                pstmt.setString(1, "David");
                pstmt.setString(2, "david@example.com");
                pstmt.setInt(3, 28);
                pstmt.executeUpdate();
                conn.commit();
                System.out.println("트랜잭션 커밋 완료");
            } catch (SQLException e) {
                conn.rollback();
                System.out.println("트랜잭션 롤백: " + e.getMessage());
            }

        } catch (SQLException e) {
            System.out.println("DB 오류: " + e.getMessage());
            e.printStackTrace();
        }
    }
}
