# 13일차: 태양 추적기

## 개념 소개

이 수업에서는 마이크로비트를 이용한 태양 추적 시스템에 대해 배우게 됩니다. 주요 개념은 다음과 같습니다:

1. **빛의 세기 감지**: LDR 또는 주변광 센서를 이용한 빛 감지
2. **서보 모터 제어**: 정밀한 각도 제어를 위한 서보 모터 사용
3. **자동 추적**: 최적의 각도를 찾기 위한 알고리즘
4. **반복 제어**: 지속적으로 최적의 방향을 찾기 위한 루프
5. **제어 루프**: 목표(태양)와 현재 상태 비교

## 예시 코드

```python
from microbit import *
import time

def setup_solar_tracker():
    # LDR 핀 (여기에 표시된 것)
    light_sensor = pin0
    # 서보 모터 제어 핀
    servo_pin = pin1
    return light_sensor, servo_pin
def angle_to_pulse_width(angle):
    # 서보 모터 각도 변환 (0-180도 -> 0-20000)
    return int((angle / 180) * 20000)
def set_servo_angle(servo_pin, angle):
    pulse_width = angle_to_pulse_width(angle)
    # 서보 모터 PWM 생성
    pin1.write_analog(pulse_width)
def find_optimal_angle(light_sensor):
    best_angle = 90
    best_light = 0

    # -45도에서 225도까지 5도 간격으로 스캔
    for angle in range(-45, 226, 5):
        set_servo_angle(servo_pin, angle)
        time.sleep(50)
        light_value = light_sensor.read_analog()
        
        if light_value > best_light:
            best_light = light_value
            best_angle = angle
    
    return best_angle
def main():
    global servo_pin
    light_sensor, servo_pin = setup_solar_tracker()

    display.scroll("SolTrk")
    time.sleep(2000)

    while True:
        optimal_angle = find_optimal_angle(light_sensor)
        set_servo_angle(servo_pin, optimal_angle)
        
        # 최적의 각도와 현재 각도 차이 표시
        display.show(str(optimal_angle))
        time.sleep(2000)

main()
```

## 키 개념

- **LDR 센서**: read_analog()를 이용한 빛의 세기 측정
- **서보 모터 PWM**: write_analog()를 이용한 특정 펄스 폭 생성
- **스캔 알고리즘**: 최적의 위치를 찾기 위한 반복 검색
- **제어 루프**: 지속적으로 환경 변화에 적응

## 실행 방법

1. LDR 센서를 핀 0과 연결
2. 서보 모터를 핀 1과 연결 (5V와 GND도 연결)
3. 3V와 GND를 서보 모터에 공급
4. 보드를 컴퓨터에 USB로 연결
5. main.py 파일을 보드에 복사
6. 보드가 최적의 각도를 찾을 때까지 대기

## 개선 제안

- 더 빠른 추적을 위한 적외선 센서 추가
- 태양 위치 추적기를 위한 태양 각도 센서 추가 (DS1307)
- 실시간으로 일출/일몰 시간 표시
- 조도 측정기를 위한 광센서 추가 (BME280)