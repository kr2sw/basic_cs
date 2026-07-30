# Arduino 기초 (20개 챕터)

Arduino 마이크로컨트롤러 프로그래밍의 기초부터 고급 개념까지 학습할 수 있는 예제 모음입니다.

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
