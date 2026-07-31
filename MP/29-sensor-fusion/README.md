# 29: 센서 융합 — Moving Average, Kalman Filter Basics

## 개요

단일 센서 값은 노이즈와 드리프트를 포함합니다. **센서 융합(Sensor Fusion)** 은 여러 측정을 결합해 더 정확하고 신뢰할 수 있는 값을 얻는 기법입니다. 이번 레슨에서는 가장 널리 쓰이는 **이동 평균(Moving Average)** 과 **1차원 칼만 필터**를 직접 구현해 비교합니다.

## 왜 필터가 필요한가

- 가속도계: 진동 노이즈 큼
- 자이로: 노이즈는 작지만 시간이 지나면 **드리프트**(바이어스 누적)
- ADC: 양자화 노이즈

필터는 노이즈를 줄이되, 신호의 변화(실제 움직임)를 최대한 늦추지 않아야 합니다.

## 이동 평균 (Moving Average)

최근 N개의 값을 평균냅니다. 창 크기 N이 클수록 부드러워지지만 **지연(latency)** 도 커집니다.

```python
class MovingAverage:
    def __init__(self, window=5):
        self.window = window
        self.buffer = []

    def update(self, value):
        self.buffer.append(value)
        if len(self.buffer) > self.window:
            self.buffer.pop(0)
        return sum(self.buffer) / len(self.buffer)
```

## 1차원 칼만 필터

칼만 필터는 **예측(predict)** 과 **갱신(update)** 을 반복합니다. 노이즈 특성(공정/측정 공분산)을 알고 있으면 이동 평균보다 지연이 적고 정밀합니다.

```python
class Kalman1D:
    def __init__(self, process_noise, measurement_noise):
        self.q = process_noise      # 공정 노이즈 (Q)
        self.r = measurement_noise  # 측정 노이즈 (R)
        self.x = 0.0                # 추정 상태
        self.p = 1.0                # 오차 공분산

    def update(self, z):
        # 1) 예측
        self.p = self.p + self.q
        # 2) 칼만 이득 계산
        k = self.p / (self.p + self.r)
        # 3) 갱신
        self.x = self.x + k * (z - self.x)
        self.p = (1 - k) * self.p
        return self.x
```

- **Q (공정 노이즈)**: 시스템 자체의 불확실성 → 클수록 측정값을 신뢰
- **R (측정 노이즈)**: 센서 노이즈 → 클수록 예측값을 신뢰

## 가속도계에 적용

칼만 필터로 가속도계의 진동 노이즈를 줄이고, 실제 중력 벡터 방향을 추정합니다.

## 실행/업로드 방법

1. **Thonny IDE**: `MP/29-sensor-fusion/main.py`를 실행(F5). 실제 센서가 없으면 시뮬레이션 노이즈로 비교 그래프를 그립니다.
2. **ampy**:
   ```bash
   ampy --port COM3 put MP/29-sensor-fusion/main.py
   ampy --port COM3 run MP/29-sensor-fusion/main.py
   ```
3. MPU6050/ADXL345 등 I2C 가속도계가 있으면 `SimulatedSensor` 대신 실제 값을 넣어보세요.

## 핵심 개념 요약

- 이동 평균: 단순하지만 지연이 큼
- 칼만 필터: Q/R로 노이즈 특성을 모델링해 예측+갱신
- 정확도와 지연 사이의 트레이드오프가 설계의 핵심
