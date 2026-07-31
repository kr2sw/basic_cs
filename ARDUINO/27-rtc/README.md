# 27: 실시간 시계 — Real Time Clock

DS3231 RTC 모듈로 실제 날짜와 시간을 읽고, 시간 동기화와 알람을 구현합니다.

## 학습 내용
- RTC 개념과 배터리 백업
- RTClib 라이브러리로 DS3231 사용
- 시간 설정(동기화)과 읽기
- 알람 기능 구현

## RTC와 DS3231

RTC(Real-Time Clock)는 전원이 꺼져도 CR2032 배터리로 시간을 유지합니다. DS3231은 온도 보정이 되어 정확도가 높고 온도 센서도 내장되어 있습니다. I2C 인터페이스를 사용합니다.

```cpp
#include <Wire.h>
#include "RTClib.h"
RTC_DS3231 rtc;

rtc.begin();  // I2C 연결 확인
```

## 시간 읽기

`rtc.now()`는 `DateTime` 객체를 반환하고, 연/월/일/시/분/초를 각각 가져올 수 있습니다.

```cpp
DateTime now = rtc.now();
Serial.print(now.year());
Serial.print("-");
Serial.print(now.month());
Serial.print("-");
Serial.print(now.day());
Serial.print(" ");
Serial.print(now.hour());
Serial.print(":");
Serial.println(now.minute());
```

## 시간 동기화

배터리가 방전되어 전원이 손실되면 `rtc.lostPower()`가 true를 반환합니다. 컴파일 시각으로 1회 동기화할 수 있습니다.

```cpp
if (rtc.lostPower()) {
  rtc.adjust(DateTime(F(__DATE__), F(__TIME__)));
}
```

시리얼 명령으로 원하는 시각을 넣는 방법도 사용할 수 있습니다.

## 알람

DS3231은 내장 알람(인터럽트)을 지원합니다. 소프트웨어 방식으로는 매 루프에서 현재 시각을 확인해 특정 시각이 되면 동작시키는 폴링을 사용할 수 있습니다.

```cpp
if (now.hour() == 12 && now.minute() == 0 && now.second() == 0) {
  digitalWrite(ALARM_PIN, HIGH);  // 12:00 정각에 알림
}
```

## 회로 연결 (DS3231)

| DS3231 모듈 | Arduino Uno |
|-------------|-------------|
| VCC | 5V |
| GND | GND |
| SDA | A4 |
| SCL | A5 |
| SQW/INT | (선택) D2 — 인터럽트/알람 핀 |

## 실행 방법

1. **라이브러리 관리자**에서 `RTClib`(Adafruit)를 설치합니다.
2. 모듈에 CR2032 배터리가 장착되어 있는지 확인합니다.
3. 이 챕터의 `.ino`를 업로드하면 시리얼 모니터(9600)에 매초 현재 시간이 출력됩니다.
4. 배터리가 방전된 첫 실행이면 컴파일 시각으로 자동 동기화됩니다.

## 응용 아이디어

- SD 카드 로깅(23장)에 실제 시각 타임스탬프 기록
- 자동 관수/급이 시스템 스케줄러
- 전원을 껐다 켜도 시간이 유지되는 시계/알람 장치
