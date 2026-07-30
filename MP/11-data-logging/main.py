from microbit import *
import time
import sleep

def current_time():
    return str(running_time())
def log_temperature_to_csv():
    temperature = 22.5
    timestamp = current_time()
    filename = "temperature_log.csv"

    try:
        with open(filename, 'a') as f:
            if f.tell() == 0:
                f.write("시간,온도(°C)\n")
            f.write(f"{timestamp},{temperature}\n")
        for i in range(3):
            display.scroll("LOG OK")
            sleep(200)
    except Exception as e:
        display.scroll("ERR")
        sleep(500)
def main():
    global temperature_log_running
    temperature_log_running = True
    while temperature_log_running:
        log_temperature_to_csv()
        sleep(5000)

temperature_log_running = False

main()