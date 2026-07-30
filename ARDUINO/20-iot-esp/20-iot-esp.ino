#include <ESP8266WiFi.h>
#include <ESP8266HTTPClient.h>
#include <WiFiClient.h>

const char* SSID = "YourWiFi";
const char* PASSWORD = "YourPassword";
const char* SERVER_URL = "http://jsonplaceholder.typicode.com/todos/1";

void setup() {
  Serial.begin(115200);

  WiFi.begin(SSID, PASSWORD);
  Serial.print("Connecting to WiFi");

  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }

  Serial.println("\nWiFi connected");
  Serial.print("IP: ");
  Serial.println(WiFi.localIP());
}

void loop() {
  if (WiFi.status() == WL_CONNECTED) {
    WiFiClient client;
    HTTPClient http;

    http.begin(client, SERVER_URL);

    int httpCode = http.GET();

    if (httpCode > 0) {
      Serial.print("HTTP Response code: ");
      Serial.println(httpCode);

      if (httpCode == HTTP_CODE_OK) {
        String payload = http.getString();
        Serial.println("Response:");
        Serial.println(payload);
      }
    } else {
      Serial.print("HTTP Error: ");
      Serial.println(http.errorToString(httpCode).c_str());
    }

    http.end();
  } else {
    Serial.println("WiFi disconnected");
  }

  delay(10000);
}
