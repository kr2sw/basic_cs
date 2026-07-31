#include <Wire.h>
#include "RTClib.h"

RTC_DS3231 rtc;  // 실시간 시계 모듈

const int ALARM_PIN = 13;   // 알람 시각에 켜지는 LED
const int HOUR = 12;        // 알람 시
const int MINUTE = 0;       // 알람 분

void setup() {
  Serial.begin(9600);
  while (!Serial) { delay(10); }
  pinMode(ALARM_PIN, OUTPUT);

  if (!rtc.begin()) {
    Serial.println("RTC 초기화 실패! 배선 확인 (SDA=A4, SCL=A5)");
    while (1) delay(10);
  }

  // 배터리 방전으로 시간이 유실되면 컴파일 시각으로 동기화
  if (rtc.lostPower()) {
    Serial.println("RTC 전원 손실 감지 - 컴파일 시각으로 동기화");
    rtc.adjust(DateTime(F(__DATE__), F(__TIME__)));
  }

  Serial.print("알람 설정: ");
  Serial.print(HOUR);
  Serial.print(":");
  Serial.println(MINUTE);
  Serial.println("시간 정보를 출력합니다.");
}

void loop() {
  DateTime now = rtc.now();

  // 날짜/시간을 "YYYY-MM-DD HH:MM:SS" 형식으로 출력
  char buf[24];
  snprintf(buf, sizeof(buf), "%04d-%02d-%02d %02d:%02d:%02d",
           now.year(), now.month(), now.day(),
           now.hour(), now.minute(), now.second());
  Serial.print("[");
  Serial.print(buf);
  Serial.print("] ");

  // 온도 보정 센서: RTC 온도 출력
  Serial.print("RTC온도: ");
  Serial.print(rtc.getTemperature());
  Serial.println(" C");

  // 알람 확인 (폴링 방식): 정해진 시:분에 LED 켜기
  static bool alarmTriggered = false;
  if (now.hour() == HOUR && now.minute() == MINUTE && now.second() < 5) {
    digitalWrite(ALARM_PIN, HIGH);
    if (!alarmTriggered) {
      Serial.println(">>> 알람 발생! (정각 알림)");
      alarmTriggered = true;
    }
  } else {
    digitalWrite(ALARM_PIN, LOW);
    alarmTriggered = false;
  }

  delay(1000);
}
