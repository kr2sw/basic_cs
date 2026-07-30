# 게임

이 레슨에서는 프로그래밍 시나리오를 생성하고, 게임 컨셉, 플레이 조건, 입출력 처리 방식을 작성하는 방법을 학습합니다.

## snake game

고전적인 스네이크 게임을 프로그래밍합니다:
- 플레이어가 화살표 키로 뱀을 제어
- 뱀이 이동할 때마다 먹이를 먹음
- 뱀이 충돌하면 게임 오버

게임 설계 사항:
- 스네이크 크기 3블록으로 시작
- 특정 간격마다 먹이 생성
- 게임 오버: 스네이크가 화면 밖으로 나가거나 자기 자신을 만나는 경우

## pong

아크릴릭 고전 게임인 핑퐁을 모바일 장치와 프로그래밍합니다:
- 두 개의 플랫폼이 왼쪽과 오른쪽에 위치
- 플레이어가 플랫폼 상하로 이동
- 공이 플랫폼에 부딪히고 튕겨 나감
- 공이 밖으로 나가면 점수 획득

## 입력 처리

이러한 게임에서 중요한 부분은 사용자 입력을 처리하는 것입니다:
- 버튼_a, 버튼_b, 버튼_ab를 화살표 키로 사용
- button_a를 위에서 아래로 이동하는 데 사용
- button_b를 아래에서 위로 이동하는 데 사용
- button_ab를 화살표 키와 같이 사용하는 데 사용

```python
if button_a.is_pressed():
    # 위에서 아래로 이동
elif button_b.is_pressed():
    # 아래에서 위로 이동
elif button_ab.is_pressed():
    # 다른 동작 실행
```

## 게임 예제 프로그램

```python
from microbit import *

def setup():
    display.scroll("Game")
    sleep(2000)
    display.clear()

# 핑퐁 게임(간소화된 예제)
def pong_game():
    # 상대방의 점수
    opponent_score = 0
    
    # 사용자의 점수
    user_score = 0
    
    # 공의 위치
    ball_x = 2
    ball_y = 2
    
    while True:
        # 상대방이 움직이기
        if opponent_score < user_score:
            ball_y = min(ball_y + 1, 4)
        
        # 사용자 입력을 통한 플랫폼 이동(간소화된 예제)
        if button_a.is_pressed():
            user_score += 1
        elif button_b.is_pressed():
            user_score -= 1
        
        # 공과 플랫폼 충돌 확인
        if ball_x == 0 and ball_y == user_score:
            opponent_score += 1
        
        display.show(str(user_score) + ":" + str(opponent_score))
        sleep(500)

# 스네이크 게임(간소화된 예제)
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

# 게임 실행
pong_game()
```