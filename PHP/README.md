# PHP 기초 강의 (20개 챕터)

PHP 프로그래밍 언어의 기초부터 고급 개념까지 학습할 수 있는 예제 모음입니다.

## 역사

PHP는 1994년 Rasmus Lerdorf가 개인 홈페이지를 관리하기 위해 만든 "Personal Home Page Tools"에서 시작했습니다. 1995년 PHP/FI 2.0이 공개되었고, 1997년 Zeev Suraski와 Andi Gutmans가 PHP 3.0의 파서를 재작성하면서 PHP의 의미가 "PHP: Hypertext Preprocessor"로 변경되었습니다. 2004년 PHP 5(Zend Engine 2)에서 객체 지향 기능이 대폭 강화되었고, 2015년 PHP 7(Zend Engine 3)에서 성능이 2배 향상되었습니다. 2020년 PHP 8.0에서는 JIT 컴파일러와 named arguments, attributes 등 현대적인 기능이 도입되었습니다. 전 세계 웹사이트의 약 75%가 PHP로 구동되고 있습니다.

## 특징

- **서버사이드 스크립트**: HTML에 직접 삽입 가능한 임베디드 언어
- **광범위한 호스팅**: 대부분의 웹 호스팅이 PHP를 기본 지원
- **방대한 CMS 생태계**: WordPress, Drupal, Joomla 등이 PHP 기반
- **다양한 데이터베이스 지원**: MySQL, PostgreSQL, SQLite, MongoDB 등
- **세션/쿠키 관리**: 내장된 세션 및 쿠키 처리 기능
- **컴포저(Composer)**: 현대적인 의존성 관리와 오토로딩
- **성능**: PHP 8.0+ JIT 컴파일러로 대폭 향상된 성능

## 실행

```bash
cd PHP/01-hello-world && php index.php
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | 기본 출력, 변수, 주석, PHP 기본 문법 |
| 02 | Variables | 변수, 데이터 타입, 형변환, 상수, null |
| 03 | Control Flow | if/else, switch, for/while/foreach, break/continue |
| 04 | Arrays | 인덱스 배열, 연관 배열, 다차원 배열, 배열 함수 |
| 05 | Functions | 함수 정의, 파라미터, return, 가변인자, 화살표 함수 |
| 06 | Strings | 문자열 함수, 포맷팅, 정규표현식, heredoc |
| 07 | OOP | 클래스, 객체, 생성자, 접근 제어자, static |
| 08 | Inheritance | 상속, parent, 오버라이딩, final, 클래스 상수 |
| 09 | Interface & Abstract | 인터페이스, 추상 클래스, trait |
| 10 | Superglobals | $_GET, $_POST, $_SESSION, $_COOKIE, $_SERVER |
| 11 | Forms & Validation | 폼 처리, 필터링, 유효성 검사, htmlspecialchars |
| 12 | File Handling | 파일 읽기/쓰기, 디렉토리, file_get_contents |
| 13 | Error Handling | 예외 처리, try-catch, throw, 사용자 정의 예외 |
| 14 | Sessions & Cookies | 세션 관리, 쿠키 설정/읽기/삭제 |
| 15 | Database (PDO) | PDO 연결, prepared statements, CRUD, 트랜잭션 |
| 16 | JSON & APIs | json_encode/decode, cURL, REST API 호출 |
| 17 | Date & Time | date(), DateTime, DateInterval, DateTimeZone |
| 18 | File Upload | 파일 업로드, 다중 업로드, MIME 검사 |
| 19 | Namespaces | 네임스페이스, use, autoload, Composer |
| 20 | MVC Pattern | 간단한 MVC 패턴, 라우팅, 컨트롤러 |
