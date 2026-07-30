import microbit
import time

class Motor:
    """DC 모터 제어 클래스"""
    def __init__(self, forward_pin, backward_pin, speed_pin=2):
        self.forward = microbit.Pin(forward_pin)
        self.backward = microbit.Pin(backward_pin)
        self.speed = microbit.PWM(speed_pin)
        self.speed.frequency(1000)
        self.stop()

    def forward_(self, speed=100):
        self.forward.write(1)
        self.backward.write(0)
        self.speed.write(speed / 100.0)

    def backward(self, speed=100):
        self.forward.write(0)
        self.backward.write(1)
        self.speed.write(speed / 100.0)

    def turn_left(self, speed=100):
        self.forward.write(0)
        self.backward.write(0)

    def turn_right(self, speed=100):
        self.forward.write(1)
        self.backward.write(1)

    def stop(self):
        self.forward.write(0)
        self.backward.write(0)
        self.speed.write(0)

class Robot:
    """로봇 제어 클래스"""
    def __init__(self):
        self.left_motor = Motor(0, 1)
        self.right_motor = Motor(2, 3)

    def move_forward(self, duration_ms=1000, speed=100):
        self.left_motor.forward_(speed)
        self.right_motor.forward_(speed)
        time.sleep(duration_ms / 1000.0)
        self.stop()

    def move_backward(self, duration_ms=1000, speed=100):
        self.left_motor.backward(speed)
        self.right_motor.backward(speed)
        time.sleep(duration_ms / 1000.0)
        self.stop()

    def turn_left(self, duration_ms=500, speed=100):
        self.left_motor.backward(speed)
        self.right_motor.forward(speed)
        time.sleep(duration_ms / 1000.0)
        self.stop()

    def turn_right(self, duration_ms=500, speed=100):
        self.left_motor.forward(speed)
        self.right_motor.backward(speed)
        time.sleep(duration_ms / 1000.0)
        self.stop()

    def stop(self):
        self.left_motor.stop()
        self.right_motor.stop()

    def execute_sequence(self, sequence):
        for action, duration, speed in sequence:
            if action == 'forward':
                self.move_forward(duration, speed)
            elif action == 'backward':
                self.move_backward(duration, speed)
            elif action == 'left':
                self.turn_left(duration, speed)
            elif action == 'right':
                self.turn_right(duration, speed)
            elif action == 'stop':
                self.stop()
            time.sleep(0.1)

def main():
    robot = Robot()
    print("로봇 시퀀스 시작")

    # 기본 명령어 시퀀스
    sequence = [
        ('forward', 1000, 80),
        ('stop', 500, 100),
        ('turn_left', 500, 100),
        ('forward', 1000, 80),
        ('stop', 500, 100),
        ('turn_right', 500, 100),
        ('forward', 1000, 80),
        ('stop', 500, 100),
        ('backward', 1000, 80),
        ('stop', 500, 100),
    ]

    robot.execute_sequence(sequence)
    print("시퀀스 완료")

if __name__ == "__main__":
    main()