# 센서

이 레슨에서는 다양한 센서를 사용하여 데이터를 수집하는 방법을 학습합니다.

## 온도 센서 (DS18B20)

DS18B20는 1-극성 디지털 온도 센서입니다:
- 핀 0-2에 연결 (1와이어 프로토콜 사용)
- -55°C에서 +125°C까지 측정 가능
- 내장 열보상 기능
- T-타입 J-타입 및 K-타입 저항과 호환

## light sensor (LDR)

LDR (광저항 센서)는 빛의 양에 따라 저항이 변합니다:
- 빛을 받으면 저항 감소
- 어둠 속에서 저항 증가
- ADC 핀과 함께 사용

## 센서 클래스 생성

재사용 가능한 센서 클래스를 생성하여 데이터 처리를 간소화할 수 있습니다.
```python
class Sensor:
    def __init__(self, pin):
        self.pin = pin
    
    def read(self):
        return self.pin.read_analog()
    
    def calibrate(self):
        return self.pin.read_analog()

# 온도 센서 클래스
class TemperatureSensor:
    def __init__(self, pin):
        self.pin = pin
        self.sensors = ds18b20.DS18B20(pin)
    
    def read_temp(self):
        return self.sensors.read_temp()
    
    def read_raw(self):
        return self.sensors.read_raw()

# 빛 센서 클래스
class LightSensor:
    def __init__(self, pin):
        self.pin = pin
    
    def read_luminosity(self):
        return self.pin.read_analog()
    
    def light_level(self):
        level = self.pin.read_analog()
        if level < 100: return "어두운"
        elif level < 500: return "보통"
        else: return "밝은"
```

## 센서 예제

```python
from microbit import *

def setup():
    temp_sensor = TemperatureSensor(machine.Pin(3))
    light_sensor = LightSensor(machine.Pin(4))
    
    temp = temp_sensor.read_temp()
    lumi = light_sensor.read_luminosity()
    
    display.show(str(temp) + "C, " + lumi)
    sleep(2000)
```