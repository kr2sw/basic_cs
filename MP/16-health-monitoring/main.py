from microbit import *
import time

class HealthMonitor:
    def __init__(self):
        self.heartbeat = 0
        self.distance = 0
        self.calories = 0
        self.steps = 0
        self.stress_level = 0
        self.movement_threshold = 10
    def simulated_heartbeat(self):
        import random
        bpm = 60 + random.uniform(-20, 20)
        self.heartbeat = int(bpm)
        if bpm > 100:
            display.scroll("HIGH HR")
        elif bpm < 50:
            display.scroll("LOW HR")
        else:
            display.show(Image.HEART)
        return bpm
    def measured_distance(self):
        import random
        dist = random.uniform(10, 200)
        self.distance = int(dist)
        
        if dist < 30:
            display.scroll("Near")
        elif dist < 100:
            display.scroll("Medium")
        else:
            display.scroll("Far")
        
        return dist
    def calculate_metrics(self):
        self.calories = int(self.steps * 0.05)
        self.stress_level = (self.distance / 100) * (self.heartbeat / 80)
        self.stress_level = min(self.stress_level, 10)
    def display_status(self):
        status = f"HR:{self.heartbeat} D:{self.distance} C:{self.calories} S:{self.stress_level:.1f}"
        display.scroll(status)
        sleep(1000)
    
    def show_status_icon(self):
        if self.heartbeat > 100:
            display.show(Image.SKULL)
        elif self.distance < 30:
            display.show(Image.SMILE)
        elif self.stress_level > 7:
            display.show(Image.ANGRY)
        elif self.heartbeat < 50:
            display.show(Image.CONFUSED)
        else:
            display.show(Image.HAPPY)
    def main_loop(self):
        while True:
            self.simulated_heartbeat()
            self.measured_distance()
            
            self.steps += 1
            
            self.calculate_metrics()
            
            self.display_status()
            
            self.show_status_icon()
            
            sleep(2000)
def main():
    monitor = HealthMonitor()
    
    display.scroll("Health Monitor")
    sleep(2000)
    
    monitor.main_loop()

main()