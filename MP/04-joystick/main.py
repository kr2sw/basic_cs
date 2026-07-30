# Joystick

from microbit import *

def setup():
    display.scroll("Joystick Test")
    sleep(2000)
    display.clear()
    
    while True:
        x = accelerometer.get_x()
        y = accelerometer.get_y()
        
        if abs(x) > 100 and abs(y) < 100:
            if x < 0:
                led.plot(0, 0)
                display.show(Image.LEFT)
            else:
                led.plot(4, 0)
                display.show(Image.RIGHT)
        elif abs(y) > 100 and abs(x) < 100:
            if y < 0:
                led.plot(2, 0)
                display.show(Image.UP)
            else:
                led.plot(2, 4)
                display.show(Image.DOWN)
        else:
            led.clear()
            display.clear()
        sleep(100)