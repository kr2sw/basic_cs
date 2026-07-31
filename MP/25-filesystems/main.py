# 25: 파일 시스템 — LittleFS, 데이터 영속화, 구조
# 대상: ESP32/Pico 등 LittleFS를 쓰는 모든 보드
import os
import json
import time

CONFIG_FILE = "/config.json"
LOG_FILE = "/data/temp.log"
DATA_DIR = "/data"

# --- 파일 시스템 구조 보기 ------------------------------------------
def show_tree(path, indent=0):
    for entry in os.listdir(path):
        full = path + "/" + entry if path != "/" else "/" + entry
        print("  " * indent + entry)
        if os.stat(full)[0] & 0x4000:      # 디렉터리 비트 확인
            show_tree(full, indent + 1)


def ensure_layout():
    """필요한 디렉터리를 만들고 전체 구조를 출력"""
    if not os.path.exists(DATA_DIR):
        os.mkdir(DATA_DIR)
    print("=== 파일 시스템 구조 ===")
    show_tree("/")


# --- JSON 설정 영속화 -------------------------------------------------
DEFAULT_CONFIG = {
    "device_name": "sensor-01",
    "sample_interval": 5,
    "temperature_threshold": 30.0,
}


def load_config():
    try:
        with open(CONFIG_FILE) as f:
            return json.load(f)
    except (OSError, ValueError):
        print("설정 파일 없음 — 기본값 사용")
        return dict(DEFAULT_CONFIG)


def save_config(config):
    with open(CONFIG_FILE, "w") as f:
        json.dump(config)
    print(f"설정 저장됨: {config}")


# --- 버퍼링된 로그 쓰기 (플래시 수명 보호) ----------------------------
_log_buffer = []


def append_log(temp, humidity):
    """로그를 메모리 버퍼에 쌓고 20개마다 파일에 한 번에 기록"""
    global _log_buffer
    _log_buffer.append(f"{time.time()},{temp:.1f},{humidity:.1f}")
    if len(_log_buffer) >= 20:
        with open(LOG_FILE, "a") as f:
            f.write("\n".join(_log_buffer) + "\n")
        _log_buffer.clear()
        size = os.stat(LOG_FILE)[6]
        print(f"로그 flush (현재 {size} 바이트)")


def main():
    ensure_layout()

    print("\n=== 설정 로드 (영속성 확인) ===")
    config = load_config()
    print(f"기기: {config['device_name']}, 간격: {config['sample_interval']}s")

    # 설정을 조금 바꿔 저장 → 두 번째 실행에서 반영됨
    config["sample_interval"] = config["sample_interval"] + 1
    save_config(config)

    print("\n=== 시뮬레이션 센서 로깅 ===")
    # 실제 센서(예: DHT22)로 대체 가능한 시뮬레이션
    temp, humidity = 25.0, 50.0
    for i in range(45):
        temp += 0.1
        humidity += 0.05
        append_log(temp, humidity)
        time.sleep_ms(50)

    print("\n=== 로그 파일 내용 확인 ===")
    with open(LOG_FILE) as f:
        lines = f.readlines()
    print(f"총 {len(lines)} 라인, 최근 3줄:")
    for line in lines[-3:]:
        print("  " + line.strip())

    print("\n=== 파일 정리 (선택) ===")
    print("os.remove('/config.json') 로 설정 초기화 가능")


if __name__ == "__main__":
    main()
