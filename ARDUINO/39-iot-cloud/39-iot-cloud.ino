// IoT 클라우드 연동 예제 - HTTP POST JSON 전송
#if defined(ESP8266)
#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <WiFiClient.h>
#else
#include <WiFi.h>
#include <HTTPClient.h>
#endif

const char* SSID = "YourWiFi";
const char* PASSWORD = "YourPassword";

// POST 전송 대상 (테스트: httpbin.org는 받은 JSON을 그대로 반환)
const char* SERVER_URL = "https://httpbin.org/post";
const char* API_KEY = "YOUR_API_KEY";  // 대시보드/플랫폼 키

// 가상 센서 값
float temp = 23.0;
float hum = 50.0;
int light = 300;

unsigned long lastSend = 0;

void connectWiFi() {
  WiFi.begin(SSID, PASSWORD);
  Serial.print("WiFi 연결 중");
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println();
  Serial.print("IP: ");
  Serial.println(WiFi.localIP());
}

void sendData() {
  // JSON 페이로드 생성
  String json = "{";
  json += "\"device\":\"sensor-01\",";
  json += "\"temp\":" + String(temp, 1) + ",";
  json += "\"hum\":" + String(hum, 1) + ",";
  json += "\"light\":" + String(light);
  json += "}";

  if (WiFi.status() == WL_CONNECTED) {
    HTTPClient http;

#if defined(ESP8266)
    WiFiClient client;
    http.begin(client, SERVER_URL);
#else
    http.begin(SERVER_URL);
#endif

    http.addHeader("Content-Type", "application/json");
    http.addHeader("X-API-Key", API_KEY);

    Serial.print("POST 전송: ");
    Serial.println(json);

    int httpCode = http.POST(json);

    if (httpCode > 0) {
      Serial.print("응답 코드: ");
      Serial.println(httpCode);
      String response = http.getString();
      Serial.println("응답:");
      Serial.println(response.substring(0, 200));  // 긴 응답은 앞부분만
    } else {
      Serial.print("전송 실패: ");
      Serial.println(http.errorToString(httpCode));
    }

    http.end();
  } else {
    Serial.println("WiFi 연결 끊김");
    connectWiFi();
  }
}

void setup() {
  Serial.begin(115200);
  connectWiFi();
  Serial.println("클라우드 전송 준비 완료 (60초 간격)");
}

void loop() {
  // 센서 값 갱신 (시뮬레이션)
  temp += random(-5, 6) / 10.0;
  hum += random(-3, 4);
  light += random(-10, 11);

  // 60초마다 전송
  if (millis() - lastSend >= 60000) {
    lastSend = millis();
    sendData();
  }
}
