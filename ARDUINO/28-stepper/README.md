# 28: 스테퍼 모터 — Stepper Motor

A4988 드라이버로 스테퍼 모터를 정밀하게 제어합니다. 스텝(STEP)/방향(DIR) 신호를 이해하고 위치 제어를 구현합니다.

## 학습 내용
- 스테퍼 모터 동작 원리와 A4988 드라이버
- STEP/DIR/ENABLE 핀 제어
- 스텝 각도와 회전 계산
- 시리얼 명령으로 위치 제어

## 스테퍼 모터와 A4988

스테퍼 모터는 전자석을 순서대로 켜서 회전축을 일정 각도씩 돌립니다. A4988 드라이버는 Arduino의 STEP/DIR 펄스를 모터 코일 전류로 변환합니다. 모터 한 회전에 필요한 스텝 수는 스텝 각도로 계산합니다.

```
NEMA17(1.8°) + 마이크로스텝 1/16 → 360 / 1.8 × 16 = 3200 스텝/회전
```

## STEP/DIR 제어

DIR 핀으로 회전 방향을 정하고, STEP 핀에 펄스를 주면 한 스텝씩 움직입니다.

```cpp
digitalWrite(DIR_PIN, direction);          // 방향
digitalWrite(STEP_PIN, HIGH);             // 펄스 상승
delayMicroseconds(1000);
digitalWrite(STEP_PIN, LOW);              // 펄스 하강
delayMicroseconds(1000);
```

`delayMicroseconds()`로 펄스 폭을 조절하면 속도가 달라집니다.

## 스텝 계산

```cpp
const int STEPS_PER_REV = 3200;   // 1/16 마이크로스텝 기준
float degrees = 90.0;
int steps = STEPS_PER_REV * (degrees / 360.0);
```

## 전원 및 커패시터

A4988은 8~35V 모터 전원을 사용하고, 전원에 **100µF 커패시터**를 병렬 연결해야 동작이 안정적입니다. 전류 제한(리미트)도 전원 전에 조정합니다.

## 회로 연결 (A4988 + NEMA17)

| A4988 | Arduino Uno | NEMA17 |
|-------|-------------|--------|
| STEP | D8 | |
| DIR | D9 | |
| ENABLE | D10 | |
| VDD | 5V | |
| GND | GND | |
| VMOT | 8-35V(+) | |
| GND(VMOT 옆) | GND(-) | |
| 1A/1B/2A/2B | | 모터 코일 |
| MS1,MS2,MS3 | GND(또는 분리) | 마이크로스텝 설정 |

> 마이크로스텝: MS1~MS3를 모두 LOW로 두면 풀스텝, 모두 HIGH면 1/16 스텝입니다.

## 실행 방법

1. 위 회로를 연결하고 이 챕터의 `.ino`를 업로드합니다.
2. 시리얼 모니터(9600)에 명령을 입력합니다.
   - `f 90` → 시계 방향 90도 회전
   - `b 180` → 반시계 방향 180도 회전
3. 모터가 지정한 각도만큼 정확히 회전합니다.

## 응용 아이디어

- 3D 프린터 축, CNC, 카메라 슬라이더
- AccelStepper 라이브러리로 가감속 부드러운 제어
- 34장(상태 머신)과 결합한 자동 도어/게이트
