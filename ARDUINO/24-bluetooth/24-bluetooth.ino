#include <SoftwareSerial.h>

// HC-05 블루투스 모듈 연결 (SoftwareSerial)
// HC-05 TX → 핀2, HC-05 RX → 핀3
SoftwareSerial bt(2, 3);

// AT 명령 모드 여부 (EN 핀 HIGH 시 true)
const bool AT_MODE = false;

void setup() {
  // 하드웨어 시리얼: PC와 통신
  Serial.begin(9600);

  // 소프트웨어 시리얼: 블루투스 모듈과 통신
  bt.begin(9600);

  if (AT_MODE) {
    Serial.println("AT 명령 모드 - 명령을 입력하세요 (예: AT, AT+NAME=MyBT)");
  } else {
    Serial.println("통신 모드 - 블루투스로 수신된 데이터를 출력합니다");
  }
}

void loop() {
  // 블루투스 → PC 시리얼로 전달
  if (bt.available()) {
    char c = bt.read();
    Serial.write(c);
  }

  // PC 시리얼 → 블루투스로 전달 (AT 명령 또는 데이터 송신)
  if (Serial.available()) {
    char c = Serial.read();
    bt.write(c);
  }

  // 일정 간격으로 블루투스 경유 데이터 전송 예시
  static unsigned long lastSend = 0;
  if (!AT_MODE && millis() - lastSend > 5000) {
    lastSend = millis();
    bt.print("hello from arduino (t=");
    bt.print(millis());
    bt.println(")");
    Serial.println("(데이터를 블루투스로 전송했습니다)");
  }
}
