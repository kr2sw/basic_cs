# MicroPython 과정 (기초 20개 + 중급 20개 챕터)

MicroPython은 마이크로컨트롤러와 제한된 리소스 환경에서 실행되도록 최적화된 Python 3 구현체입니다.

## 역사

MicroPython은 2013년 Damien George가 Kickstarter 캠페인을 통해 처음 공개했습니다. 원래는 STM32 기반의 pyboard를 위해 개발되었으며, Python 3.4의 핵심 기능을 256KB 플래시, 16KB RAM 환경에서 실행할 수 있도록 최적화했습니다. 2016년 BBC micro:bit에 MicroPython이 탑재되면서 교육 분야에서 널리 사용되기 시작했습니다. 2021년 Raspberry Pi Pico가 RP2040 칩과 함께 출시되면서 MicroPython은 더욱 대중화되었으며, 현재는 ESP32, ESP8266, STM32, nRF 등 다양한 보드를 지원합니다.

## 특징

- **Python 3 호환**: 데스크톱 Python과 유사한 문법, 표준 라이브러리 서브셋
- **인터랙티브 REPL**: USB 시리얼로 연결하면 즉시 코드 실행 및 테스트 가능
- **적은 리소스**: 256KB 플래시, 16KB RAM에서도 실행 가능
- **하드웨어 직접 제어**: GPIO, I2C, SPI, UART, ADC, PWM 등을 Python으로 직접 제어
- **파일 시스템**: 내부 플래시 메모리에 Python 스크립트 저장 및 실행
- **방대한 커뮤니티 라이브러리**: 센서, 디스플레이, 통신 모듈용 드라이버 풍부

## 실행

```bash
# Thonny IDE에서 파일 열기 → 실행
# 또는 ampy로 업로드
ampy --port COM3 put main.py
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | MicroPython 소개, REPL, LED 제어 |
| 02 | Buttons | 버튼 입력, 인터럽트, 풀업/풀다운 |
| 03 | LED Display | LED 매트릭스, 7세그먼트, NeoPixel |
| 04 | Joystick | 조이스틱 입력, 아날로그 값 매핑 |
| 05 | Motors | DC 모터, 서보 모터, 스테퍼 모터 |
| 06 | Sensors | 온도/습도/거리 센서, I2C 센서 |
| 07 | NFC | NFC/RFID 리더, MIFARE 카드 |
| 08 | Bluetooth | BLE 통신, 데이터 송수신 |
| 09 | Music | 부저, 멜로디, 사운드 출력 |
| 10 | Games | 간단한 게임 제작, 디스플레이 활용 |
| 11 | Data Logging | CSV 기록, SD 카드, 시계열 데이터 |
| 12 | Robotics | 로봇 팔 제어, 라인 트레이서 |
| 13 | Solar Tracker | 태양광 추적 시스템, 광센서 |
| 14 | Weather Station | 기상 관측소, 센서 융합 |
| 15 | Smart Home | 홈 오토메이션, 릴레이 제어 |
| 16 | Health Monitoring | 심박수/체온 측정, IoT 전송 |
| 17 | Educational Games | 학습용 게임, 퀴즈 프로그램 |
| 18 | Wearables | 웨어러블 디바이스, 저전력 설계 |
| 19 | Internet of Things | MQTT, HTTP 클라이언트, 클라우드 |
| 20 | Artificial Intelligence | TinyML, Edge AI, 센서 데이터 분석 |
| 21 | I2C Advanced | 고급 I2C, 다중 디바이스, 스캔, 레지스터 직접 접근 |
| 22 | SPI | SPI 통신, SPI 기기, 비트뱅킹, OLED |
| 23 | BLE Advanced | GATT 서비스, 커스텀 캐릭터리스틱, 노티피케이션 |
| 24 | MQTT Advanced | QoS, retained, TLS, last will |
| 25 | File Systems | LittleFS, 데이터 영속화, 파일 구조 |
| 26 | Networking | TCP 소켓 서버/클라이언트, DNS |
| 27 | uasyncio | 비동기 태스크, 이벤트 루프 |
| 28 | State Machine | 이벤트 기반 설계, HSM |
| 29 | Sensor Fusion | 이동 평균, 칼만 필터 기초 |
| 30 | PWM Advanced | 서보, 사운드, LED 밝기 곡선 |
| 31 | Display Graphics | framebuffer, 도형, 폰트 |
| 32 | Power Management | 딥 슬립, 주기 웨이크, 전류 최적화 |
| 33 | LoRa | LoRa 모듈, 게이트웨이, 장거리 통신 |
| 34 | Edge AI | TensorFlow Lite Micro 개념, 센서 분류 |
| 35 | Security | 암호화, 키 관리, 보안 부팅 개념 |
| 36 | RTOS Concepts | 태스크, 큐, 세마포어 (uasyncio 관점) |
| 37 | Multi-Board | ESP32 + Pico 통신, 프레임워크 |
| 38 | Web Server | MicroWebSrv 유사 구현, REST |
| 39 | OTA | 원격 펌웨어 업데이트 개념 |
| 40 | Final Project | IoT 환경 모니터링 시스템 |

## 중급 과정 (21~40)

기초 과정에서 다진 마이크로컨트롤러 지식을 바탕으로, 통신 프로토콜 심화(I2C/SPI/BLE/MQTT), 시스템 설계(상태 머신, RTOS 개념, 파일 시스템), 그리고 IoT 실전 기술(OTA, 보안, LoRa, 엣지 AI)까지 다룹니다. ESP32/Pico 보드를 기준으로 작성되었으며 하드웨어 연결은 각 챕터의 README에 설명되어 있습니다.
