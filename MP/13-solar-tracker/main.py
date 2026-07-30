from microbit import *
import time

def setup_solar_tracker():
    light_sensor = pin0
    servo_pin = pin1
    return light_sensor, servo_pin
def angle_to_pulse_width(angle):
    return int((angle / 180) * 20000)
def set_servo_angle(servo_pin, angle):
    pulse_width = angle_to_pulse_width(angle)
    pin1.write_analog(pulse_width)
def find_optimal_angle(light_sensor):
    best_angle = 90
    best_light = 0

    for angle in range(-45, 226, 5):
        set_servo_angle(servo_pin, angle)
        time.sleep(50)
        light_value = light_sensor.read_analog()
        
        if light_value > best_light:
            best_light = light_value
            best_angle = angle
    
    return best_angle
def main():
    global servo_pin
    light_sensor, servo_pin = setup_solar_tracker()

    display.scroll("SolTrk")
    time.sleep(2000)

    while True:
        optimal_angle = find_optimal_angle(light_sensor)
        set_servo_angle(servo_pin, optimal_angle)
        
        display.show(str(optimal_angle))
        time.sleep(2000)

main()