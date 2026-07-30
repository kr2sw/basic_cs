# 20일차: AI의 기초

## 개념 소개

이 수업에서는 마이크로비트를 이용한 인공지능(AI)의 기초에 대해 배우게 됩니다. 주요 개념은 다음과 같습니다:

1. **머신러닝 기초**: 간단한 예측 모델 구현
2. **지도 학습**: 라벨이 있는 데이터를 이용한 학습
3. **모델 학습**: 신경망 또는 로지스틱 회귀를 이용한 학습
4. **예측**: 새로운 데이터에 대한 예측
5. **피드백 루프**: 지속적인 학습 및 개선

## 예시 코드

```python
from microbit import *
import time
import random

class SimpleAI:
    def __init__(self):
        self.model = {
            'weights': [random.uniform(-1, 1) for _ in range(3)],
            'bias': random.uniform(-1, 1),
            'training_history': []
        }
        self.training_data = [
            {'input': [1, 0, 0], 'output': 0},
            {'input': [0, 1, 0], 'output': 1},
            {'input': [0, 0, 1], 'output': 1},
            {'input': [1, 1, 0], 'output': 1},
            {'input': [1, 0, 1], 'output': 1},
            {'input': [0, 1, 1], 'output': 1},
            {'input': [1, 1, 1], 'output': 1}
        ]
    
    def sigmoid(self, x):
        return 1 / (1 + pow(2.718281828459045, -x))
    
    def sigmoid_derivative(self, x):
        return x * (1 - x)
    
    def predict(self, inputs):
        # 선형 조합 계산
        weighted_sum = sum(w * i for w, i in zip(self.model['weights'], inputs)) + self.model['bias']
        output = self.sigmoid(weighted_sum)
        return output > 0.5
    
    def train(self, iterations=1000):
        for i in range(iterations):
            # 데이터셋에서 랜덤 샘플 선택
            sample = random.choice(self.training_data)
            inputs = sample['input']
            expected = sample['output']
            
            # 예측
            prediction = self.predict(inputs)
            
            # 오류 계산
            error = expected - prediction
            
            # 가중치 업데이트 (간단한 그라디언트 하락)
            for j in range(len(self.model['weights'])):
                self.model['weights'][j] += 0.1 * error * inputs[j]
            
            self.model['bias'] += 0.1 * error
            
            # 학습 기록
            self.model['training_history'].append(error)
            
            # 게임 중에는 약 1초마다 업데이트
            if i % 500 == 0:
                display.scroll(f"Iter: {i}")
                time.sleep(500)
    
    def test_model(self):
        correct = 0
        total = len(self.training_data)
        
        for sample in self.training_data:
            result = self.predict(sample['input'])
            if result == sample['output']:
                correct += 1
        
        accuracy = (correct / total) * 100
        return accuracy
    
    def display_accuracy(self):
        accuracy = self.test_model()
        display.scroll(f"Acc:{accuracy:.1f}%")
        time.sleep(1000)
class PatternRecognition:
    def __init__(self):
        self.learned_patterns = []
        self.pattern_weights = [0.0] * 25  # 5x5 매트릭스
    
    def capture_pattern(self, grid_data):
        # 5x5 매트릭스의 합 계산
        pattern_sum = sum(grid_data)
        # 패턴 계산 및 저장
        normalized_pattern = [x / pattern_sum if pattern_sum > 0 else 0 for x in grid_data]
        self.learned_patterns.append(normalized_pattern)
        display.scroll("Pattern Saved")
        time.sleep(1000)
    
    def recognize_pattern(self, test_grid):
        test_sum = sum(test_grid)
        normalized_test = [x / test_sum if test_sum > 0 else 0 for x in test_grid]
        
        best_match = None
        best_similarity = 0
        
        for pattern in self.learned_patterns:
            similarity = sum(abs(p - t) for p, t in zip(pattern, normalized_test))
            if similarity > best_similarity:
                best_similarity = similarity
                best_match = pattern
        
        if best_match:
            display.scroll(f"Match:{best_similarity:.2f}")
        else:
            display.scroll("New?")
        
        time.sleep(1000)
    
    def main(self):
        display.scroll("Pattern AI")
        time.sleep(2000)
        
        while True:
            # 캡처 모드 (A 버튼)
            if button_a.is_pressed():
                # 5x5 매트릭스에서 패턴 캡처
                grid_data = []
                for y in range(5):
                    row = []
                    for x in range(5):
                        row.append(display.get_pixel(x, y))
                    grid_data.extend(row)
                self.capture_pattern(grid_data)
                time.sleep(1000)
            
            # 인식 모드 (B 버튼)
            elif button_b.is_pressed():
                # 테스트 매트릭스에서 패턴 인식
                test_grid = []
                for y in range(5):
                    row = []
                    for x in range(5):
                        row.append(display.get_pixel(x, y))
                    test_grid.extend(row)
                self.recognize_pattern(test_grid)
                time.sleep(1000)
            
            else:
                display.show(Image.ANGRY)
                time.sleep(1000)
class ChatBot:
    def __init__(self):
        self.knowledge_base = {
            "hello": "Hi there! How can I help you?",
            "how are you": "I'm a micro:bit AI, I'm functioning normally!",
            "time": "I can't tell time directly, but I can display the running time.",
            "thank you": "You're welcome! Have a great day!",
            "help": "I can help with basic AI demos, pattern recognition, and simple games. Press A for AI training, B for Chat."
        }
        self.current_topic = None
    
    def get_response(self, user_input):
        user_input = user_input.lower().strip()
        
        # 키워드 매칭
        for keyword in self.knowledge_base:
            if keyword in user_input:
                return self.knowledge_base[keyword]
        
        # 모호한 입력 처리
        if len(user_input) < 3:
            return "Please say something more meaningful."
        
        # 기본 응답
        responses = [
            "That's interesting! Tell me more.",
            "I've learned something new today!",
            "My circuits are processing that information...",
            "That's a fascinating question!",
            "I'm still learning about that topic."
        ]
        
        return random.choice(responses)
    
    def chat_interaction(self):
        display.scroll("ChatBot Ready")
        time.sleep(2000)
        
        while True:
            # 채팅 시작 (PIN0 터치)
            if pin0.is_touched():
                display.scroll("Say Something")
                time.sleep(1000)
                
                # 간단한 음성 입력 시뮬레이션 (여기서는 A/B 버튼 사용)
                messages = ["Hello", "How are you", "What's the time", "Thank you", "Help"]
                for msg in messages:
                    response = self.get_response(msg)
                    display.scroll(response)
                    time.sleep(2000)
                
                display.scroll("Chat End")
                time.sleep(1000)
            
            else:
                display.show(Image.HEART)
                time.sleep(1000)
    
    def main(self):
        display.scroll("ChatBot AI")
        time.sleep(2000)
        
        self.chat_interaction()
def main():
    print("AI 유형 선택:")
    print("A - 간단 AI")
    print("B - 패턴 인식")
    print("C - 챗봇")
    
    if button_a.is_pressed():
        ai = SimpleAI()
        ai.train()
        ai.display_accuracy()
    elif button_b.is_pressed():
        pattern = PatternRecognition()
        pattern.main()
    elif button_c.is_pressed():
        chatbot = ChatBot()
        chatbot.main()
```

## 키 개념

- **단순 신경망**: 시그모이드 함수를 이용한 기본 학습 모델
- **패턴 인식**: 픽셀 매트릭스를 이용한 이미지 유사성 비교
- **추론 엔진**: 규칙 기반 지식 기반을 이용한 응답 생성
- **지도 학습**: 레이블이 있는 데이터를 이용한 모델 학습
- **결정 트리**: 규칙에 따른 의사 결정 (패턴 인식과 유사)

## 실행 방법

1. 간단한 AI에 대한 이해 (여기서는 XOR 문제 해결)
2. 마이크로비트를 컴퓨터에 USB로 연결
3. main.py 파일을 보드에 복사
4. A 버튼으로 간단 AI 모델 학습, B 버튼으로 패턴 인식 실행, C 버튼으로 챗봇 시작

## 학습 내용

- AI는 어떻게 학습하는가?
- 데이터는 어떻게 모델에 주입되는가?
- 신경망이 필요한 이유는 무엇인가?
- 마이크로비트로 AI를 구현하는 방법은 무엇인가?
- 패턴 인식은 어떻게 작동하는가?
- 간단한 챗봇을 만드는 방법은 무엇인가?

## 개선 제안

- 실제 센서 데이터 추가 (온도, 습도, 소리)
- 더 큰 데이터셋으로 모델 개선
- 피크 및 홀까지 계산을 포함한 더 복잡한 패턴 인식
- TensorFlow Lite로의 마이그레이션 (더 고급 하드웨어 필요)
- BLE를 이용한 여러 마이크로비트 간의 AI 협업
- 실제 문제에 대한 모델 미세 조정