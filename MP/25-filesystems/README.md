# 25: 파일 시스템 — LittleFS, Data Persistence, Layout

## 개요

마이크로컨트롤러의 내부 플래시는 **파일 시스템**으로 구성되어 있습니다. MicroPython은 기본적으로 **LittleFS**를 사용하며, Python 스크립트뿐 아니라 센서 데이터, 설정값, 로그를 파일로 저장할 수 있습니다. 전원을 꺼도 데이터가 유지되는 **영속화(persistence)** 의 기초를 배웁니다.

## 파일 시스템 구조

LittleFS는 작은 임베디드 장치용 플래시 파일 시스템으로, 다음 특징이 있습니다:

- 갑작스러운 전원 차단에도 데이터 손상이 적음 (저널링)
- 플래시의 쓰기 수명(수만~수십만 회)을 고려해 **버퍼링/배치** 쓰기 권장
- 크기 제한: ESP32는 보통 몇 MB, Pico는 1MB 내외

```python
import os
print(os.listdir("/"))          # 루트 파일 목록
print(os.getcwd())              # 현재 디렉터리
os.mkdir("/data")               # 디렉터리 생성
```

## JSON으로 설정 저장/불러오기

기기가 재시작해도 유지해야 할 Wi-Fi 설정, 임계값 등을 JSON 파일로 저장합니다.

```python
import json, os

CONFIG_FILE = "/config.json"

def save_config(data):
    with open(CONFIG_FILE, "w") as f:
        json.dump(data, f)

def load_config():
    if CONFIG_FILE not in os.listdir("/"):
        return {"threshold": 30.0, "interval": 10}
    with open(CONFIG_FILE) as f:
        return json.load(f)
```

## 데이터 로깅과 배치 쓰기

센서 데이터를 계속 파일에 쓰면 플래시 수명이 빨리 닳습니다. 메모리에 모아 두고 **일정 개수마다 한 번에** 기록합니다.

```python
logs = []
def add_log(temp):
    logs.append(temp)
    if len(logs) >= 10:              # 10개마다 flush
        with open("/data/temp.csv", "a") as f:
            f.write("\n".join(map(str, logs)) + "\n")
        logs.clear()
```

## 파일/디렉터리 관리

```python
os.remove("/old.txt")        # 파일 삭제
os.rename("/a.txt", "/b.txt")  # 이름 변경
os.stat("/data/temp.csv")    # 크기/수정 시간 확인
```

## 실행/업로드 방법

1. **Thonny IDE**: 파일을 열어 실행하면 현재 디렉터리 트리를 시리얼 출력으로 보여줍니다.
2. **ampy**: 보드에 `main.py` 업로드 후 실행:
   ```bash
   ampy --port COM3 put MP/25-filesystems/main.py
   ampy --port COM3 run MP/25-filesystems/main.py
   ```
3. main.py를 두 번 실행하면 이전에 저장한 설정이 로드되는 것을 확인할 수 있습니다.
4. Thonny의 파일 패널에서 보드 내부 `/data` 폴더를 직접 탐색할 수 있습니다.

## 핵심 개념 요약

- LittleFS: 전원 차단에도 안전한 플래시 파일 시스템
- JSON 설정 파일로 기기 상태 영속화
- 버퍼링/배치 쓰기로 플래시 수명 보호
- `os` 모듈로 파일·디렉터리 관리
