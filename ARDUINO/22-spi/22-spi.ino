#include <SPI.h>

// 74HC595 시프트 레지스터 연결 핀
const int LATCH_PIN = 10;  // RCLK: 8개 출력을 한꺼번에 갱신하는 핀
const int LED_COUNT = 8;

void updateShiftRegister(byte pattern) {
  // 래치를 내려 출력 갱신을 잠시 막는다
  digitalWrite(LATCH_PIN, LOW);
  // SPI로 1바이트 전송 (MOSI=11, SCK=13 사용)
  SPI.transfer(pattern);
  // 래치를 올려 8개 출력을 동시에 반영한다
  digitalWrite(LATCH_PIN, HIGH);
}

void setup() {
  Serial.begin(9600);
  pinMode(LATCH_PIN, OUTPUT);
  digitalWrite(LATCH_PIN, HIGH);

  SPI.begin();  // SPI 마스터 초기화
  Serial.println("SPI + 74HC595 시작 - LED 패턴 실행");
}

void loop() {
  // 왼쪽 → 오른쪽 점등
  for (int i = 0; i < LED_COUNT; i++) {
    updateShiftRegister(1 << i);
    delay(150);
  }

  // 오른쪽 → 왼쪽 점등
  for (int i = LED_COUNT - 1; i >= 0; i--) {
    updateShiftRegister(1 << i);
    delay(150);
  }

  // 모든 LED 점멸 (깜빡임)
  for (int i = 0; i < 3; i++) {
    updateShiftRegister(0xFF);
    delay(200);
    updateShiftRegister(0x00);
    delay(200);
  }

  // 체커보드 패턴
  updateShiftRegister(0xAA);
  delay(400);
  updateShiftRegister(0x55);
  delay(400);
  updateShiftRegister(0x00);
  delay(400);
}
