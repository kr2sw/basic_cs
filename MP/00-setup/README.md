# 00 개발환경 설정

## 필수 도구

- **MicroPython 펌웨어** - 보드에 맞는 펌웨어
- **Thonny IDE** (권장, https://thonny.org/)
- **Python 3** - 펌웨어 플래싱 도구 실행용
- **ampy** 또는 **rshell** (선택 사항, CLI 업로드용)

## MicroPython 펌웨어 설치

### Raspberry Pi Pico
1. [MicroPython 다운로드 페이지](https://micropython.org/download/rp2-pico/) 방문
2. `.uf2` 파일 다운로드
3. Pico의 BOOTSEL 버튼을 누른 상태로 USB 연결
4. `RPI-RP2` 드라이브에 `.uf2` 파일 복사
5. 자동으로 재부팅되며 MicroPython 설치 완료

### ESP32/ESP8266
```bash
# esptool 설치
pip install esptool

# ESP32 펌웨어 플래싱
esptool.py --port <PORT> --baud 460800 write_flash 0 <펌웨어.bin>
```

## Thonny IDE 설정

1. Thonny 설치 후 실행
2. **실행 > 인터프리터 선택**
3. **MicroPython (Raspberry Pi Pico)** 또는 해당 보드 선택
4. 포트 자동 감지됨

## CLI 업로드 도구

```bash
pip install adafruit-ampy

# 파일 업로드
ampy --port <PORT> put main.py

# 파일 실행
ampy --port <PORT> run main.py

# REPL 접속
ampy --port <PORT> repl
```

## VS Code 확장

- **MicroPython** (dawidd6)
- **Pico-W-Go** (Raspberry Pi Pico 전용)
