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
        weighted_sum = sum(w * i for w, i in zip(self.model['weights'], inputs)) + self.model['bias']
        output = self.sigmoid(weighted_sum)
        return output > 0.5
    
    def train(self, iterations=1000):
        for i in range(iterations):
            sample = random.choice(self.training_data)
            inputs = sample['input']
            expected = sample['output']
            
            prediction = self.predict(inputs)
            
            error = expected - prediction
            
            for j in range(len(self.model['weights'])):
                self.model['weights'][j] += 0.1 * error * inputs[j]
            
            self.model['bias'] += 0.1 * error
            
            self.model['training_history'].append(error)
            
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
        self.pattern_weights = [0.0] * 25
    
    def capture_pattern(self, grid_data):
        pattern_sum = sum(grid_data)
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
            if button_a.is_pressed():
                grid_data = []
                for y in range(5):
                    row = []
                    for x in range(5):
                        row.append(display.get_pixel(x, y))
                    grid_data.extend(row)
                self.capture_pattern(grid_data)
                time.sleep(1000)
            
            elif button_b.is_pressed():
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
        
        for keyword in self.knowledge_base:
            if keyword in user_input:
                return self.knowledge_base[keyword]
        
        if len(user_input) < 3:
            return "Please say something more meaningful."
        
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
            if pin0.is_touched():
                display.scroll("Say Something")
                time.sleep(1000)
                
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