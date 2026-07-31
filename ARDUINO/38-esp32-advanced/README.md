# 38: ESP32 심화 — ESP32 Advanced

ESP32의 웹 서버, OTA(무선 업데이트), FreeRTOS 멀티태스킹을 다룹니다.

## 학습 내용
- ESP32 웹 서버 (WebServer.h)
- OTA 무선 업데이트 (ArduinoOTA)
- FreeRTOS 태스크로 진짜 멀티태스킹
- WebServer + OTA + 태스크 통합

## 웹 서버

`WebServer`로 핀 제어, 데이터 조회를 웹으로 제공합니다. 이번에는 GET 파라미터(`?led=1`)로 LED를 제어합니다.

```cpp
#include <WebServer.h>
WebServer server(80);

server.on("/led", []() {
  String v = server.arg("state");
  digitalWrite(LED_BUILTIN, v == "on");
  server.send(200, "text/plain", "ok");
});
server.begin();
```

## OTA (Over The Air)

OTA는 USB 연결 없이 WiFi로 펌웨어를 업데이트하는 기능입니다. ArduinoOTA 라이브러리만으로 구현할 수 있습니다.

```cpp
#include <ArduinoOTA.h>

ArduinoOTA.setHostname("esp32-dev");
ArduinoOTA.onStart([]() { Serial.println("OTA 시작"); });
ArduinoOTA.begin();          // 설정
ArduinoOTA.handle();         // 루프에서 호출
```

OTA 업데이트 시엔 **도구 → 포트**에서 `esp32-dev`(네트워크 포트)를 선택하고 업로드하면 됩니다.

## FreeRTOS 태스크

ESP32는 듀얼 코어로, `xTaskCreatePinnedToCore()`로 각 코어에 작업을 할당해 진짜 병렬 실행이 가능합니다. `loop()`는 코어 1에서 도는 것과 별개입니다.

```cpp
void blinkTask(void* param) {
  pinMode(LED_BUILTIN, OUTPUT);
  while (true) {
    digitalWrite(LED_BUILTIN, !digitalRead(LED_BUILTIN));
    vTaskDelay(500 / portTICK_PERIOD_MS);  // 블로킹 delay 대신 사용
  }
}

xTaskCreatePinnedToCore(blinkTask, "blink", 2048, NULL, 1, NULL, 0);
```

## 회로 연결

ESP32 DevKit에 USB 연결만으로 충분합니다. 내장 LED(핀 2)가 FreeRTOS 태스크에 의해 점멸합니다.

## 실행 방법

1. ESP32 보드 패키지를 설치합니다.
2. `.ino` 파일에서 `SSID`, `PASSWORD`를 수정하고 업로드합니다.
3. 시리얼 모니터(115200)에서 IP와 OTA 시작 로그를 확인합니다.
4. 브라우저로 `http://<IP>/led?state=on`을 열면 LED가 켜집니다.
5. **OTA 실습**: 네트워크 포트를 선택하고 프로그램을 다시 업로드합니다 (USB 없이 업데이트 성공).
6. LED 점멸(코어 0 태스크)이 웹 서버 처리와 간섭 없이 동시에 진행되는 것을 확인합니다.

## 응용 아이디어

- 원격 업데이트 가능한 제품 펌웨어
- 센서 수집(코어 0) + 웹 서버(코어 1) 분리
- 39장(IoT 클라우드) 대시보드 서버로 확장
