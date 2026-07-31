# Arduino 강좌 (40개 챕터)

- 기초 과정: 01~20
- 중급 과정: 21~40

Arduino 마이크로컨트롤러 프로그래밍의 기초부터 고급 개념까지 학습할 수 있는 예제 모음입니다.

## 역사

Arduino는 2005년 이탈리아 Ivrea의 Interaction Design Institute에서 Massimo Banzi와 David Cuartielles가 학생들을 위해 개발했습니다. Wiring 프로젝트(2003, Hernando Barragán)를 기반으로 하여, 예술가와 디자이너도 쉽게 사용할 수 있는 마이크로컨트롤러 플랫폼을 목표로 했습니다. 첫 보드는 Arduino Serial(DB9 커넥터)이었으며, 이후 Arduino Uno(2010)가 가장 널리 사용되는 표준 보드가 되었습니다. 현재는 공식 보드 외에도 수백 가지 호환 보드(ESP32, STM32 등)가 존재합니다.

## 특징

- **간편한 개발 환경**: Arduino IDE 하나로 코드 작성, 컴파일, 업로드까지 가능
- **하드웨어 추상화**: 복잡한 레지스터 설정 없이 `digitalWrite()`, `analogRead()` 등 간단한 함수로 제어
- **풍부한 라이브러리**: 센서, 모터, 디스플레이 등 수천 개의 라이브러리 지원
- **크로스 플랫폼**: Windows, macOS, Linux 모두 지원
- **오픈소스 하드웨어**: 회로도와 PCB 설계가 공개되어 있어 누구나 제작 가능
- **광범위한 생태계**: Shields(확장 보드), 다양한 센서/모듈 호환

## 실행 방법

```bash
# 각 .ino 파일을 Arduino IDE에서 열어서 업로드
# 또는 arduino-cli로 업로드
arduino-cli compile --fqbn arduino:avr:uno 01-introduction/01-introduction.ino
arduino-cli upload --fqbn arduino:avr:uno -p COM3 01-introduction/01-introduction.ino
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Introduction | Arduino IDE, setup/loop, LED blink |
| 02 | Digital I/O | digitalRead, 버튼 입력, 풀업 저항 |
| 03 | Analog Input | analogRead, 가변저항, 전압 분배 |
| 04 | PWM | analogWrite, LED 페이드, PWM 핀 |
| 05 | Serial | Serial.begin, Serial.print/read |
| 06 | Conditional | if/else, 버튼 상태, 디바운싱 |
| 07 | Loops | for/while, LED 패턴, Knight Rider |
| 08 | Functions | 함수 정의, 파라미터, 리턴값 |
| 09 | Arrays | 핀 배열, LED 시퀀스, 데이터 저장 |
| 10 | LCD Display | I2C LCD, 문자 출력 |
| 11 | Servo | Servo 라이브러리, sweep, 위치 제어 |
| 12 | Ultrasonic | HC-SR04 초음파 센서, 거리 측정 |
| 13 | DHT Sensor | DHT11 온습도 센서 |
| 14 | IR Remote | IR 리모컨 수신, 코드 처리 |
| 15 | DC Motor | L298N 모터 드라이버, 속도/방향 제어 |
| 16 | Interrupts | attachInterrupt, 버튼 인터럽트 |
| 17 | EEPROM | EEPROM 읽기/쓰기, 데이터 영속성 |
| 18 | Timers | millis(), micros(), Blink Without Delay |
| 19 | I2C | Wire 라이브러리, Master/Slave |
| 20 | IoT (ESP) | ESP8266/ESP32 WiFi, HTTP 요청 |
| 21 | 고급 센서 | BMP280 기압, MPU6050 가속도/자이로 |
| 22 | SPI | SPI 핀, 시프트 레지스터, SD 카드 |
| 23 | SD 카드 | 파일 로깅, CSV 저장, 타임스탬프 |
| 24 | 블루투스 | HC-05/HM-10 AT 명령, 데이터 송수신 |
| 25 | WiFi | ESP8266/ESP32 HTTP 클라이언트/서버 |
| 26 | MQTT | pub/sub, 브로커 연결, 토픽 관리 |
| 27 | 실시간 시계 | DS3231, 시간 동기화, 알람 |
| 28 | 스테퍼 모터 | A4988 드라이버, 스텝 제어 |
| 29 | 오디오 | DFPlayer, 멜로디 생성, 볼륨 제어 |
| 30 | OLED/TFT | I2C OLED, 그래픽, 메뉴 UI |
| 31 | GPS | NMEA 파싱, 위치/속도/시간 |
| 32 | 전원 관리 | 딥 슬립, 인터럽트 웨이크, 배터리 |
| 33 | PID | 온도/모터 PID 루프 |
| 34 | 상태 머신 | enum 기반 FSM, 시나리오 설계 |
| 35 | 멀티태스킹 | millis 스케줄러, 비블로킹 패턴 |
| 36 | 데이터 수집 | 시리얼 플로터, CSV, 샘플링 |
| 37 | 보안 | XOR 암호화, 키 저장, 인증 토큰 |
| 38 | ESP32 심화 | 웹 서버, OTA, FreeRTOS 개념 |
| 39 | IoT 클라우드 | HTTP POST, 대시보드 연동 개념 |
| 40 | 종합 프로젝트 | 기상 관측소 / 로봇 제어 |
