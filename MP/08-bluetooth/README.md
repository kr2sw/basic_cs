# 블루투스

이 레슨에서는 BLE(블루투스 저전력)를 사용하여 마이크로비트를 다른 장치와 통신하는 방법을 학습합니다.

## BLE 광고

BLE는 모바일 장치와 쉽게 통신할 수 있게 해주는 저전력, 단거리 무선 통신 프로토콜입니다:
- 긴 대기 시간(수일-수주) 지원
- 저전력 소모
- 주파수 호핑(공격으로부터 안전)
- 2.4GHz ISM 대역 사용

## BLE 주변 장치

마이크로비트는 BLE 주변 장치로 작동할 수 있습니다:
- 서비스 데이터/특성 정의
- 광고 페이로드 생성
- 연결 요청 처리
- 여러 주변 장치 동시에 지원

## BLE 스캐닝

스마트폰과 같은 BLE 스캐닝 장치가 마이크로비트를 검색할 수 있습니다:
- BLE 스캐닝
- 서비스 UUID 확인
- 광고 데이터 읽기

## BLE 예제 프로그램

```python
from microbit import *

def setup():
    # BLE 서비스 데이터 설정
    services = [ "180D", "180A" ]  # 표준 서비스 UUID
    
    # BLE 광고 메시지 설정
    display.scroll("Advertising")
    
    # 주변 장치 모드 진입
    # ... BLE 코드 ...

def on_scan():
    # BLE 스캐닝
    display.scroll("Scanning BLE")
    sleep(2000)
    display.clear()

# 스캐닝 함수 실행
on_scan()
```