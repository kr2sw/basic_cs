# 37: 멀티보드 — ESP32 + Pico 통신, 프레임워크
# UART 프레임 프로토콜: 센서 노드(Pico) ↔ 게이트웨이(ESP32)
# 한 코드에서 MODE로 역할 선택 (각 보드에 맞게 수정)
import json
import time
from machine import UART, Pin

# --- 역할 설정: "SENSOR"=Pico(센서 노드), "GATEWAY"=ESP32(게이트웨이)
MODE = "GATEWAY"

# UART 핀 설정 (보드별)
if MODE == "SENSOR":
    uart = UART(0, baudrate=9600, tx=Pin(0), rx=Pin(1))   # Pico
else:
    uart = UART(2, baudrate=9600, tx=Pin(17), rx=Pin(16)) # ESP32

# --- 프레임 프로토콜 --------------------------------------------------------
START, END = 0x7E, 0x7F
TYPE_SENSOR = 0x01
TYPE_COMMAND = 0x02

def make_frame(msg_type, payload: bytes) -> bytes:
    """[START][TYPE][LEN][PAYLOAD][CHECKSUM][END] 프레임 생성"""
    checksum = (msg_type + len(payload) + sum(payload)) & 0xFF
    return bytes([START, msg_type, len(payload)]) + payload \
           + bytes([checksum, END])

def parse_frames(stream: bytearray):
    """바이트 스트림에서 완성된 프레임을 찾아 반환 (수신 버퍼 처리)"""
    frames = []
    while True:
        if len(stream) < 4:
            break
        if stream[0] != START:
            stream.pop(0)                      # START까지 스킵
            continue
        msg_type, length = stream[1], stream[2]
        total = 4 + length                     # START+TYPE+LEN+PAYLOAD+CHK+END
        if len(stream) < total:
            break                              # 프레임 아직 미완성
        payload = bytes(stream[3:3 + length])
        checksum = stream[3 + length]
        if checksum != (msg_type + length + sum(payload)) & 0xFF:
            print("[오류] 체크섬 불일치 — 프레임 폐기")
        else:
            frames.append((msg_type, payload))
        del stream[:total]                     # 처리한 프레임 제거
    return frames

# --- 센서 노드 (Pico) ---------------------------------------------------------
def run_sensor_node():
    print("센서 노드 시작 — 3초마다 데이터 전송")
    count = 0
    while True:
        payload = json.dumps({
            "seq": count,
            "temp": 21.0 + count * 0.1,
            "hum": 45.0 + count % 5,
        }).encode()
        frame = make_frame(TYPE_SENSOR, payload)
        uart.write(frame)
        print(f"[TX] {payload.decode()}")
        count += 1
        time.sleep(3)

# --- 게이트웨이 (ESP32) --------------------------------------------------------
def run_gateway():
    print("게이트웨이 시작 — 프레임 수신 대기")
    buffer = bytearray()
    while True:
        if uart.any():
            buffer.extend(uart.read())          # 수신 바이트 추가
        for msg_type, payload in parse_frames(buffer):
            if msg_type == TYPE_SENSOR:
                data = json.loads(payload.decode())
                print(f"[RX] 센서: seq={data['seq']} "
                      f"temp={data['temp']:.1f} hum={data['hum']:.0f}%")
                # 여기서 Wi-Fi/MQTT로 클라우드 전송 가능
                # (network.WLAN → umqtt.simple 로 확장)
        time.sleep(0.1)


def main():
    if MODE == "SENSOR":
        run_sensor_node()
    else:
        run_gateway()


if __name__ == "__main__":
    main()
