# 19일차: IoT의 기초

## 개념 소개

이 수업에서는 마이크로비트를 이용한 IoT(사물인터넷)의 기초에 대해 배우게 됩니다. 주요 개념은 다음과 같습니다:

1. **HTTP 요청**: 마이크로비트가 웹 서버와 통신하는 방법
2. **REST API**: 간단하고 강력한 웹 서비스 통신 프로토콜
3. **클라우드 연동**: 데이터를 원격 서버에 저장 및 공유
4. **데이터 전송**: 센서 데이터를 인터넷으로 전송
5. **웹 후크**: 간단한 웹 서비스 Trigger 및 IFTTT와 같은 서비스 이용

## 예시 코드

```python
from microbit import *
import urequests
import time

class IoTDevice:
    def __init__(self):
        self.api_key = "YOUR_API_KEY"
        self.device_id = "microbit_001"
        self.server_url = "https://api.thingspeak.com/update"
        self.post_url = "https://maker.ifttt.com/trigger/your_event/with/key/your_key"
    def read_sensor_data(self):
        temperature = 22.5
        humidity = 45.0
        light = 500
        return {
            "field1": temperature,
            "field2": humidity,
            "field3": light,
            "created_at": time.time()
        }
    def post_to_thingspeak(self, data):
        params = f"api_key={self.api_key}&field1={data['field1']}&field2={data['field2']}&field3={data['field3']}"
        headers = {"Content-Type": "application/x-www-form-urlencoded"}
        
        try:
            response = urequests.post(self.server_url + "?" + params, headers=headers)
            if response.status_code == 200:
                display.scroll("Upload OK")
            else:
                display.scroll("Upload Fail")
            response.close()
        except Exception as e:
            display.scroll("Error")
            print(f"Error: {e}")
    def post_to_ifttt(self, data):
        headers = {"Content-Type": "application/json"}
        body = {
            "value1": data['field1'],
            "value2": data['field2'],
            "value3": data['field3']
        }
        
        try:
            response = urequests.post(self.post_url, headers=headers, json=body)
            if response.status_code == 200:
                display.scroll("IFTTT OK")
            else:
                display.scroll("IFTTT Fail")
            response.close()
        except Exception as e:
            display.scroll("IFTTT Error")
            print(f"Error: {e}")
    def webhook_call(self):
        import urllib.parse
        params = urllib.parse.urlencode({
            "field1": 22.5,
            "field2": 45.0,
            "field3": 500,
            "key": "YOUR_WEBHOOK_KEY"
        })
        
        try:
            response = urequests.post("https://api.cosm.com/v2/devices/YOUR_DEVICE_ID/data?" + params)
            if response.status_code == 200:
                display.scroll("Webhook OK")
            else:
                display.scroll("Webhook Fail")
            response.close()
        except Exception as e:
            display.scroll("Webhook Error")
            print(f"Error: {e}")
    def main(self):
        display.scroll("IoT Device")
        time.sleep(2000)
        
        while True:
            if button_a.is_pressed():
                data = self.read_sensor_data()
                self.post_to_thingspeak(data)
                time.sleep(5000)
            
            elif button_b.is_pressed():
                data = self.read_sensor_data()
                self.post_to_ifttt(data)
                time.sleep(5000)
            
            elif pin0.is_touched():
                self.webhook_call()
                time.sleep(5000)
            
            else:
                display.show(Image.SUN)
                time.sleep(1000)

class WeatherStationIoT:
    def __init__(self):
        self.server = "https://api.thingspeak.com/update"
        self.api_key = "YOUR_API_KEY"
    
    def simulate_weather(self):
        import random
        return {
            "temperature": 20 + random.uniform(-5, 5),
            "humidity": 50 + random.uniform(-20, 20),
            "pressure": 1013 + random.uniform(-10, 10),
            "wind_speed": random.uniform(0, 20),
            "timestamp": int(time.time() * 1000)
        }
    
    def upload_data(self, data):
        payload = {
            "api_key": self.api_key,
            "field1": data["temperature"],
            "field2": data["humidity"],
            "field3": data["pressure"],
            "field4": data["wind_speed"],
            "created_at": data["timestamp"]
        }
        
        try:
            response = urequests.post(self.server, json=payload)
            if response.status_code == 200:
                display.scroll("Weather OK")
            else:
                display.scroll("Weather Fail")
            response.close()
        except Exception as e:
            display.scroll("Weather Error")
            print(f"Error: {e}")
    
    def main(self):
        display.scroll("Weather IoT")
        time.sleep(2000)
        
        while True:
            weather_data = self.simulate_weather()
            self.upload_data(weather_data)
            
            display.scroll(f"T:{weather_data['temperature']:.1f}")
            time.sleep(10000)

if __name__ == "__main__":
    print("IoT 선택:")
    print("A - 기본 IoT")
    print("B - Weather IoT")
    
    if button_a.is_pressed():
        device = IoTDevice()
        device.main()
    elif button_b.is_pressed():
        station = WeatherStationIoT()
        station.main()
```

## 키 개념

- **urequests 모듈**: 마이크로비트를 위한 HTTP 클라이언트
- **REST API**: GET, POST, PUT, DELETE 등의 표준 웹 프로토콜
- **JSON 형식**: 데이터 교환을 위한 표준 포맷
- **웹 후크**: 간단한 이벤트 기반 웹 서비스
- **오류 처리**: 네트워크 오류 및 예외 처리

## 실행 방법

1. `api.thingspeak.com`, `maker.ifttt.com` 등의 서비스를 위한 API 키 발급
2. 마이크로비트를 컴퓨터에 USB로 연결
3. main.py 파일을 보드에 복사
4. 보드를 인터넷에 연결 (Wi-Fi 모듈 또는 USB 테더링)
5. A 버튼으로 ThingSpeak에 데이터 전송, B 버튼으로 IFTTT에 전송, PIN0으로 웹훅 전송

## 사용 가능한 서비스

- **ThingSpeak**: 간단하고 강력한 IoT 플랫폼
- **IFTTT**: 여러 서비스 간의 연결
- **Cosm**: 데이터 중심의 IoT 플랫폼
- ** Firebase**: 실시간 데이터베이스
- **OpenWeatherMap**: 날씨 데이터 API

## 개선 제안

- DHT11 또는 BME280 센서 추가 (실제 측정)
- 마이크로비트 Wi-Fi 모듈 추가
- 더 자주 데이터 전송을 위한 스케줄러 추가
- 웹UI를 위한 간단한 HTTP 서버 생성
- 여러 디바이스를 위한 그룹 기능 추가