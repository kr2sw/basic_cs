# 23: 고급 BLE — GATT 서비스, 커스텀 캐릭터리스틱, 노티피케이션
# 대상: ESP32 (BLE 지원), 스마트폰(nRF Connect)에서 테스트
import bluetooth
import struct
import time
from machine import Pin, Timer

# 온보드 LED (ESP32 DevKit)
led = Pin(2, Pin.OUT)

# --- 커스텀 GATT 서비스 정의 -------------------------------------
# 16비트 커스텀 UUID 사용 (표준 서비스 UUID가 아님)
_SERVICE_UUID = bluetooth.UUID(0xAAAA)
_CHAR_LED_UUID = bluetooth.UUID(0xBBBB)     # LED on/off 쓰기용
_CHAR_TEMP_UUID = bluetooth.UUID(0xCCCC)    # 온도 알림용

# (서비스 UUID, (캐릭터리스틱 정의...)) 튜플 구조
_LED_SERVICE = (
    _SERVICE_UUID,
    (
        (_CHAR_LED_UUID, bluetooth.FLAG_READ | bluetooth.FLAG_WRITE),
        (_CHAR_TEMP_UUID, bluetooth.FLAG_READ | bluetooth.FLAG_NOTIFY),
    ),
)

_ADV_APPEARANCE = 0x0003  # Generic Sensor


class LEDBLEServer:
    def __init__(self):
        self.ble = bluetooth.BLE()
        self.ble.active(True)
        self.ble.irq(self._irq)
        ((self._led_handle, self._temp_handle),) = \
            self.ble.gatts_register_services((_LED_SERVICE,))

        self._connected = False
        self._led_state = 0
        self._connections = set()
        self._led_value = struct.pack("<B", self._led_state)
        self.ble.gatts_write(self._led_handle, self._led_value)
        self._advertise()

    def _irq(self, event, data):
        """BLE 이벤트 콜백"""
        if event == bluetooth.IRQ_CENTRAL_CONNECT:
            # 연결됨 — 광고 중단
            conn_handle, addr_type, addr = data
            self._connections.add(conn_handle)
            self._connected = True
            print(f"연결됨: {addr.hex()}")
        elif event == bluetooth.IRQ_CENTRAL_DISCONNECT:
            conn_handle, addr_type, addr = data
            self._connections.discard(conn_handle)
            self._connected = len(self._connections) > 0
            print("연결 해제 — 재광고 시작")
            self._advertise()
        elif event == bluetooth.IRQ_GATTS_WRITE:
            conn_handle, attr_handle = data
            if attr_handle == self._led_handle:
                value = self.ble.gatts_read(self._led_handle)
                self._led_state = value[0] & 1
                led.value(self._led_state)          # 실제 LED 제어
                print(f"LED 쓰기 수신: {self._led_state}")
        elif event == bluetooth.IRQ_GATTS_READ_REQUEST:
            conn_handle, attr_handle = data
            if attr_handle == self._temp_handle:
                return None                          # 기본값 반환

    def _advertise(self):
        """광고 패킷 구성: 이름 + 서비스 UUID"""
        name = bytes("ESP32-BLE", "utf8")
        payload = bytearray()
        payload += bytes((len(name) + 1, 0x09)) + name
        payload += bytes((0x02, 0x01, 0x06))
        payload += bytes((0x03, 0x03, 0xAA, 0xAA))   # 0xAAAA 서비스
        self.ble.gap_advertise(100, payload)

    def notify_temperature(self, temperature):
        """온도 값을 float로 인코딩해 연결된 모든 클라이언트에 알림"""
        value = struct.pack("<f", temperature)
        self.ble.gatts_write(self._temp_handle, value)
        for conn in self._connections:
            self.ble.gatts_notify(conn, self._temp_handle, value)

    def run(self):
        """가짜 온도 센서로 2초마다 노티피케이션 전송"""
        temp = 20.0
        while True:
            # 간단한 온도 시뮬레이션 (실제 센서로 교체 가능)
            temp += 0.2
            if temp > 30.0:
                temp = 20.0
            if self._connected:
                self.notify_temperature(temp)
                print(f"알림 전송: {temp:.1f}°C")
            else:
                print("대기 중 — 앱에서 연결하세요")
            time.sleep_ms(2000)


def main():
    print("BLE 고급 레슨 시작 — 서비스 0xAAAA")
    server = LEDBLEServer()
    server.run()


if __name__ == "__main__":
    main()
