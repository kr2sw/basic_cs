# 17일차: 교육용 게임

## 개념 소개

이 수업에서는 마이크로비트를 이용한 교육용 게임에 대해 배우게 됩니다. 주요 개념은 다음과 같습니다:

1. **수학 게임**: 덧셈, 뺄셈, 곱셈, 나눗셈 등을 이용한 문제 풀이
2. **퍼즐 게임**: 스피너나 사과를 이용한 논리력 향상
3. **대화형 학습**: 게임 플레이를 통한 학습 효과 증대
4. **즉각적인 피드백**: 정답과 오답에 대한 즉각적인 반응
5. **점수 시스템**: 학습 동기를 위한 점수 및 랭킹

## 예시 코드

```python
from microbit import *
import time
import random

class MathTutor:
    def __init__(self):
        self.score = 0
        self.lives = 3
        self.current_question = ""
        self.game_mode = "addition"
        self.question_count = 0
    def generate_addition(self):
        a = random.randint(1, 20)
        b = random.randint(1, 20)
        self.current_question = f"{a} + {b}"
        return a + b
    def generate_subtraction(self):
        a = random.randint(1, 20)
        b = random.randint(1, 20)
        if b > a:
            a, b = b, a
        self.current_question = f"{a} - {b}"
        return a - b
    def generate_multiplication(self):
        a = random.randint(1, 10)
        b = random.randint(1, 10)
        self.current_question = f"{a} × {b}"
        return a * b
    def generate_division(self):
        a = random.randint(1, 10) * random.randint(1, 10)
        b = random.randint(1, 10)
        self.current_question = f"{a} ÷ {b}"
        return a // b
    def ask_question(self):
        mode_handlers = {
            "addition": self.generate_addition,
            "subtraction": self.generate_subtraction,
            "multiplication": self.generate_multiplication,
            "division": self.generate_division
        }
        
        self.game_mode = random.choice(list(mode_handlers.keys()))
        answer = mode_handlers[self.game_mode]()
        display.scroll(f"Q{self.question_count+1}: {self.current_question} = ?")
        time.sleep(2000)
        
        return answer
    def check_answer(self, user_answer, correct_answer):
        if user_answer == correct_answer:
            self.score += 10
            display.show(Image.HEART)
            time.sleep(500)
            return True
        else:
            self.lives -= 1
            display.show(Image.SKULL)
            time.sleep(500)
            if self.lives <= 0:
                display.scroll("Game Over")
                return "game_over"
            return False
    def handle_input(self, answer):
        if len(answer) == 0:
            return
        
        try:
            user_answer = int(answer)
            result = self.check_answer(user_answer, self.current_question_answer)
            
            if result == True:
                display.scroll("Correct!")
                time.sleep(1000)
                self.ask_question()
            elif result == "game_over":
                display.scroll(f"Final Score: {self.score}")
                time.sleep(3000)
        except:
            display.scroll("Enter Number")
            time.sleep(1000)
            self.ask_question()
    def show_game_status(self):
        status = f"Lives:{self.lives} Score:{self.score}"
        display.scroll(status)
        time.sleep(1000)
    def main(self):
        display.scroll("Math Tutor")
        time.sleep(2000)
        
        self.ask_question()
        
        while True:
            if button_a.is_pressed():
                self.show_game_status()
                time.sleep(500)
            elif button_b.is_pressed():
                break
            
            time.sleep(100)

class SnakeGame:
    def __init__(self):
        self.snake = [(0, 5)]
        self.food = (8, 3)
        self.direction = (0, 1)
        self.score = 0
        self.game_over = False
    def draw_board(self):
        display.clear()
        for segment in self.snake:
            display.set_pixel(segment[0], segment[1], 9)
        display.set_pixel(self.food[0], self.food[1], 8)
    def move_snake(self):
        head_x, head_y = self.snake[-1]
        new_x = head_x + self.direction[0]
        new_y = head_y + self.direction[1]
        
        if new_x < 0 or new_x >= 5 or new_y < 0 or new_y >= 5 or (new_x, new_y) in self.snake:
            return False
        
        self.snake.append((new_x, new_y))
        
        if (new_x, new_y) == self.food:
            self.score += 10
            self.food = (random.randint(2, 3), random.randint(2, 3))
        else:
            self.snake.pop(0)
        
        return True
    def show_results(self):
        display.scroll(f"Score: {self.score}")
        time.sleep(2000)
    def main(self):
        display.scroll("Apple Pick")
        time.sleep(2000)
        
        while not self.game_over:
            self.draw_board()
            
            if button_a.is_pressed():
                if self.direction[1] == 0:
                    self.direction = (0, 1)
            elif button_b.is_pressed():
                if self.direction[1] == 0:
                    self.direction = (0, -1)
            elif pin0.is_touched():
                if self.direction[0] == 0:
                    self.direction = (1, 0)
            elif pin1.is_touched():
                if self.direction[0] == 0:
                    self.direction = (-1, 0)
            
            if not self.move_snake():
                self.game_over = True
                self.show_results()
            
            time.sleep(500)

if __name__ == "__main__":
    print("Select Game:")
    print("A - Math Tutor")
    print("B - Snake Game")
    
    if button_a.is_pressed():
        tutor = MathTutor()
        tutor.main()
    elif button_b.is_pressed():
        game = SnakeGame()
        game.main()