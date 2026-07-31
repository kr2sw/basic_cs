# 40: 종합 프로젝트 — IoT 환경 모니터링 시스템
# 대상: ESP32 + DHT22(온습도) + SSD1306 OLED + MQTT
# 이 코드는 21~39장에서 배운 기능을 하나의 시스템으로 통합합니다
import network
import json
import time
from machine import Pin, I2C
import ssd1306

try:
    import dht
    HAVE_DHT = True
except ImportError:
    HAVE_DHT = False

try:
    from umqtt.simple import MQTTClient
    HAVE_MQTT = True
except ImportError:
    HAVE_MQTT = False

# --- 설정 -------------------------------------------------------------------
SSID = "YOUR_WIFI_SSID"
PASSWORD = "YOUR_WIFI_PASSWORD"
MQTT_BROKER = "test.mosquitto.org"
MQTT_TOPIC = "iot/monitor/room1"
THRESHOLD_TEMP = 28.0     # 알람 임계 온도

# --- 하드웨어 -----------------------------------------------------------------
i2c = I2C(0, scl=Pin(22), sda=Pin(21), freq=400_000)
oled = ssd1306.SSD1306_I2C(128, 64, i2c)
led_alarm = Pin(2, Pin.OUT)
sensor = dht.DHT22(Pin(4)) if HAVE_DHT else None

wlan = network.WLAN(network.STA_IF)

# --- 상태 저장 (25장 파일시스템 영속화) ----------------------------------------
def save_state(reading):
    try:
        with open("/data/last.json", "w") as f:
            json.dump(reading, f)
    except OSError:
        pass

def read_state():
    try:
        with open("/data/last.json") as f:
            return json.load(f)
    except (OSError, ValueError):
        return None

# --- 네트워크 (26장 소켓) --------------------------------------------------------
def connect_wifi():
    wlan.active(True)
    wlan.connect(SSID, PASSWORD)
    while not wlan.isconnected():
        time.sleep(0.5)
    print("Wi-Fi:", wlan.ifconfig()[0])

# --- MQTT (24장 QoS/retain) --------------------------------------------------------
def publish_reading(client, reading):
    if not HAVE_MQTT or client is None:
        return False
    try:
        payload = json.dumps(reading).encode()
        client.publish(MQTT_TOPIC, payload, qos=1, retain=True)
        print(f"MQTT 발행: {payload.decode()}")
        return True
    except OSError as e:
        print("MQTT 발행 실패:", e)
        return False

# --- 센서 읽기 ------------------------------------------------------------------------
def read_sensors():
    """DHT22 온습도 측정 (실패 시 이전 값 유지)"""
    if sensor:
        try:
            sensor.measure()
            return {"temp": sensor.temperature(), "hum": sensor.humidity()}
        except OSError:
            pass
    prev = read_state()
    return prev or {"temp": 0.0, "hum": 0.0}

# --- 디스플레이 (31장 그래픽) ------------------------------------------------------------
def display_reading(reading):
    oled.fill(0)
    oled.text("Room Monitor", 0, 0)
    oled.text(f"Temp: {reading['temp']:.1f}C", 0, 16)
    oled.text(f"Hum:  {reading['hum']:.0f}%", 0, 32)
    oled.text("MQTT: " + ("OK" if HAVE_MQTT else "NO"), 0, 48)
    oled.show()

# --- 알람 (28장 상태 머신) -------------------------------------------------------------------
class AlarmFSM:
    NORMAL, ALARM = "NORMAL", "ALARM"

    def __init__(self):
        self.state = self.NORMAL

    def on_reading(self, reading):
        temp = reading["temp"]
        if temp >= THRESHOLD_TEMP and self.state == self.NORMAL:
            self.state = self.ALARM
            print(f"[알람] 온도 {temp:.1f}°C — 임계 초과")
        elif temp < THRESHOLD_TEMP and self.state == self.ALARM:
            self.state = self.NORMAL
            print("[정상] 온도 회복")
        led_alarm.value(1 if self.state == self.ALARM else 0)
        return self.state


def main():
    print("=== IoT 환경 모니터링 시스템 ===")
    connect_wifi()

    client = None
    if HAVE_MQTT:
        client = MQTTClient("esp32-monitor", MQTT_BROKER)
        client.set_last_will(MQTT_TOPIC + "/status", b"offline", retain=True)
        client.connect()
        client.publish(MQTT_TOPIC + "/status", b"online", retain=True)
        print(f"MQTT 연결: {MQTT_BROKER}")

    alarm = AlarmFSM()
    last_report = time.time()

    print("모니터링 시작 — 5초 간격 측정")
    while True:
        reading = read_sensors()
        state = alarm.on_reading(reading)
        reading["alarm"] = state
        display_reading(reading)
        save_state(reading)

        # 30초마다 클라우드 보고 (플래시/배터리 절약)
        if time.time() - last_report >= 30:
            publish_reading(client, reading)
            last_report = time.time()

        time.sleep(5)


if __name__ == "__main__":
    main()
