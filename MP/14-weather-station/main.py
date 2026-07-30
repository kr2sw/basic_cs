from microbit import *
import time
import random

class WeatherStation:
    def __init__(self):
        self.data = {
            'temperature': 0,
            'humidity': 0,
            'pressure': 0,
            'light': 0
        }
    def simulate_sensor_data(self):
        self.data['temperature'] = 20 + random.uniform(-5, 5)
        self.data['humidity'] = 50 + random.uniform(-20, 20)
        self.data['pressure'] = 1013 + random.uniform(-10, 10)
        self.data['light'] = 500 + random.uniform(-100, 100)
    def display_weather(self):
        self.simulate_sensor_data()
        
        temp = f"{self.data['temperature']:.1f}°C"
        humi = f"{self.data['humidity']:.0f}%"
        pres = f"{self.data['pressure']:.0f}hPa"
        
        display.scroll(f"T:{temp} H:{humi}")
        sleep(1000)
        
        display.scroll(f"P:{pres}")
        sleep(1000)
        
        display.show(Image.SUN if self.data['light'] > 700 else Image.CLOUD)
    def upload_to_cloud(self):
        try:
            data_string = ",".join([
                str(self.data['temperature']),
                str(self.data['humidity']),
                str(self.data['pressure']),
                str(self.data['light']),
                str(int(time.time() * 1000))
            ])
            pybytes.send_data(data_string)
            display.scroll("Upload")
        except:
            display.scroll("NoNet")
    def main_loop(self):
        while True:
            self.display_weather()
            self.upload_to_cloud()
            sleep(5000)
def main():
    station = WeatherStation()
    
    display.scroll("WS Ready")
    sleep(2000)
    
    station.main_loop()

main()