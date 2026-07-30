# Motors

import machine

# PWM 모터 제어 설정
motor_pwm = machine.PWM(machine.Pin(0))
motor_pwm.freq(1000)

# 방향 제어 핀
motor_dir = machine.Pin(1)

def setup():
    display.scroll("Motor Control")
    sleep(2000)
    display.clear()

def forward():
    led.plot(0, 0)
    motor_dir.value(1)  # 전진 방향
    motor_pwm.duty(2047)  # 50% 듀티 사이클

def backward():
    led.plot(4, 0)
    motor_dir.value(0)  # 후진 방향
    motor_pwm.duty(2047)  # 50% 듀티 사이클

def stop():
    led.clear()
    motor_pwm.duty(0)  # 정지

button_a.on_pressed(forward)
button_b.on_pressed(backward)
button_ab.on_pressed(stop)

while True:
    sleep(100)