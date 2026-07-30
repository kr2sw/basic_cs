# Sensors

import machine

# 온도 센서 (DS18B20)
def setup():
    display.scroll("Temperature")
    sleep(2000)
    display.clear()
    
    # 온도 센서 설정
    temp_sensor = machine.DS18B20(machine.Pin(0))
    
    # 온도 읽기
    temperature = temp_sensor.read_temp()
    display.show(str(temperature) + "C")
    sleep(2000)
    display.clear()

# 빛 센서 (LDR)
def setup():
    display.scroll("Light Sensor")
    sleep(2000)
    display.clear()
    
    # 빛 센서 설정
    light_sensor = machine.Pin(1)
    
    # 빛 level 읽기
    luminosity = light_sensor.read_analog()
    display.show(str(luminosity))
    sleep(2000)
    display.clear()

# 센서 읽기
while True:
    if button_a.is_pressed():
        display.scroll("Temperature")
        temp_sensor = machine.DS18B20(machine.Pin(0))
        temperature = temp_sensor.read_temp()
        display.show(str(temperature) + "C")
        sleep(2000)
    
    if button_b.is_pressed():
        display.scroll("Light")
        light_sensor = machine.Pin(1)
        luminosity = light_sensor.read_analog()
        display.show(str(luminosity))
        sleep(2000)
    
    if button_ab.is_pressed():
        display.scroll("Calibration")
        for i in range(10):
            temp = machine.DS18B20(machine.Pin(0)).read_temp()
            display.show(str(temp) + "C")
            sleep(200)
        display.clear()
        sleep(1000)