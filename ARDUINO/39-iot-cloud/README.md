# 39: IoT 클라우드 — IoT Cloud

센서 데이터를 HTTP POST로 클라우드 API에 전송하고 대시보드 연동 개념을 학습합니다.

## 학습 내용
- HTTP POST와 JSON 형식
- ESP8266/ESP32에서 HTTPS/HTTP POST 전송
- 클라우드 API 구조 (엔드포인트, API 키)
- 대시보드 연동 개념

## HTTP POST와 JSON

GET이 데이터를 "가져오는" 것이라면 POST는 데이터를 "보내는" 것입니다. IoT 장치는 센서 값을 JSON으로 묶어 서버에 전송합니다.

```json
{ "device": "sensor-01", "temp": 24.5, "hum": 55.0 }
```

## POST 요청 코드

ESP32/ESP8266은 `HTTPClient`로 POST를 보냅니다. 헤더에 데이터 타입과 API 키를 실습니다.

```cpp
#include <HTTPClient.h>
HTTPClient http;
http.begin(client, SERVER_URL);
http.addHeader("Content-Type", "application/json");
http.addHeader("X-API-Key", API_KEY);
int code = http.POST(json);
Serial.println(http.getString());
http.end();
```

> ESP8266은 `http.begin(client, url)` 형태가 필요합니다.

## 대시보드 연동

수집된 데이터는 서버가 저장하고, 대시보드(웹/앱)가 그래프로 보여줍니다.

```
센서 → POST JSON → 클라우드 API(DB 저장) → 대시보드 그래프
```

- **ThingSpeak**: 채널 + 필드 개념으로 몇 줄이면 연동되는 무료 IoT 서비스
- **자체 서버**: 38장의 ESP32 웹 서버를 확장하거나 라즈베리파이 + Node-RED 사용
- **상용 플랫폼**: AWS IoT Core, Azure IoT Hub, Google Cloud IoT

## 회로 연결

ESP8266/ESP32 보드에 USB 전원만 연결하면 됩니다. 가변저항을 ADC 핀(A0)에 연결해 실제 센서 값을 보낼 수 있습니다.

| 보드 | ADC 핀 |
|------|--------|
| ESP8266 (NodeMCU) | A0 |
| ESP32 | GPIO34 |

## 실행 방법

1. `.ino` 파일에서 `SSID`, `PASSWORD`, `SERVER_URL`를 수정합니다.
2. ThingSpeak를 사용한다면 채널의 Write API Key를 `API_KEY`에 입력합니다.
3. ESP 보드로 업로드하고 시리얼 모니터(115200)에서 POST 응답(HTTP 코드)을 확인합니다.
4. HTTP 200 응답을 받으면 데이터가 서버에 저장된 것입니다.
5. 테스트 엔드포인트(`https://httpbin.org/post`)로 보내면 서버가 받은 JSON을 그대로 반환합니다.

## 응용 아이디어

- 온습도 센서를 연결해 실내 모니터링 대시보드 구축
- 여러 장치의 데이터를 한 대시보드로 집계 (26장 MQTT와 결합)
- 이메일/앱 푸시 알림 (임계값 초과 감지)
