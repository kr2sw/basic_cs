# 18일차: 웨어러블 전자제품

## 개념 소개

이 수업에서는 마이크로비트를 이용한 웨어러블 전자제품에 대해 배우게 됩니다. 주요 개념은 다음과 같습니다:

1. **LED 매트릭스**: 팔찌, 의류, аксессуар에 착용 가능한 LED 디스플레이
2. **대화형 의류**: 압력 센서, 버튼, 기타 입력을 이용한 인터랙티브 의류
3. **모바일 페어링**: 스마트폰과 마이크로비트의 BLE 연결 (간단한 버전)
4. **에너지 절약**: 피크 시간대 디밍을 이용한 배터리 수명 연장
5. **모션 감지**: 움직임에 반응하는 LED 패턴

## 예시 코드

```python
from microbit import *
import time

class LEDBracelet:
    def __init__(self):
        self.heartbeat_enabled = False
        self.mood = "normal"  # normal, happy, sad, excited
        self.sequence = [0, 1, 2, 3, 4, 5]
        self.sequence_index = 0
    def heartbeat_pattern(self):
        for i in range(3):
            display.show(Image.HEART)
            time.sleep(200)
            display.show(Image.SILHOUETTE)
            time.sleep(200)
        display.show(Image.HEART)
        self.heartbeat_enabled = False
    def rainbow_cycle(self, wait=0.1):
        colors = [
            (255, 0, 0), (255, 127, 0), (255, 255, 0), (0, 255, 0),
            (0, 255, 255), (0, 127, 255), (0, 0, 255), (255, 0, 255)
        ]
        while True:
            for color in colors:
                r, g, b = color
                if r > 255: r = 255
                if g > 255: g = 255
                if b > 255: b = 255
                display.show(Image(r, g, b))
                time.sleep(wait)
    def mood_based_display(self):
        if self.mood == "happy":
            display.show(Image.HAPPY)
        elif self.mood == "sad":
            display.show(Image.SAD)
        elif self.mood == "excited":
            for i in range(3):
                display.show(Image.HEART)
                time.sleep(100)
                display.show(Image.SILHOUETTE)
                time.sleep(100)
        else:
            display.show(Image.SMILE)
    def pressure_response(self):
        # 압력 센서를 이용한 리듬 패턴
        for i in range(3):
            display.scroll(str(i+1))
            time.sleep(500)
        self.mood = "excited"
    def main_loop(self):
        display.scroll("LED Bracelet")
        time.sleep(2000)
        
        while True:
            # 시연 시퀀스 실행
            if button_a.is_pressed():
                self.heartbeat_pattern()
                time.sleep(1000)
            
            elif button_b.is_pressed():
                self.heartbeat_enabled = True
                time.sleep(100)
            
            # 연속적인 LED 패턴
            display.show(Image(self.sequence[self.sequence_index]))
            self.sequence_index = (self.sequence_index + 1) % len(self.sequence)
            time.sleep(500)
            
            # 기분 기반 표시
            if self.mood != "normal":
                self.mood_based_display()
                time.sleep(1000)
            
            # 압력 감시 (3번 터치 시 패턴 변화)
            for i in range(3):
                if pin0.is_touched():
                    self.pressure_response()
                    time.sleep(2000)

class InteractiveClothing:
    def __init__(self):
        self.active_zones = {"left": False, "right": False, "top": False}
        self.symptom_tracker = {"headache": False, "fatigue": False, "relaxation": False}
    def zone_detector(self):
        # 세 개의 압력 지점 확인
        if pin0.is_touched():
            self.active_zones["left"] = not self.active_zones["left"]
            display.scroll("Left ON" if self.active_zones["left"] else "Left OFF")
        
        elif pin1.is_touched():
            self.active_zones["right"] = not self.active_zones["right"]
            display.scroll("Right ON" if self.active_zones["right"] else "Right OFF")
        
        elif pin2.is_touched():
            self.active_zones["top"] = not self.active_zones["top"]
            display.scroll("Top ON" if self.active_zones["top"] else "Top OFF")
    
    def symptom_checker(self):
        # 건강 증상 추적
        if button_a.is_pressed():
            self.symptom_tracker["headache"] = not self.symptom_tracker["headache"]
            display.scroll("Headache: YES" if self.symptom_tracker["headache"] else "Headache: NO")
        
        elif button_b.is_pressed():
            self.symptom_tracker["fatigue"] = not self.symptom_tracker["fatigue"]
            display.scroll("Fatigue: YES" if self.symptom_tracker["fatigue"] else "Fatigue: NO")
        
        elif pin0.is_touched():
            self.symptom_tracker["relaxation"] = not self.symptom_tracker["relaxation"]
            display.scroll("Relaxation: YES" if self.symptom_tracker["relaxation"] else "Relaxation: NO")
    
    def wellness_feedback(self):
        # 웰니스 지표 수집 및 피드백
        total_symptoms = sum(self.symptom_tracker.values())
        
        if total_symptoms >= 2:
            display.scroll("Need Rest")
            for i in range(3):
                display.show(Image.ANGRY)
                time.sleep(300)
                display.show(Image.SAD)
                time.sleep(300)
        elif total_symptoms == 1:
            display.scroll("More Rest")
            display.show(Image.CONFUSED)
        elif total_symptoms == 0:
            display.scroll("Feeling Good")
            for i in range(2):
                display.show(Image.HAPPY)
                time.sleep(300)
                display.show(Image.HEART)
                time.sleep(300)
    
    def main(self):
        display.scroll("Interactive Clothing")
        time.sleep(2000)
        
        while True:
            self.zone_detector()
            time.sleep(100)
            
            self.symptom_checker()
            time.sleep(100)
            
            self.wellness_feedback()
            time.sleep(2000)

if __name__ == "__main__":
    print(" 웨어러블 선택:")
    print("A - LED 팔찌")
    print("B - 인터랙티브 의류")
    
    if button_a.is_pressed():
        bracelet = LEDBracelet()
        bracelet.main_loop()
    elif button_b.is_pressed():
        clothing = InteractiveClothing()
        clothing.main()