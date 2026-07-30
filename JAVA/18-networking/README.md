# 18: Networking — 네트워킹

## Socket / ServerSocket

TCP 기반 네트워크 통신의 기본 클래스입니다.

- `ServerSocket`: 서버가 클라이언트 연결을 기다림
- `Socket`: 클라이언트가 서버에 연결

## InetAddress

IP 주소를 표현하는 클래스입니다.

```java
InetAddress addr = InetAddress.getByName("localhost");
```

## URL / URLConnection

HTTP 통신을 위한 고수준 API입니다.

```java
URL url = new URL("https://api.example.com/data");
HttpURLConnection conn = (HttpURLConnection) url.openConnection();
```
