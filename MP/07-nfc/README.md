# NFC 태그 감지

이 레슨에서는 NFC(근거리 무선통신) 모듈을 사용하여 마이크로비트와 통신하는 방법을 학습합니다.

## NFC 태그 감지

NFC는 마이크로비트와 다른 NFC 장치를 연결할 수 있게 해주는 단거리 무선 통신 기술입니다:
- 13.56MHz 대역 사용
- 최대 10cm 거리
- 패시브/액티브 모드 지원
- 표준 NFC 포맷(ISO/IEC 14443 Type A/B) 지원

## RFID

RFID는 근거리 인식 기술로, 태그의 데이터를 읽을 수 있습니다:
- 태그 ID 읽기 (UID)
- 데이터 기록 (씀)
- 태그 유형(ISO/IEC 14443, Felica, etc.) 감지
- 액티브/패시브 태그 지원

## 간단한 인증

사용자가 NFC 태그를 통과시키면 인증합니다:
```python
from microbit import *

# NFC 태그 설정
nfc = machine.NFC()

# 태그 감지 함수
def on_tag():
    if nfc.is_tag_present():
        tag_data = nfc.read_tag()
        display.scroll("Tag ID: " + tag_data)
        sleep(1000)
        display.clear()

# 태그 감지 버튼
button_a.on_pressed(on_tag)
```

## NFC 예제 프로그램

```python
from microbit import *

def setup():
    display.scroll("NFC Tag Scan")
    sleep(2000)
    display.clear()

def scan_nfc():
    nfc = machine.NFC()
    display.scroll("Scanning...")
    
    # 1초 동안 스캔
    for i in range(10):
        if nfc.is_tag_present():
            tag_id = nfc.read_tag()[0:8]  # 태그 ID (첫 8자리)
            display.show("OK")
            sleep(1000)
            break
        sleep(100)
    else:
        display.show("NO")
        sleep(1000)
    
    display.clear()

# 함수 실행
nfc_tag.on_tag(scan_nfc)
```