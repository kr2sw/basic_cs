# 12일차: 로봇 공학

## 개념 소개

이 수업에서는 마이크로비트를 이용한 로봇 공학 제어에 대해 배우게 됩니다. 주요 개념은 다음과 같습니다:

1. **서보 모터 제어**: 서보 모터를 이용한 정밀한 각도 제어
2. **바퀴 로봇**: 두 개의 바퀴를 이용한 이동
3. **모터 드라이버**: H-브리지 또는 전용 드라이버를 이용한 모터 제어
4. **이동 제어**: 전진, 후진, 좌회전, 우회전
5. **센서 기반 제어**: 장애물 감지 및 경로 계획

## 예시 코드

```python
from microbit import *

def setup_motors():
    # PWM 핀 설정 (예시)
    motor_left_pwm = pin0
    motor_right_pwm = pin1
    motor_left_dir = pin2
    motor_right_dir = pin3
    return motor_left_pwm, motor_right_pwm, motor_left_dir, motor_right_dir
def set_motor_speed(motor, speed, direction_pin):
    if speed < 0:
        direction_pin.write0()  # 후진
        speed = -speed
    else:
        direction_pin.write1()  # 전진
    motor.pulse_width_percent(speed)
def forward(left_pwm, right_pwm, left_dir, right_dir, speed=50):
    set_motor_speed(left_pwm, speed, left_dir)
    set_motor_speed(right_pwm, speed, right_dir)
def backward(left_pwm, right_pwm, left_dir, right_dir, speed=50):
    set_motor_speed(left_pwm, -speed, left_dir)
    set_motor_speed(right_pwm, -speed, right_dir)
def turn_left(left_pwm, right_pwm, left_dir, right_dir, speed=50):
    set_motor_speed(left_pwm, -speed, left_dir)
    set_motor_speed(right_pwm, speed, right_dir)
def turn_right(left_pwm, right_pwm, left_dir, right_dir, speed=50):
    set_motor_speed(left_pwm, speed, left_dir)
    set_motor_speed(right_pwm, -speed, right_dir)
def stop(left_pwm, right_pwm, left_dir, right_dir):
    left_pwm.write0()
    right_pwm.write0()
def obstacle_avoidance():
    # 초음파 센서를 이용한 장애물 감지 예시
    if button_a.is_pressed():
        forward(left_pwm, right_pwm, left_dir, right_dir)
    elif button_b.is_pressed():
        backward(left_pwm, right_pwm, left_dir, right_dir)
    else:
        # 장애물 감지 시 정지
        stop(left_pwm, right_pwm, left_dir, right_dir)
        for i in range(3):
            display.scroll("STOP")
            sleep(500)
    # 1초 대기
    sleep(1000)
def main():
    left_pwm, right_pwm, left_dir, right_dir = setup_motors()

    display.scroll("Robot Ready")
    sleep(2000)

    while True:
        obstacle_avoidance()

main()
```

## 키 개념

- **PWM 제어**: pulse_width_percent()를 이용한 속도 조절
- **방향 제어**: 별도의 GPIO를 이용한 전후진 제어
- **인터럽트 처리**: 버튼 press() 이벤트 처리
- **狀態 모니터링**: 센서 값을 이용한 의사 결정

## 실행 방법

1. 모든 모터 연결이 올바른지 확인 (PWM, 방향, 전원)
2. 보드를 컴퓨터에 USB로 연결
3. main.py 파일을 보드에 복사
4. A 버튼으로 전진, B 버튼으로 후진

## 개선 제안

- 초음파 센서 (HC-SR04) 추가
- 범프 센서 추가
- 더 부드러운 회피를 위한 PID 제어
- 더 많은 센서를 이용한 자율 주행