# 15일차: 스마트 홈 자동화

## 개념 소개

이 수업에서는 마이크로비트를 이용한 스마트 홈 자동화에 대해 배우게 됩니다. 주요 개념은 다음과 같습니다:

1. **스위치 제어**: 디지털 입력(버튼 또는 스위치)을 이용한 제어
2. **신홈 자동화**: 미리 설정된 시퀀스(씬)를 이용한 실행
3. **일정 관리**: 시간 기반 자동화 (간단한 버전)
4. **상태 저장**: 간단한 플래그를 이용한 상태 기억
5. **실용적 응용**: 조명, 팬, 경고 시스템 등

## 예시 코드

```python
from microbit import *
import time

class SmartHome:
    def __init__(self):
        self.states = {
            'light1': False,
            'light2': False,
            'fan': False,
            'alarm': False
        }
    def toggle_light(self, light_num):
        self.states[f'light{light_num}'] = not self.states[f'light{light_num}']
        state = "ON" if self.states[f'light{light_num}'] else "OFF"
        display.scroll(f"Light{light_num} {state}")
        return self.states[f'light{light_num}']
    def toggle_fan(self):
        self.states['fan'] = not self.states['fan']
        state = "ON" if self.states['fan'] else "OFF"
        display.scroll(f"Fan {state}")
        if self.states['fan']:
            display.show(Image.HEART)
        else:
            display.show(Image.SQUARE)
        return self.states['fan']
    def trigger_alarm(self):
        if not self.states['alarm']:
            self.states['alarm'] = True
            display.scroll("ALARM ON")
            for i in range(3):
                display.show(Image.SKULL)
                time.sleep(500)
                display.show(Image.HEART)
                time.sleep(500)
            self.states['alarm'] = False
            display.scroll("ALARM OFF")
    def morning_routine(self):
        display.scroll("Morning Routine")
        time.sleep(1000)
        
        # 아침 7시 (간단한 시뮬레이션)
        self.toggle_light(1)
        time.sleep(500)
        
        if True:  # 밤이 아닐 때
            self.toggle_fan(True)
        time.sleep(500)
        
        display.scroll("All Done")
        time.sleep(1000)
    def emergency_mode(self):
        display.scroll("EMERGENCY")
        for i in range(5):
            display.show(Image.SKULL)
            time.sleep(200)
            display.show(Image.HEART)
            time.sleep(200)
        self.states['alarm'] = True
        self.toggle_light(2)
    def main(self):
        display.scroll("Smart Home")
        time.sleep(2000)
        
        while True:
            if button_a.is_pressed():
                self.toggle_light(1)
                time.sleep(500)
            
            elif button_b.is_pressed():
                self.toggle_fan()
                time.sleep(500)
            
            elif button_a.is_pressed() and button_b.is_pressed():
                self.trigger_alarm()
                time.sleep(1000)
            
            # 간단한 일정 관리 (5초마다)
            time.sleep(5000)
            self.morning_routine()
            
            # 비상 상황 감지 (예: 과열)
            if True:  # 온도 센서가 50°C를 초과하는 조건
                self.emergency_mode()
                time.sleep(10000)

if __name__ == "__main__":
    home = SmartHome()
    home.main()