// ESP8266 / ESP32 WiFi + HTTP 서버/클라이언트 예제
#if defined(ESP8266)
#include <ESP8266WiFi.h>
#include <ESP8266WebServer.h>
typedef ESP8266WebServer MyServer;
#else
#include <WiFi.h>
#include <WebServer.h>
typedef WebServer MyServer;
#endif

const char* SSID = "YourWiFi";
const char* PASSWORD = "YourPassword";

MyServer server(80);

// 임의의 센서 값 (온도 시뮬레이션)
float fakeTemp = 24.5;

// 루트 경로: HTML 웹페이지 응답
void handleRoot() {
  String html = "<!DOCTYPE html><html><head><meta charset='utf-8'>";
  html += "<title>ESP WiFi</title></head><body>";
  html += "<h1>Arduino 중급 25장 - WiFi</h1>";
  html += "<p>온도: ";
  html += fakeTemp;
  html += " C</p>";
  html += "<a href='/data'>JSON 데이터 보기</a>";
  html += "</body></html>";
  server.send(200, "text/html", html);
}

// /data 경로: JSON 형식 데이터 응답
void handleData() {
  String json = "{\"temp\": " + String(fakeTemp, 1) + ", \"unit\": \"C\"}";
  server.send(200, "application/json", json);
}

void setup() {
  Serial.begin(115200);

  // WiFi 연결
  Serial.print("WiFi 연결 중");
  WiFi.begin(SSID, PASSWORD);
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println("\n연결 완료");
  Serial.print("접속 주소: http://");
  Serial.println(WiFi.localIP());

  // HTTP 서버 라우트 등록 및 시작
  server.on("/", handleRoot);
  server.on("/data", handleData);
  server.begin();
  Serial.println("HTTP 서버 시작 (포트 80)");
}

void loop() {
  server.handleClient();  // 들어온 요청 처리

  // 센서 값 갱신 (천천히 변하는 시뮬레이션)
  static unsigned long lastUpdate = 0;
  if (millis() - lastUpdate > 2000) {
    lastUpdate = millis();
    fakeTemp += random(-10, 11) / 10.0;
  }
}
