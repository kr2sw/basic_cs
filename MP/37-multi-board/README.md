# 37: 멀티보드 — ESP32 + Pico Communication, Framework

## 개요

한 프로젝트에서 두 보드가 협력해야 하는 경우가 많습니다. ESP32(Wi-Fi)는 네트워크와 클라우드를 담당하고, Pico는 센서 수집을 담당하는 식으로 **역할을 나눕니다**. 이번 레슨에서는 **UART(직렬)** 프로토콜로 두 보드를 연결하고, 안정적인 통신을 위한 **프레임워크(프레임 프로토콜)** 를 설계합니다.

## 역할 분담 (분산 아키텍처)

```
┌───────────────┐   UART   ┌──────────────────┐
│ Raspberry Pi Pico │◄────────►│ ESP32 (게이트웨이) │── Wi-Fi → 클라우드
│  · 센서 수집     │  TX/RX   │  · MQTT/HTTP      │
│  · 저전력 슬립   │  GND     │  · 명령 중계       │
└───────────────┘          └──────────────────┘
```

- **센서 노드(Pico)**: 데이터 수집, 배터리 절약
- **게이트웨이(ESP32)**: Wi-Fi 연결, 클라우드 전송, 명령 하달

## UART 통신 기초

```python
from machine import UART, Pin
uart = UART(1, baudrate=9600, tx=Pin(4), rx=Pin(5))
uart.write(b"data")          # 송신
data = uart.read()           # 수신
```

TX/RX는 반드시 **교차 연결**합니다. 한 보드의 TX → 상대 보드의 RX, GND는 공통으로 묶습니다.

## 프레임 프로토콜 설계

원시 바이트를 그냥 주고받으면 데이터 경계를 구분할 수 없습니다. **프레임(틀)** 을 정의합니다.

```text
[START 0x7E][TYPE 1B][LENGTH 1B][PAYLOAD nB][CHECKSUM 1B][END 0x7F]
```

```python
def make_frame(msg_type, payload: bytes) -> bytes:
    checksum = (msg_type + len(payload) + sum(payload)) & 0xFF
    return b"\x7e" + bytes([msg_type, len(payload)]) + payload \
           + bytes([checksum]) + b"\x7f"
```

START/END로 프레임 경계를 찾고, CHECKSUM으로 수신 오류를 감지합니다.

## JSON 페이로드

센서 값과 메타데이터는 JSON으로 직렬화해 페이로드에 담습니다.

```python
import json
payload = json.dumps({"temp": 21.5, "hum": 44.2}).encode()
```

## 실행/업로드 방법

1. 보드 A(Pico)에는 `MP/37-multi-board/main.py`의 센서 모드를, 보드 B(ESP32)에는 게이트웨이 모드를 업로드합니다.
2. **Thonny IDE**: 각 보드를 연결해 실행(F5).
3. **ampy**:
   ```bash
   ampy --port COM3 put MP/37-multi-board/main.py   # 보드별 직렬 포트로
   ```
4. 게이트웨이의 시리얼에서 센서 프레임이 수신되는 것을 확인합니다.

## 핀 연결 (Pico ↔ ESP32)

| Pico | ESP32 |
|------|-------|
| TX (GPIO4) | RX (GPIO16) |
| RX (GPIO5) | TX (GPIO17) |
| GND | GND |

## 핵심 개념 요약

- 역할 분담: 센서 노드 + 게이트웨이 아키텍처
- UART TX/RX 교차 연결, GND 공통
- START/CHECKSUM/END 프레임으로 안정적 통신
- JSON 페이로드로 데이터 구조화
