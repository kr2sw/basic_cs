// ESP32 심화: 웹 서버 + OTA + FreeRTOS 태스크
#if defined(ESP32)
#include <WiFi.h>
#include <WebServer.h>
#include <ArduinoOTA.h>
#else
#error "이 챕터(38)는 ESP32 보드 전용 예제입니다."
#endif

const char* SSID = "YourWiFi";
const char* PASSWORD = "YourPassword";

WebServer server(80);

// --- FreeRTOS 태스크: 코어 0에서 LED 점멸 (독립 실행) ---
void blinkTask(void* param) {
  pinMode(LED_BUILTIN, OUTPUT);
  while (true) {
    digitalWrite(LED_BUILTIN, !digitalRead(LED_BUILTIN));
    vTaskDelay(500 / portTICK_PERIOD_MS);  // 500ms 대기 (delay 대신)
  }
}

void handleRoot() {
  String html = "<h1>ESP32 Server</h1>";
  html += "<a href='/led?state=on'>LED ON</a><br>";
  html += "<a href='/led?state=off'>LED OFF</a><br>";
  server.send(200, "text/html", html);
}

void handleLed() {
  String state = server.arg("state");
  if (state == "on") {
    digitalWrite(LED_BUILTIN, HIGH);
    server.send(200, "text/plain", "LED ON");
  } else if (state == "off") {
    digitalWrite(LED_BUILTIN, LOW);
    server.send(200, "text/plain", "LED OFF");
  } else {
    server.send(400, "text/plain", "unknown state");
  }
}

void setup() {
  Serial.begin(115200);

  // WiFi 연결
  WiFi.begin(SSID, PASSWORD);
  Serial.print("WiFi 연결 중");
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println();
  Serial.print("접속 주소: http://");
  Serial.println(WiFi.localIP());

  // 웹 서버 라우트 등록
  server.on("/", handleRoot);
  server.on("/led", handleLed);
  server.begin();
  Serial.println("웹 서버 시작");

  // OTA 설정 및 시작
  ArduinoOTA.setHostname("esp32-dev");
  ArduinoOTA.onStart([]() { Serial.println("OTA 업데이트 시작"); });
  ArduinoOTA.begin();
  Serial.println("OTA 준비 완료 (포트: esp32-dev)");

  // FreeRTOS 태스크 생성 (코어 0에 고정)
  xTaskCreatePinnedToCore(
    blinkTask,   // 실행할 함수
    "blink",     // 태스크 이름
    2048,        // 스택 크기 (바이트)
    NULL,        // 파라미터
    1,           // 우선순위
    NULL,        // 태스크 핸들
    0            // 코어 0
  );
  Serial.println("블링크 태스크 시작 (코어 0)");
}

void loop() {
  server.handleClient();  // 웹 요청 처리 (코어 1의 loop)
  ArduinoOTA.handle();    // OTA 요청 처리
}
