# 29: 센서 융합 — 이동 평균, 칼만 필터 기초
# 대상: ESP32/Pico (실제 센서 없이도 시뮬레이션으로 동작)
import random
import math
import time

TRUE_VALUE = 1.0          # 실제 중력 축 값 (시뮬레이션용)
MEASURE_NOISE_STD = 0.35  # 측정 노이즈 표준편차


# --- 1) 이동 평균 필터 ----------------------------------------------------
class MovingAverage:
    def __init__(self, window=8):
        self.window = window
        self.buffer = []

    def update(self, value):
        """새 값 추가 후 창 평균 반환"""
        self.buffer.append(value)
        if len(self.buffer) > self.window:
            self.buffer.pop(0)
        return sum(self.buffer) / len(self.buffer)


# --- 2) 1차원 칼만 필터 -----------------------------------------------------
class Kalman1D:
    def __init__(self, process_noise=0.01, measurement_noise=0.12):
        self.q = process_noise      # Q: 공정 노이즈
        self.r = measurement_noise  # R: 측정 노이즈
        self.x = TRUE_VALUE         # 초기 상태 추정
        self.p = 1.0                # 초기 오차 공분산

    def update(self, z):
        # 예측 단계 (등속 가정: 상태 변화 없음)
        self.p += self.q
        # 갱신 단계
        k = self.p / (self.p + self.r)          # 칼만 이득
        self.x += k * (z - self.x)              # 상태 갱신
        self.p *= (1 - k)                       # 오차 공분산 갱신
        return self.x


# --- 3) 시뮬레이션 센서 ------------------------------------------------------
class SimulatedSensor:
    """실제 가속도계를 대체: 참값 + 가우시안 노이즈"""

    def read(self):
        return TRUE_VALUE + random.gauss(0, MEASURE_NOISE_STD)


def gaussian_noise(mean=0, std=1):
    """가우시안 노이즈 (Box-Muller 변환)"""
    u1 = max(random.random(), 1e-9)
    u2 = random.random()
    return mean + std * math.sqrt(-2 * math.log(u1)) * math.cos(2 * math.pi * u2)


def simulate_sensor():
    """참값 + 노이즈 (가우시안 함수 사용)"""
    return TRUE_VALUE + gaussian_noise(0, MEASURE_NOISE_STD)


def compute_error(series):
    """참값과의 평균 제곱 오차 (RMSE) 계산"""
    return math.sqrt(sum((v - TRUE_VALUE) ** 2 for v in series) / len(series))


def main():
    sensor = SimulatedSensor()
    mov_avg = MovingAverage(window=8)
    kalman = Kalman1D(process_noise=0.01, measurement_noise=0.12)

    raw, ma, kf = [], [], []

    print("=== 센서 융합 비교 (참값 1.0g, 노이즈 ±0.35) ===")
    print("샘플 |  원시값  | 이동평균 |  칼만   ")
    for i in range(60):
        z = sensor.read()
        raw.append(z)
        ma.append(mov_avg.update(z))
        kf.append(kalman.update(z))
        if i % 10 == 0:
            print(f"{i:4d} | {z:6.3f} | {ma[-1]:6.3f} | {kf[-1]:6.3f}")

    print()
    print(f"RMSE 원시값  : {compute_error(raw):.4f}")
    print(f"RMSE 이동평균: {compute_error(ma):.4f}")
    print(f"RMSE 칼만    : {compute_error(kf):.4f}")
    print("\n결과: 필터 적용으로 노이즈가 크게 줄었습니다.")
    print("Q/R을 바꿔보면 응답 속도와 부드러움의 트레이드오프를 볼 수 있습니다.")

    # --- 실제 I2C 센서(MPU6050) 사용 시 교체 지점 ---------------------------
    # from machine import Pin, I2C
    # i2c = I2C(0, scl=Pin(22), sda=Pin(21))
    # # MPU6050 가속도 Z축 읽기 → kalman.update(z_accel)
    # 여기서는 시뮬레이션으로 충분


if __name__ == "__main__":
    main()
