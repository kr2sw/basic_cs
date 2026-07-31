# 24: 블루투스 — Bluetooth

HC-05/HM-10 블루투스 모듈을 사용하여 스마트폰이나 PC와 무선 데이터를 주고받습니다.

## 학습 내용
- 블루투스 모듈 종류 (HC-05: 클래식, HM-10: BLE)
- AT 명령으로 모듈 설정
- SoftwareSerial로 UART 통신
- 데이터 송수신과 에코 테스트

## 블루투스 모듈 이해

- **HC-05**: 클래식 블루투스(2.0), UART, 페어링 없이도 송수신 가능. 보드레이트 기본 9600
- **HM-10**: BLE(4.0), 스마트폰 앱과 연결 가능, 전력 소모 낮음
- **AT 명령 모드**: 설정 변경(이름, 보드레이트, 마스터/슬레이브)은 EN 핀을 HIGH로 올려 진입

```
AT 명령 예시
AT            → OK
AT+NAME=MyBT  → 이름 변경
AT+UART=9600  → 보드레이트 변경
AT+ROLE=0     → 슬레이브 모드
```

## SoftwareSerial

Uno에는 하드웨어 UART가 1개뿐이므로, 추가 시리얼은 `SoftwareSerial`로 만듭니다.

```cpp
#include <SoftwareSerial.h>
SoftwareSerial bt(2, 3);  // RX=2, TX=3

void setup() {
  bt.begin(9600);  // 블루투스 모듈과 동일한 보드레이트
}
```

## 송수신 구조

블루투스 모듈은 Arduino의 UART와 연결되어 있어서, `Serial`과 `bt` 두 경로를 서로 릴레이하면 무선 터널이 됩니다.

```cpp
if (bt.available()) Serial.write(bt.read());   // 무선 → 시리얼
if (Serial.available()) bt.write(Serial.read()); // 시리얼 → 무선
```

## 회로 연결 (HC-05)

| HC-05 | Arduino Uno |
|-------|-------------|
| VCC | 5V (3.3V 권장 시 3.3V) |
| GND | GND |
| TX | 2 (SoftwareSerial RX) |
| RX | 3 (SoftwareSerial TX, 전압 분배 권장) |
| EN | 5V (AT 명령 모드 진입 시) |

> HC-05 RX는 3.3V 로직입니다. 5V 핀에 직접 연결하면 통신이 불안정할 수 있으므로 저항 분배기(1kΩ + 2kΩ)를 권장합니다.

## 실행 방법

1. 위 회로를 연결하고 이 챕터의 `.ino`를 업로드합니다.
2. **AT 명령 모드**: EN 핀을 5V로 올린 상태에서 시리얼 모니터(9600, NL&CR)에 `AT`를 입력하면 `OK`가 응답합니다.
3. **통신 모드**: EN을 GND(또는 분리)로 내리고 전원을 재인가한 뒤, 스마트폰 블루투스 설정에서 `MYBT`(예시)에 페어링합니다.
4. 블루투스 터미널 앱에서 문자를 보내면 시리얼 모니터에 표시되고, 시리얼 모니터 입력은 앱으로 전송됩니다.

## 응용 아이디어

- 스마트폰으로 LED/모터 원격 제어
- HM-10 + BLE 앱으로 센서 데이터 모니터링
- 시리얼 브리지로 로봇 원격 조종
