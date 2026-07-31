# 30: 고급 PWM — Servo, Sound, LED Brightness Curve
# 대상: ESP32 (서보=GPIO13, 부저=GPIO15, LED=GPIO2)
from machine import Pin, PWM
import time
import math

SERVO_PIN = 13
BUZZER_PIN = 15
LED_PIN = 2

# --- 1) 서보 모터 ----------------------------------------------------------
# 서보: 50Hz(20ms) 주기의 PWM. 펄스폭으로 각도를 지정
# 0.5ms(0°) ~ 1.5ms(90°) ~ 2.5ms(180°)
SERVO_MIN_MS = 0.5
SERVO_MAX_MS = 2.5
SERVO_PERIOD_MS = 20.0


def angle_to_duty(angle, freq=50, resolution=1024):
    """각도(0~180) → 16비트 듀티 값 변환"""
    # 듀티 비율 = 펄스폭 / 주기 → duty = ratio * (2^resolution - 1)
    ratio = (SERVO_MIN_MS + (angle / 180.0) * (SERVO_MAX_MS - SERVO_MIN_MS)) / SERVO_PERIOD_MS
    return int(ratio * 65535)   # 16비트 듀티 (ESP32)


def sweep_servo(servo):
    """서보를 0°→180° 왕복"""
    print("서보 스윕 시작")
    for angle in range(0, 181, 2):
        servo.duty_u16(angle_to_duty(angle))
        time.sleep_ms(10)
    for angle in range(180, -1, -2):
        servo.duty_u16(angle_to_duty(angle))
        time.sleep_ms(10)


# --- 2) 부저로 멜로디 ---------------------------------------------------------
# 각 음계의 주파수 (C4~C6)
NOTE_C4, NOTE_D4, NOTE_E4, NOTE_F4 = 262, 294, 330, 349
NOTE_G4, NOTE_A4, NOTE_B4, NOTE_C5 = 392, 440, 494, 523

TWINKLE = [NOTE_C4, NOTE_C4, NOTE_G4, NOTE_G4,
           NOTE_A4, NOTE_A4, NOTE_G4, 0]   # 0 = 쉼

def play_tone(buzzer, freq, duration_ms):
    if freq == 0:
        buzzer.duty_u16(0)                  # 쉼
    else:
        buzzer.freq(freq)
        buzzer.duty_u16(32768)              # 50% 듀티
    time.sleep_ms(duration_ms)
    buzzer.duty_u16(0)                      # 소리 끄기


def play_twinkle(buzzer):
    print("반짝반짝 작은 별 연주")
    for note in TWINKLE:
        play_tone(buzzer, note, 400)
        time.sleep_ms(50)
    time.sleep_ms(300)


# --- 3) LED 밝기 감마 보정 ------------------------------------------------------
def linear_brightness(pwm, brightness, max_duty=65535):
    """선형 밝기: 사람 눈은 저조도에 민감 → 어두운 부분이 거의 안 보임"""
    pwm.duty_u16(int(max_duty * brightness))


def gamma_brightness(pwm, brightness, gamma=2.2, max_duty=65535):
    """감마 보정 밝기: 밝기를 1/2.2 제곱으로 보정해 눈에 선형으로 보임"""
    duty = int(max_duty * (brightness ** gamma))
    pwm.duty_u16(duty)


def compare_brightness_curves(led):
    print("LED 밝기 곡선 비교")
    print("  50% 지점 → 선형은 32768, 감마 보정은 ~22700 (실제로는 눈에 밝게 보임)")
    for brightness in [0.0, 0.25, 0.5, 0.75, 1.0]:
        linear_brightness(led, brightness)
        time.sleep_ms(500)
        gamma_brightness(led, brightness)
        time.sleep_ms(500)
    led.duty_u16(0)


def main():
    servo = PWM(Pin(SERVO_PIN), freq=50)        # 서보는 반드시 50Hz
    buzzer = PWM(Pin(BUZZER_PIN), freq=440)
    led = PWM(Pin(LED_PIN), freq=1000)          # LED는 1kHz면 충분

    sweep_servo(servo)
    play_twinkle(buzzer)
    compare_brightness_curves(led)

    # 부드러운 페이드 (감마 보정 사용)
    print("감마 보정 페이드 인/아웃")
    for step in range(101):
        gamma_brightness(led, step / 100.0)
        time.sleep_ms(20)
    for step in range(100, -1, -1):
        gamma_brightness(led, step / 100.0)
        time.sleep_ms(20)

    servo.deinit()
    buzzer.deinit()
    led.deinit()
    print("완료")


if __name__ == "__main__":
    main()
