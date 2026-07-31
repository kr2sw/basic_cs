# 23: SD 카드 — SD Card

SD 카드에 데이터를 파일로 저장합니다. 파일 로깅, CSV 형식 저장, 타임스탬프 기록을 다룹니다.

## 학습 내용
- SD.h 라이브러리와 SD.begin()
- 파일 생성, 쓰기, 닫기
- CSV(Comma-Separated Values) 형식으로 데이터 저장
- 타임스탬프 기록과 데이터 로거 만들기

## SD 카드와 SPI

SD 카드 모듈은 SPI 통신을 사용합니다. Uno의 11, 12, 13번 핀과 별도의 CS 핀(보통 10)을 사용합니다.

```cpp
#include <SD.h>
#include <SPI.h>
const int CHIP_SELECT = 10;

if (!SD.begin(CHIP_SELECT)) {
  Serial.println("SD 카드 초기화 실패");
}
```

## 파일 쓰기와 CSV

`File` 객체로 파일을 열고, `print()`/`println()`으로 데이터를 기록합니다. `FILE_WRITE` 모드는 파일 끝에 이어서 씁니다.

```cpp
File dataFile = SD.open("log.csv", FILE_WRITE);
if (dataFile) {
  dataFile.print(millis());
  dataFile.print(",");
  dataFile.println(analogRead(A0));  // 한 줄: 시간,값
  dataFile.close();                   // 반드시 닫아야 저장됨
}
```

CSV 형식은 엑셀이나 파이썬 pandas로 쉽게 열어 분석할 수 있습니다.

## 타임스탬프

`millis()`는 부팅 후 경과 시간이므로 파일에 절대 시각을 남기려면 RTC(27장)를 함께 사용합니다. 이 챕터에서는 부팅 시 시리얼로 입력받은 시작 시간에 경과 시간을 더해 타임스탬프를 만들 수 있습니다.

```cpp
unsigned long startTime = 1700000000;  // 예: 에폭시(Unix) 시작 시각
unsigned long now = startTime + millis() / 1000;
```

## 회로 연결 (SD 카드 모듈)

| SD 모듈 | Arduino Uno |
|---------|-------------|
| VCC | 5V |
| GND | GND |
| CS | 10 |
| MOSI | 11 |
| SCK | 13 |
| MISO | 12 |

> 가변저항(포텐셔미터)을 A0에 연결하면 조도/전압 값이 기록됩니다.

## 실행 방법

1. SD 카드를 **FAT32**로 포맷하고 모듈에 삽입합니다.
2. 이 챕터의 `.ino` 파일을 열고 **업로드**합니다.
3. 시리얼 모니터(9600)로 초기화 과정을 확인합니다.
4. 전원을 빼고 SD 카드를 PC에 연결하면 `log.csv` 파일이 생성되어 있습니다.

## 응용 아이디어

- 온습도 센서(DHT)와 결합한 환경 데이터 로거
- GPS(31장) 위치 기록기
- RTC(27장)와 결합해 실제 시각 타임스탬프 기록
