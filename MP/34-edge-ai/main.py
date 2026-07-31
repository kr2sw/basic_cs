# 34: 엣지 AI — TensorFlow Lite Micro Concepts, Sensor Classification
# 대상: ESP32/Pico (사전 학습된 int8 양자화 모델을 사용한다고 가정)
import time

# --- 엣지 AI 개념 ------------------------------------------------------------
# 클라우드 AI: 데이터를 서버로 보내 추론 → 지연/프라이버시/전원 문제
# 엣지 AI: MCU(마이크로컨트롤러)에서 직접 추론
#   - TinyML: KB 단위 모델, 수십 mW 추론
#   - TFLite Micro: C++ 추론 엔진, int8 양자화 연산에 특화
#   - 양자화: float32 가중치 → int8 로 변환 (메모리 1/4, 속도 향상)

# --- 양자화 개념 ---------------------------------------------------------------
# float32 범위 [min, max] → int8 [-128, 127]
# scale = (max - min) / 255,  zero_point = round(-min / scale)
def quantize(weights, min_v, max_v):
    """float 배열을 int8 양자화 배열로 변환"""
    scale = (max_v - min_v) / 255.0
    zero_point = round(-min_v / scale) if scale > 0 else 0
    q = [max(-128, min(127, round(w / scale) + zero_point)) for w in weights]
    return q, scale, zero_point


def dequantize(q, scale, zero_point):
    """int8 배열을 float로 복원"""
    return [(v - zero_point) * scale for v in q]


# --- 실습: 소형 분류 모델 (수제 코드) ------------------------------------------
# [진동, 밝기] 두 피처로 [앉기/서기/걷기] 를 분류하는 간단한 모델
# 실제로는 TFLite Micro가 이 연산을 담당. 여기서는 구조만 재현
CLASSES = ["sit", "stand", "walk"]
# 사전 학습된 가중치 (개념 예시, 임의값 아님 — 수치만 대표값)
WEIGHTS = [
    [1.2, -0.3, 0.8],   # 클래스별 가중치
    [0.5, 0.9, -1.1],
]
BIAS = [0.1, -0.2, 0.0]


def classify(features):
    """선형 분류기: argmax(weights·features + bias)"""
    scores = []
    for cls in range(len(CLASSES)):
        s = BIAS[cls]
        for f, w in zip(features, WEIGHTS[cls]):
            s += f * w
        scores.append(s)
    best = scores.index(max(scores))
    return CLASSES[best], scores


def quantized_classify(q_features, scale, zp):
    """양자화된 특징으로 분류 (int8 연산 시뮬레이션)"""
    features = dequantize(q_features, scale, zp)
    return classify(features)


# --- 센서 데이터 수집 (시뮬레이션) -------------------------------------------
def sample_features():
    """실제 IMU/광센서를 대신하는 시뮬레이션 샘플"""
    import random
    return [random.uniform(0, 1), random.uniform(0, 1)]


def main():
    print("=== 엣지 AI / TFLite Micro 개념 데모 ===")
    print("1) float 가중치 양자화")
    fw = WEIGHTS
    qw, scale, zp = quantize(fw[0], -2.0, 2.0)
    print(f"  float: {fw[0]}")
    print(f"  int8 : {qw} (scale={scale:.3f}, zp={zp})")

    print("\n2) 센서 실시간 분류")
    for i in range(5):
        features = sample_features()
        label, scores = classify(features)
        print(f"  샘플{i}: features={[round(f,2) for f in features]} "
              f"→ {label} (scores={[round(s,2) for s in scores]})")
        time.sleep(1)

    print("\n3) 실제 TFLite Micro 적용 지점")
    print("  - PC에서 모델을 .tflite(int8)로 변환")
    print("  - tflite-micro C++ 라이브러리를 펌웨어에 포함")
    print("  - interpreter.invoke() 한 번에 추론 완료")
    print("  - 본 코드의 classify()가 그 추론 과정을 단순 재현한 것")


if __name__ == "__main__":
    main()
