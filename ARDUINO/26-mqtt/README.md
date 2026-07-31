# 26: MQTT — Message Queuing Telemetry Transport

MQTT는 IoT에 특화된 경량 발행/구독(Pub/Sub) 메시지 프로토콜입니다. 브로커를 중심으로 토픽을 구독하고 발행합니다.

## 학습 내용
- MQTT 개념: 브로커, 토픽, 발행(publish)/구독(subscribe)
- PubSubClient 라이브러리
- 브로커 연결과 재연결 처리
- 토픽 구조 설계

## MQTT 동작 방식

클라이언트는 서로 직접 통신하지 않고 **브로커**를 경유합니다. 발행자는 토픽에 메시지를 올리고, 구독자는 해당 토픽을 구독하면 메시지를 받습니다.

```
센서 ──발행──> 브로커 ──전달──> 구독자 (대시보드/앱)
구독자 <─── 구독 topic ───┘
```

```cpp
#include <PubSubClient.h>
PubSubClient mqtt(client);

mqtt.setServer("broker.hivemq.com", 1883);
mqtt.setCallback(callback);
mqtt.subscribe("arduino/control");  // 구독
mqtt.publish("arduino/sensor", "24.5");  // 발행
```

## 콜백으로 메시지 수신

구독한 토픽에 메시지가 도착하면 `setCallback`에 등록한 함수가 호출됩니다.

```cpp
void callback(char* topic, byte* payload, unsigned int len) {
  String msg = "";
  for (int i = 0; i < len; i++) msg += (char)payload[i];
  if (msg == "ON") digitalWrite(LED_BUILTIN, HIGH);
}
```

## 토픽 설계

토픽은 `/`로 계층을 구분하며, 장치별·용도별로 체계적으로 만듭니다.

```
arduino/sensor/temp      → 온도 발행
arduino/control/led      → LED 제어 수신
home/room1/temp          → 집/방1/온도
```

`+`(한 단계 와일드카드), `#`(모든 하위 단계)로 여러 토픽을 한 번에 구독할 수 있습니다.

## 재연결 처리

네트워크가 끊기면 `mqtt.loop()` 호출 중에 연결을 재시도해야 합니다. `mqtt.connect()` 실패 시 잠시 후 다시 시도합니다.

```cpp
if (!mqtt.connected()) {
  if (mqtt.connect("arduino-client")) {
    mqtt.subscribe("arduino/control");
  }
}
mqtt.loop();
```

## 회로 연결

ESP8266/ESP32 보드에 USB 전원만 연결하면 됩니다. 별도 배선은 없습니다. 내장 LED가 토픽 `arduino/control` 메시지로 제어됩니다.

## 실행 방법

1. **라이브러리 관리자**에서 `PubSubClient`를 설치합니다.
2. `.ino` 파일에서 `SSID`, `PASSWORD`를 수정합니다.
3. 해당 보드(ESP8266/ESP32)로 업로드하고 시리얼 모니터(115200)를 엽니다.
4. 온도가 `arduino/sensor` 토픽으로 주기적으로 발행됩니다.
5. MQTT 클라이언트 앱이나 `mosquitto_pub`로 `arduino/control` 토픽에 `ON`/`OFF`를 보내 LED를 제어해 봅니다.

> 테스트 브로커: `broker.hivemq.com`(공개 브로커). 브로커 주소는 코드 상단에서 변경 가능합니다.

## 응용 아이디어

- 여러 센서 노드의 데이터를 한 대시보드로 집계
- 스마트폰 MQTT 앱으로 원격 제어
- 39장(IoT 클라우드)에서 대시보드 연동으로 확장
