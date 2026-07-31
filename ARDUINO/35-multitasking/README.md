# 35: 멀티태스킹 — Multitasking

`delay()` 없이 millis() 기반 스케줄러로 여러 작업을 동시에 실행하는 비블로킹 패턴을 다룹니다.

## 학습 내용
- blocking(차단) vs non-blocking(비차단) 코드
- millis() 타이밍 패턴
- Task 구조체 스케줄러
- 여러 센서/출력을 동시 처리

## delay()의 문제점

`delay()`는 그 시간 동안 프로그램을 멈춥니다. 두 가지 이상을 동시에 하려면(버튼 읽기 + LED 점멸) 다른 방법이 필요합니다.

```cpp
// bad: LED가 1초마다 깜빡이는 동안 버튼을 즉시 응답할 수 없다
while (true) {
  digitalWrite(LED, HIGH); delay(1000);
  digitalWrite(LED, LOW);  delay(1000);
}
```

## millis() 비블로킹 패턴

"마지막 실행 시각"을 기억해 두고, 경과 시간이 주기를 넘었을 때만 작업을 실행합니다.

```cpp
unsigned long lastBlink = 0;
if (millis() - lastBlink >= 500) {
  lastBlink = millis();
  digitalWrite(LED_PIN, !digitalRead(LED_PIN));
}
```

## 스케줄러 만들기

작업(함수 포인터 + 주기)을 배열로 관리하면 확장 가능한 스케줄러가 됩니다.

```cpp
struct Task {
  void (*func)();   // 실행할 함수
  unsigned long interval;
  unsigned long lastRun;
};

Task tasks[] = {
  { blink,  500, 0 },
  { readSensor, 2000, 0 },
  { printStatus, 5000, 0 },
};
```

## 회로 연결

| 부품 | Arduino Uno |
|------|-------------|
| LED (+220Ω) | D13 (내장) |
| 가변저항 | A0 (센서 값 샘플링) |
| 버튼 | D2, 반대쪽 GND |

## 실행 방법

1. 이 챕터의 `.ino`를 업로드합니다.
2. LED가 500ms마다 깜빡이는 동안, 가변저항 값이 2초마다 갱신되고 상태가 5초마다 출력됩니다.
3. 시리얼 모니터(9600)에서 각 작업이 서로 다른 주기로 실행되는 것을 확인합니다.
4. 버튼을 눌러도 LED 점멸이 멈추지 않는 것을 관찰합니다 (비블로킹 동작).

## 응용 아이디어

- 여러 LED가 다른 주기로 깜빡이는 애니메이션
- 센서 수집 + 디스플레이 갱신 + 통신을 하나의 루프에서 처리
- 38장(ESP32 FreeRTOS)에서 진짜 멀티태스킹으로 확장
