# NFC

import machine

# NFC 모듈 설정
nfc = machine.NFC()

def setup():
    display.scroll("NFC Tag Scan")
    sleep(2000)
    display.clear()

def scan_nfc():
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
scan_nfc()

while True:
    sleep(100)