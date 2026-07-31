# 39: OTA — 원격 펌웨어 업데이트 개념
# 대상: ESP32 + Wi-Fi
# 주의: 실제 플래시 기록(esptool)은 파티션 설정이 필요하므로,
# 이 코드는 OTA 프로세스(다운로드→검증→적용)를 안전하게 시뮬레이션합니다.
import network
import socket
import hashlib
import json
import time
from machine import Pin, reset

SSID = "YOUR_WIFI_SSID"
PASSWORD = "YOUR_WIFI_PASSWORD"
SERVER = "192.168.0.100"      # PC(펌웨어 서버) IP
PORT = 8000
FIRMWARE_URL = f"/firmware.bin"

CURRENT_VERSION = "1.0.0"
# 서버가 제공하는 새 버전 메타데이터 (예: /version.json)
EXPECTED_HASH = None          # 실제 운영 시 서버에서 받아 비교

wlan = network.WLAN(network.STA_IF)

def connect_wifi():
    wlan.active(True)
    wlan.connect(SSID, PASSWORD)
    while not wlan.isconnected():
        time.sleep(0.5)
    print("Wi-Fi 연결:", wlan.ifconfig()[0])


def http_get(path, max_size=64 * 1024):
    """HTTP GET으로 서버에서 파일 다운로드"""
    addr = socket.getaddrinfo(SERVER, PORT)[0][-1]
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.settimeout(10)
    try:
        s.connect(addr)
        request = f"GET {path} HTTP/1.0\r\nHost: {SERVER}\r\n\r\n"
        s.send(request.encode())
        data = b""
        while len(data) < max_size:
            chunk = s.recv(4096)
            if not chunk:
                break
            data += chunk
        # 헤더와 본문 분리
        if b"\r\n\r\n" in data:
            header, body = data.split(b"\r\n\r\n", 1)
            return body
        return b""
    except OSError as e:
        print("다운로드 실패:", e)
        return None
    finally:
        s.close()


def fetch_version():
    """서버에서 새 버전 정보(version.json) 조회"""
    body = http_get("/version.json", max_size=4096)
    if not body:
        return None
    try:
        return json.loads(body.decode())
    except ValueError:
        return None


def verify_firmware(firmware, expected_hash):
    """체크섬 검증: 다운로드한 펌웨어의 손상/위변조 감지"""
    digest = hashlib.sha256(firmware).hexdigest()
    print(f"  펌웨어 크기 : {len(firmware)} 바이트")
    print(f"  SHA-256     : {digest[:32]}...")
    if expected_hash is None:
        print("  [경고] 예상 해시 없음 — 검증 생략 (운영 시 반드시 비교)")
        return True
    if digest == expected_hash:
        print("  검증 통과")
        return True
    print("  검증 실패 — 펌웨어 적용 취소")
    return False


def simulate_apply(firmware):
    """실제 플래시 기록 대신 검증 결과를 시뮬레이션"""
    print("  플래시 기록 시뮬레이션 완료")
    print("  (실제 운영: espota/parttool로 OTA 파티션에 기록)")


def main():
    connect_wifi()

    print("=== OTA 업데이트 프로세스 ===")
    print(f"현재 버전: {CURRENT_VERSION}")

    # 1) 서버에서 버전 확인
    info = fetch_version()
    if info is None:
        print("버전 정보 조회 실패 — 서버 확인 필요")
        return
    server_version = info.get("version", "0.0.0")
    print(f"서버 버전: {server_version}")

    # 2) 버전 비교 — 낮으면 스킵
    if server_version <= CURRENT_VERSION:
        print("이미 최신 버전입니다")
        return

    # 3) 새 펌웨어 다운로드
    print(f"새 펌웨어 다운로드: {FIRMWARE_URL}")
    firmware = http_get(FIRMWARE_URL)
    if firmware is None or len(firmware) == 0:
        print("다운로드 실패")
        return

    # 4) 검증 후 적용
    if verify_firmware(firmware, EXPECTED_HASH):
        simulate_apply(firmware)
        # 실제 환경에서는 여기서 reset()을 호출하고,
        # 새 펌웨어가 부팅되면 정상 동작을 서버에 보고합니다.
        # 보고가 없으면 이전 파티션으로 롤백 (부팅 플래그 방식).
        print("업데이트 완료 — 재부팅 대기 (시뮬레이션)")
    else:
        print("업데이트 중단")


if __name__ == "__main__":
    main()
