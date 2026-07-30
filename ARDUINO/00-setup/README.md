# 00 개발환경 설정

## 필수 도구

- **Arduino IDE** (https://www.arduino.cc/en/software)
- **Arduino CLI** (선택 사항, https://arduino.github.io/arduino-cli/)

## Arduino IDE 설치

1. 위 링크에서 운영체제에 맞는 Arduino IDE 다운로드 및 설치
2. 설치 후 USB로 보드 연결
3. **도구 > 보드 > 보드 매니저**에서 사용할 보드 패키지 설치 (예: Arduino AVR Boards)
4. **도구 > 포트**에서 연결된 보드의 포트 선택

## Arduino CLI 설치 및 설정

```bash
# Windows (scoop)
scoop install arduino-cli

# macOS
brew install arduino-cli

# Linux
curl -fsSL https://raw.githubusercontent.com/arduino/arduino-cli/master/install.sh | sh
```

### 초기 설정

```bash
arduino-cli config init
arduino-cli core update-index
arduino-cli core install arduino:avr
```

### 컴파일 및 업로드

```bash
cd 01-introduction
arduino-cli compile --fqbn arduino:avr:uno .
arduino-cli upload --fqbn arduino:avr:uno -p <PORT>
```

## 보드 종류별 FQBN

| 보드 | FQBN |
|------|------|
| Arduino Uno | `arduino:avr:uno` |
| Arduino Nano | `arduino:avr:nano` |
| Arduino Mega | `arduino:avr:mega` |
| ESP8266 | `esp8266:esp8266:nodemcuv2` |
| ESP32 | `esp32:esp32:esp32` |
| Raspberry Pi Pico | `rp2040:rp2040:rpipico` |
