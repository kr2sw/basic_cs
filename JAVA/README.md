# Java 기초 강의 (20개 챕터)

Java 프로그래밍 언어의 기초부터 고급 개념까지 학습할 수 있는 예제 모음입니다.

## 역사

Java는 1995년 Sun Microsystems의 James Gosling과 Green Team이 "Oak"라는 이름으로 처음 개발했습니다. 원래는 가전제기(STB, PDA)용 언어였으나, 인터넷의 성장과 함께 웹 애플리케이션 언어로 급부상했습니다. 1995년 Netscape Navigator에 Java 애플릿이 탑재되면서 대중화되었고, 2006년 오픈소스(GPL)로 전환되었습니다. 2010년 Oracle이 Sun을 인수하면서 Java의 주도권이 넘어갔습니다. 주요 버전으로는 Java 5(2004, Generics/Enum), Java 8(2014, Lambda/Stream), Java 17(2021, LTS), Java 21(2023, LTS)이 있습니다.

## 특징

- **Write Once, Run Anywhere**: JVM(Java Virtual Machine) 위에서 실행되어 플랫폼 독립적
- **자동 메모리 관리**: 가비지 컬렉터(GC)가 메모리를 자동으로 관리
- **강력한 표준 라이브러리**: Collections, Stream, I/O, Networking, JDBC 등 방대한 API
- **멀티스레딩**: 언어 수준의 스레드 지원과 동기화 도구
- **보안**: 바이트코드 검증, 샌드박스, SecurityManager
- **대규모 엔터프라이즈**: Spring, Jakarta EE 등 강력한 프레임워크 생태계
- **정적 타입 + 동적 기능**: 제네릭, 리플렉션, 어노테이션

## 실행

```bash
cd JAVA/01-hello-world && javac Main.java && java Main
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | 기본 입출력, 첫 Java 프로그램, 주석 |
| 02 | Variables | 변수, 기본형/참조형, 형변환, 상수 |
| 03 | Control Flow | 조건문 (if/else, switch), 반복문 (for, while, do-while) |
| 04 | Arrays | 배열, 다차원 배열, Arrays 클래스, 향상된 for문 |
| 05 | Methods | 메서드 정의, 오버로딩, 가변인자, return |
| 06 | OOP | 클래스, 객체, 생성자, this, 접근 제어자 |
| 07 | Inheritance | 상속, super, 오버라이딩, Object 클래스 |
| 08 | Interface & Abstract | 인터페이스, 추상 클래스, 다형성 |
| 09 | Packages | 패키지, import, import static, classpath |
| 10 | Exceptions | 예외 처리, try-catch-finally, throws, 사용자 정의 예외 |
| 11 | Wrapper & String | Wrapper 클래스, String, StringBuilder, StringBuffer |
| 12 | Collections | List, Set, Map, Iterator, Comparable/Comparator |
| 13 | Generics | 제네릭 클래스/메서드, 타입 파라미터, 와일드카드 |
| 14 | Lambda & Stream | 람다 표현식, Stream API, Optional, 메서드 참조 |
| 15 | I/O | File, ByteStream, CharStream, BufferedReader, PrintWriter |
| 16 | Threads | Thread, Runnable, synchronized, wait/notify |
| 17 | JDBC | JDBC 드라이버, Connection, Statement, PreparedStatement |
| 18 | Networking | Socket, ServerSocket, InetAddress, URL |
| 19 | Date & Time | LocalDate, LocalTime, LocalDateTime, DateTimeFormatter |
| 20 | Testing & Annotations | JUnit, @Test, 어노테이션 정의/사용, Reflection 기초 |
