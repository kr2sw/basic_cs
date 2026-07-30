# Games

from microbit import *

def setup():
    display.scroll("Game")
    sleep(2000)
    display.clear()

def pong_game():
    # 상대방 점수
    opponent_score = 0
    
    # 사용자 점수
    user_score = 0
    
    # 공의 위치
    ball_x = 2
    ball_y = 2
    
    while True:
        # 상대방 움직이기
        if opponent_score < user_score:
            ball_y = min(ball_y + 1, 4)
        
        # 사용자 입력을 통해 플랫폼 이동
        if button_a.is_pressed():
            user_score += 1
        elif button_b.is_pressed():
            user_score -= 1
        
        # 공과 플랫폼 충돌 확인
        if ball_x == 0 and ball_y == user_score:
            opponent_score += 1
        
        display.show(str(user_score) + ":" + str(opponent_score))
        sleep(500)

def snake_game():
    # 스네이크 크기
    snake = [(2, 2), (2, 1), (2, 0)]
    
    while True:
        # 머리 이동
        if button_a.is_pressed():
            head = (snake[0][0] + 1, snake[0][1])
        elif button_b.is_pressed():
            head = (snake[0][0] - 1, snake[0][1])
        
        snake.insert(0, head)
        snake.pop()
        
        # 충돌 확인
        if head in snake[1:]:
            display.scroll("GAME OVER")
            sleep(2000)
            break
        
        display.show("SNAKE")
        sleep(500)

pong_game()

while True:
    sleep(100)