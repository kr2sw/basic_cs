# 23: 고급 BLE — GATT Services, Custom Characteristics

## 개요

BLE(Bluetooth Low Energy)의 핵심은 **GATT(Generic Attribute Profile)** 입니다. 기초 과정에서 광고/연결만 다뤘다면, 이번 레슨에서는 **서비스(Service)** 와 **캐릭터리스틱(Characteristic)** 을 직접 정의하고, 노티피케이션(알림)으로 데이터를 실시간 전송하는 커스텀 GATT 서버를 만듭니다.

## GATT 구조

```
Device (Peripheral)
└── GATT Server
    ├── Service 0x180D (Heart Rate)
    │   ├── Characteristic 0x2A37 (Heart Rate Measurement)
    │   │   ├── Properties: Read, Notify
    │   │   └── Value: 72 bpm
    │   └── ...
    └── Service (커스텀 UUID)
        └── Characteristic (커스텀 UUID)
```

- **Service**: 기능의 묶음, **Characteristic**: 실제 데이터 값(Value)과 속성(Properties)
- Properties: `READ`, `WRITE`, `NOTIFY`, `INDICATE` 등
- UUID: 표준 서비스는 16비트(0x180D), 커스텀은 128비트 UUID 사용

## 서비스/캐릭터리스틱 정의

```python
from bluetooth import BLE
import struct

# 커스텀 128비트 UUID: 자이로 각도용
SERVICE_UUID = bluetooth.UUID(0x1234)          # 테스트용 커스텀 16비트
CHAR_TEMP_UUID = bluetooth.UUID(0x5678)

SENSOR_SERVICE = (
    SERVICE_UUID,
    ((CHAR_TEMP_UUID, bluetooth.FLAG_READ | bluetooth.FLAG_NOTIFY),),
)

BLE().gatts_register_services((SENSOR_SERVICE,))
```

## 노티피케이션 (알림)

센서 값이 바뀔 때 중앙 장치(스마트폰/다른 보드)에 **알림**을 보내면, 읽기 요청 없이도 데이터가 전달됩니다.

```python
value = struct.pack("<f", temperature)        # float 4바이트로 인코딩
ble.gatts_write(handle, value)                # 속성 값 갱신
ble.gatts_notify(0, handle)                   # 연결된 클라이언트에게 알림
```

## 연결 상태 처리

`irq` 이벤트 핸들러로 연결/연결 해제/쓰기 이벤트를 감지합니다. 연결되면 광고를 중단해 전력을 아낍니다.

## 실행/업로드 방법

1. **Thonny IDE**: 보드 연결 후 `MP/23-ble-advanced/main.py` 실행(F5).
2. **ampy**:
   ```bash
   ampy --port COM3 put MP/23-ble-advanced/main.py
   ampy --port COM3 run MP/23-ble-advanced/main.py
   ```
3. 스마트폰의 nRF Connect 앱이나 lightBlue로 **LED Service**를 검색해 캐릭터리스틱을 읽고 쓰면, 시리얼에 출력됩니다.

## 핵심 개념 요약

- `bluetooth.BLE` + `gatts_register_services()`로 GATT 서버 구성
- `FLAG_READ / FLAG_WRITE / FLAG_NOTIFY`로 특성 권한 설정
- `gatts_notify()`로 이벤트 기반 실시간 전송
- 커스텀 128비트 UUID로 고유한 서비스 정의
