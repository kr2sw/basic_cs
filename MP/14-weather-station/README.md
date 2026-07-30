# 14일차: 날씨 스테이션

## 개념 소개

이 수업에서는 마이크로비트를 이용한 종합 날씨 스테이션에 대해 배우게 됩니다. 주요 개념은 다음과 같습니다:

1. **멀티 센서 통합**: 온도, 습도, 기압, 조도 등을 하나의 시스템에서 수집
2. **BME280 센서**: 온도, 습도, 기압 측정을 위한 디지털 센서
3. **데이터 집계**: 여러 센서에서 얻은 데이터를 하나의 화면으로 통합
4. **실시간 업데이트**: 지속적으로 변화하는 날씨 데이터 표시
5. **멀티폼 디스플레이**: LED_MATRIX를 이용한 다양한 정보 표시

## 예시 코드

```python
from microbit import *
import time
import pybytes

class WeatherStation:
    def __init__(self):
        self.data = {
            'temperature': 0,
            'humidity': 0,
            'pressure': 0,
            'light': 0
        }
    def simulate_sensor_data(self):
        import random
        import time
        self.data['temperature'] = 20 + random.uniform(-5, 5)
        self.data['humidity'] = 50 + random.uniform(-20, 20)
        self.data['pressure'] = 1013 + random.uniform(-10, 10)
        self.data['light'] = 500 + random.uniform(-100, 100)
    def display_weather(self):
        self.simulate_sensor_data()
        
        temp = f"{self.data['temperature']:.1f}°C"
        humi = f"{self.data['humidity']:.0f}%"
        pres = f"{self.data['pressure']:.0f}hPa"
        
        display.scroll(f"T:{temp} H:{humi}")
        sleep(1000)
        
        display.scroll(f"P:{pres}")
        sleep(1000)
        
        display.show(Image.SUN if self.data['light'] > 700 else Image.CLOUD)
    def upload_to_cloud(self):
        try:
            data_string = ",".join([
                str(self.data['temperature']),
                str(self.data['humidity']),
                str(self.data['pressure']),
                str(self.data['light']),
                str(int(time.time() * 1000))
            ])
            pybytes.send_data(data_string)
            display.scroll("Upload")
        except:
            display.scroll("NoNet")
    def main_loop(self):
        while True:
            self.display_weather()
            self.upload_to_cloud()
            sleep(5000)
def main():
    station = WeatherStation()
    
    display.scroll("WS Ready")
    sleep(2000)
    
    station.main_loop()

main()
```

## 키 개념

- **클래스 기반 설계**: WeatherStation 클래스를 이용한 상태 관리
- **시뮬레이션**: 실제 센서 없이 테스트를 위한 데이터 생성
- **pybytes 모듈**: 클라우드 통신을 위한 간단한 HTTP 클라이언트
- **이미지 표시**: Image.SUN, Image.CLOUD 등을 이용한 다양한 아이콘 표시
- **FIFO 통신**: pybytes.send_data()를 이용한 비동기 데이터 전송

## 실행 방법

1. 모든 센서(실제 또는 시뮬레이션)가 연결되었는지 확인
2. 보드를 컴퓨터에 USB로 연결
3. main.py 파일을 보드에 복사
4. 켜진 후 5초마다 날씨 정보 표시 및 클라우드에 업로드

## 개선 제안

- 실제 BME280 센서 추가 (I2C 연결)
- 더 아름다운 디스플레이를 위한 LED_MATRIX 추가
- 통계 계산 (평균, 최대, 최소)
- 원격 제어를 위한 BLE 추가
- 비를 감지하기 위한 빗방울 센서 추가