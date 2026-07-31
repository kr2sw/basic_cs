// MQTT Pub/Sub 예제 - ESP8266 / ESP32 지원
#if defined(ESP8266)
#include <ESP8266WiFi.h>
#else
#include <WiFi.h>
#endif
#include <PubSubClient.h>

const char* SSID = "YourWiFi";
const char* PASSWORD = "YourPassword";

// 공개 테스트 브로커
const char* MQTT_SERVER = "broker.hivemq.com";
const int MQTT_PORT = 1883;

const char* TOPIC_SENSOR = "arduino/sensor";       // 발행(온도)
const char* TOPIC_CONTROL = "arduino/control";     // 구독(LED 제어)

WiFiClient wifiClient;
PubSubClient mqtt(wifiClient);

unsigned long lastPublish = 0;
float fakeTemp = 20.0;

// 구독 토픽에 메시지가 도착하면 호출되는 콜백
void mqttCallback(char* topic, byte* payload, unsigned int length) {
  String msg = "";
  for (int i = 0; i < length; i++) {
    msg += (char)payload[i];
  }

  Serial.print("수신 토픽: ");
  Serial.print(topic);
  Serial.print(", 메시지: ");
  Serial.println(msg);

  if (msg == "ON") {
    digitalWrite(LED_BUILTIN, HIGH);
  } else if (msg == "OFF") {
    digitalWrite(LED_BUILTIN, LOW);
  }
}

void reconnect() {
  while (!mqtt.connected()) {
    Serial.print("MQTT 브로커 연결 시도...");
    if (mqtt.connect("arduino-client-001")) {
      Serial.println(" 연결됨");
      mqtt.subscribe(TOPIC_CONTROL);  // 제어 토픽 구독
      Serial.print("구독: ");
      Serial.println(TOPIC_CONTROL);
    } else {
      Serial.print(" 실패 (rc=");
      Serial.print(mqtt.state());
      Serial.println(") 5초 후 재시도");
      delay(5000);
    }
  }
}

void setup() {
  Serial.begin(115200);
  pinMode(LED_BUILTIN, OUTPUT);

  WiFi.begin(SSID, PASSWORD);
  Serial.print("WiFi 연결 중");
  while (WiFi.status() != WL_CONNECTED) {
    delay(500);
    Serial.print(".");
  }
  Serial.println(" 연결됨");

  mqtt.setServer(MQTT_SERVER, MQTT_PORT);
  mqtt.setCallback(mqttCallback);
}

void loop() {
  if (!mqtt.connected()) {
    reconnect();
  }
  mqtt.loop();  // 수신 메시지 처리와 연결 유지

  // 5초마다 온도 발행
  if (millis() - lastPublish > 5000) {
    lastPublish = millis();
    fakeTemp += random(-10, 11) / 10.0;

    String payload = String(fakeTemp, 1);
    Serial.print("발행 [");
    Serial.print(TOPIC_SENSOR);
    Serial.print("]: ");
    Serial.println(payload);

    mqtt.publish(TOPIC_SENSOR, payload.c_str());
  }
}
