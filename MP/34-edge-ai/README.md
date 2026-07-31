# 34: 엣지 AI — TensorFlow Lite Micro Concepts, Sensor Classification

## 개요

AI 추론을 클라우드가 아니라 **기기 안에서(온디바이스)** 수행하는 것을 **엣지 AI(Edge AI)** 라고 합니다. MCU용 추론 엔진이 **TensorFlow Lite Micro (TFLite Micro)** 이며, 이번 레슨에서는 그 핵심 개념인 **양자화**와 **센서 분류**를 배웁니다.

## 왜 MCU에서 추론해야 하는가

- **지연**: 데이터 왕복 없이 즉시 응답
- **프라이버시**: 센서 데이터가 밖으로 나가지 않음
- **전원**: 수십 mW로 동작 (클라우드 통신보다 저전력)
- **오프라인**: 인터넷이 없어도 동작

## 양자화 (Quantization)

모델의 float32 가중치는 메모리(RAM)와 연산량이 큽니다. **int8 양자화**는 가중치를 8비트 정수로 변환해 메모리를 1/4로 줄이고 연산을 가속합니다.

```python
scale = (max_v - min_v) / 255.0
zero_point = round(-min_v / scale)
q = round(w / scale) + zero_point      # float → int8
```

정수만으로 곱셈·덧셈을 하므로 MCU에서도 실시간 추론이 가능합니다.

## TFLite Micro 흐름

```
PC에서 학습 → .tflite 변환 (양자화)
    ↓ ampy/C로 업로드
MCU에 tflite-micro 인터프리터 포함
    ↓
센서 데이터 전처리 → interpreter.invoke() → 라벨 출력
```

MicroPython에서는 직접 C++ 인터프리터를 호출하기 어려우므로, main.py는 **분류 연산의 구조**를 Python으로 재현해 개념을 익힙니다.

## 센서 분류 모델

`[진동, 밝기]` 두 피처로 `[앉기/서기/걷기]`를 분류하는 선형 분류기를 예시로 듭니다. 실제 배포 시에는 이 부분을 tflite 모델 파일과 인터프리터로 대체합니다.

```python
scores = [sum(w*f for w, f in zip(weights_c, features)) + bias_c
          for c in range(n_classes)]
label = CLASSES[scores.index(max(scores))]
```

## 실행/업로드 방법

1. **Thonny IDE**: `MP/34-edge-ai/main.py`를 열어 실행(F5). 하드웨어 없이도 개념 데모가 동작합니다.
2. **ampy**:
   ```bash
   ampy --port COM3 put MP/34-edge-ai/main.py
   ampy --port COM3 run MP/34-edge-ai/main.py
   ```
3. 시리얼에서 양자화 결과와 5회 분류 출력을 확인합니다.

## 핵심 개념 요약

- 엣지 AI = 기기 내 추론 (저지연·프라이버시·저전력)
- TFLite Micro = MCU용 int8 추론 엔진
- 양자화로 모델 크기 1/4, 정수 연산으로 가속
- 센서 → 전처리 → 추론 → 라벨 출력 파이프라인
