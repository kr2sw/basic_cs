# 33: LoRa — LoRa 모듈, 게이트웨이, 장거리 통신
# 대상: ESP32 + RFM95/SX1276 (드라이버가 설치된 경우), 미설치 시 구조 학습용
# 아래 드라이버 import 실패해도 개념 데모는 계속 동작하도록 구성
import time
from machine import Pin, SPI

# 드라이버가 있으면 사용, 없으면 시뮬레이션 (하드웨어 없이도 학습 가능)
try:
    from sx127x import SX127x
    HAVE_DRIVER = True
except ImportError:
    HAVE_DRIVER = False

FREQUENCY = 868.0     # 868MHz (지역 규정 확인 필요)
ADDRESS = 1           # 이 보드의 노드 주소

# --- LoRa 하드웨어 인터페이스 (ESP32 핀 매핑) ------------------------------
SCK, MOSI, MISO = 18, 23, 19
CS, RST, DIO0 = 5, 14, 26

class LoRaNode:
    """SX1276 래퍼: 존재 시 실제 제어, 없으면 시뮬레이션"""

    def __init__(self):
        self.lora = None
        self.rssi = -120
        if HAVE_DRIVER:
            self.spi = SPI(1, baudrate=1_000_000, polarity=0, phase=0,
                           sck=Pin(SCK), mosi=Pin(MOSI), miso=Pin(MISO))
            self.cs = Pin(CS, Pin.OUT)
            self.reset = Pin(RST, Pin.OUT)
            self.dio0 = Pin(DIO0, Pin.IN)
            self.lora = SX127x(self.spi, self.cs, self.reset, self.dio0,
                               frequency=FREQUENCY)
            self.lora.set_packet_mode()
            print("LoRa 드라이버 초기화 완료")
        else:
            print("sx127x 드라이버 없음 — 시뮬레이션 모드")

    def send(self, payload: bytes):
        """패킷 전송: [주소(1B)][시퀀스(1B)][데이터]"""
        seq = int(time.time()) & 0xFF
        packet = bytes([ADDRESS, seq]) + payload
        if self.lora:
            self.lora.send(packet)
            print(f"[TX] seq={seq} data={payload.decode()}")
        else:
            print(f"[TX-시뮬레이션] seq={seq} data={payload.decode()}")

    def receive(self, timeout=5):
        """패킷 수신, 수신 시 데이터와 RSSI 반환"""
        if self.lora:
            data = self.lora.receive(timeout=timeout)
            if data is not None and len(data) >= 2:
                src, seq = data[0], data[1]
                self.rssi = getattr(self.lora, "rssi", -120)
                return src, seq, data[2:]
            return None
        time.sleep(timeout)     # 시뮬레이션: 대기만
        return None


def run_sender(node):
    """주기적으로 센서 값을 브로드캐스트하는 노드"""
    print("송신 노드 시작 (5초 간격)")
    count = 0
    while True:
        temp = 22.0 + count * 0.1
        node.send(f"temp:{temp:.1f}".encode())
        count += 1
        time.sleep(5)


def run_gateway(node):
    """모든 노드의 패킷을 수신해 중계하는 게이트웨이"""
    print("게이트웨이 수신 대기 중")
    while True:
        result = node.receive(timeout=5)
        if result:
            src, seq, data = result
            text = data.decode(errors="replace")
            print(f"[RX] src={src} seq={seq} rssi={node.rssi} data={text}")
        else:
            print("[RX] 대기 중...")


def main():
    print("=== LoRa 데모 (노드 역할 선택) ===")
    node = LoRaNode()
    mode = 1   # 1=송신, 2=게이트웨이

    if mode == 1:
        run_sender(node)
    else:
        run_gateway(node)


if __name__ == "__main__":
    main()
