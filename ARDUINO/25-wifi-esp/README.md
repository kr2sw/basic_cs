# 25: WiFi — ESP8266/ESP32

ESP8266/ESP32의 WiFi를 사용하여 HTTP 클라이언트(요청 보내기)와 HTTP 서버(응답 제공)를 구현합니다.

## 학습 내용
- ESP8266/ESP32 보드 설정과 WiFi 연결
- WiFi.begin(), WiFi.status(), WiFi.localIP()
- HTTP 클라이언트: GET 요청 보내기
- HTTP 서버: 웹페이지와 데이터 엔드포인트 제공

## 보드 선택과 코드 분기

ESP8266과 ESP32는 라이브러리 이름이 다릅니다. `#if defined()`로 두 보드를 모두 지원할 수 있습니다.

```cpp
#if defined(ESP8266)
  #include <ESP8266WiFi.h>
  #include <ESP8266WebServer.h>
  using WebServer = ESP8266WebServer;
#else
  #include <WiFi.h>
  #include <WebServer.h>
#endif
```

## WiFi 연결

```cpp
WiFi.begin(SSID, PASSWORD);
while (WiFi.status() != WL_CONNECTED) {
  delay(500);
  Serial.print(".");
}
Serial.println(WiFi.localIP());  // 접속할 주소 출력
```

## HTTP 서버

`WebServer` 객체를 만들고 경로별 핸들러를 등록합니다.

```cpp
WebServer server(80);
server.on("/", handleRoot);       // 루트 경로
server.on("/data", handleData);  // JSON 데이터 경로
server.begin();
```

요청이 오면 `server.handleClient()`가 등록된 핸들러를 호출합니다.

## 회로 연결

ESP8266(ESP-01) 또는 ESP32 보드에 USB로 전원을 공급합니다. 추가 배선은 필요 없습니다. 단, ESP-01의 경우 3.3V 전원과 CH_PD 핀 3.3V 연결이 필요합니다.

| 보드 | WiFi 핀 | 내장 LED |
|------|---------|----------|
| ESP8266 (NodeMCU) | 내장 | D0 (LED_BUILTIN) |
| ESP32 DevKit | 내장 | 핀 2 (LED_BUILTIN) |

## 실행 방법

1. **파일 → 환경설정**에서 보드 매니저 URL에 `https://arduino.esp8266.com/stable/package_esp8266com_index.json` 또는 ESP32 주소를 추가합니다.
2. **보드 매니저**에서 `ESP8266` / `ESP32` 패키지를 설치합니다.
3. `.ino` 파일에서 `SSID`, `PASSWORD`를 수정합니다.
4. **도구 → 보드**에서 해당 보드를 선택하고 보드레이트 115200으로 업로드합니다.
5. 시리얼 모니터(115200)에서 할당받은 IP를 확인하고 브라우저로 접속합니다.

## 응용 아이디어

- 스마트폰 브라우저로 센서 값 조회하는 웹 대시보드
- 버튼으로 LED를 켜고 끄는 웹 제어기
- 39장(IoT 클라우드)에서 HTTP POST 전송으로 확장
