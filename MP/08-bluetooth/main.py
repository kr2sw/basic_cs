# Bluetooth

from microbit import *

def setup():
    display.scroll("BLE Advertising")
    sleep(2000)
    display.clear()

def scan_ble():
    display.scroll("BLE Scanning")
    sleep(2000)
    display.clear()
    
    # BLE 서비스 UUID 목록
    services = ["180D", "180A"]
    
    # BLE 스캐닝 (간소화된 예제)
    display.show("OK")
    sleep(1000)
    display.clear()
    
    # NFC와 유사한 대체 접근 방식 사용
    nfc = machine.NFC()
    if nfc.is_tag_present():
        display.show("CONNECTED")
        sleep(1000)
    else:
        display.show("NO")
        sleep(1000)

# BLE 스캐닝 예제 함수 실행
scan_ble()

while True:
    sleep(100)