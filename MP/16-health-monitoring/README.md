# 16일차: 건강 모니터링

## 개념 소개

이 수업에서는 마이크로비트를 이용한 건강 모니터링 시스템에 대해 배우게 됩니다. 주요 개념은 다음과 같습니다:

1. **심박수 센서**: IR 또는 가속도 센서를 이용한 맥박 감지
2. **거리 센서**: 초음파 센서를 이용한 거리 측정
3. **건강 측정값**: 칼로리, 걸음 수, 스트레스 레벨 등
4. **데이터 집계**: 여러 센서에서 얻은 데이터를 하나의 시스템으로 통합
5. **경고 시스템**: 위험 감지 시 알림 발송

## 예시 코드

```python
from microbit import *
import time

class HealthMonitor:
    def __init__(self):
        self.heartbeat = 0
        self.distance = 0
        self.calories = 0
        self.steps = 0
        self.stress_level = 0
        self.movement_threshold = 10  # 걸음 수 감지 임계값
    def simulated_heartbeat(self):
        # IR 센서를 이용한 맥박 감지 (시뮬레이션)
        import random
        bpm = 60 + random.uniform(-20, 20)  # 40-80 BPM 범위
        self.heartbeat = int(bpm)
        if bpm > 100:
            display.scroll("HIGH HR")
        elif bpm < 50:
            display.scroll("LOW HR")
        else:
            display.show(Image.HEART)
        return bpm
    def measured_distance(self):
        # 초음파 센서를 이용한 거리 측정 (시뮬레이션)
        import random
        dist = random.uniform(10, 200)  # 10-200cm 범위
        self.distance = int(dist)
        
        if dist < 30:
            display.scroll("Near")
        elif dist < 100:
            display.scroll("Medium")
        else:
            display.scroll("Far")
        
        return dist
    def calculate_metrics(self):
        # 활동량, 칼로리, 걸음 수 계산
        self.calories = int(self.steps * 0.05)  # 1걸음당 0.05칼로리
        self.stress_level = (self.distance / 100) * (self.heartbeat / 80)
        self.stress_level = min(self.stress_level, 10)
    def display_status(self):
        # 모든 건강 지표 표시
        status = f"HR:{self.heartbeat} D:{self.distance} C:{self.calories} S:{self.stress_level:.1f}"
        display.scroll(status)
        sleep(1000)
    
    # 이미지 매핑
    def show_status_icon(self):
        if self.heartbeat > 100:
            display.show(Image.SKULL)
        elif self.distance < 30:
            display.show(Image.SMILE)
        elif self.stress_level > 7:
            display.show(Image.ANGRY)
        elif self.heartbeat < 50:
            display.show(Image.CONFUSED)
        else:
            display.show(Image.HAPPY)
    def main_loop(self):
        while True:
            # 센서 데이터 수집
            self.simulated_heartbeat()
            self.measured_distance()
            
            # 걸음 수 감지 (가속도계 예시)
            self.steps += 1
            
            # 건강 지표 계산
            self.calculate_metrics()
            
            # 상태 표시
            self.display_status()
            
            # 경고 아이콘 표시
            self.show_status_icon()
            
            # 2초 대기 후 반복
            sleep(2000)
def main():
    monitor = HealthMonitor()
    
    display.scroll("Health Monitor")
    sleep(2000)
    
    monitor.main_loop()

main()
```

## 키 개념

- **시뮬레이션 센서**: 실제 센서 없이도 마이크로비트에서 건강 모니터링 구현
- **모바일 UI 매핑**: 상태에 따른 적절한 LED 아이콘 표시
- **생체 신호 처리**: 심박수 및 거리 데이터를 통한 건강 상태 분석
- **누적 계산**: 걸음 수와 시간에 따른 칼로리 계산
- **경고 시스템**: 위험 상태 감지 시 즉시 알림

## 실행 방법

1. 마이크로비트를 USB로 컴퓨터에 연결
2. main.py 파일을 보드에 복사
3. 보드가 켜진 후 자동으로 데이터 수집 시작
4. 보드가 오른쪽 화살표 버튼으로 건강 상태 표시

## 개선 제안

- 실제 MAX30102 IR 센서 추가 (I2C 연결)
- 더 정확한 걸음 수 감지를 위한 가속도계 추가 (ACC_X, ACC_Y, ACC_Z 사용)
- 평균 심박수 계산기 추가
- 클라우드에 데이터 전송하여 장기적인 추세를 확인
- 수면 패턴 분석기를 위한 PASCO 센서 추가