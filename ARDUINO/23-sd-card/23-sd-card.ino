#include <SD.h>
#include <SPI.h>

const int CHIP_SELECT = 10;  // SD 카드 CS 핀
const char* LOG_FILE = "log.csv";

void setup() {
  Serial.begin(9600);
  while (!Serial) { delay(10); }

  Serial.print("SD 카드 초기화... ");
  if (!SD.begin(CHIP_SELECT)) {
    Serial.println("실패! 카드 연결/포맷(FAT32)을 확인하세요.");
    return;
  }
  Serial.println("성공");

  // 파일 정보 출력
  File root = SD.open("/");
  Serial.println("SD 카드 내용:");
  while (true) {
    File entry = root.openNextFile();
    if (!entry) break;
    Serial.print("  ");
    Serial.print(entry.name());
    Serial.print("  ");
    Serial.println(entry.size());
    entry.close();
  }

  // 로그 파일이 없으면 CSV 헤더 작성
  if (!SD.exists(LOG_FILE)) {
    File dataFile = SD.open(LOG_FILE, FILE_WRITE);
    if (dataFile) {
      dataFile.println("time_ms,value");
      dataFile.close();
      Serial.println("CSV 헤더 작성 완료");
    }
  }
}

void logData(unsigned long timeMs, int value) {
  File dataFile = SD.open(LOG_FILE, FILE_WRITE);
  if (dataFile) {
    dataFile.print(timeMs);
    dataFile.print(",");
    dataFile.println(value);
    dataFile.close();
    Serial.print("기록: ");
    Serial.print(timeMs);
    Serial.print(",");
    Serial.println(value);
  } else {
    Serial.println("파일 열기 실패!");
  }
}

void loop() {
  // A0 가변저항 값을 샘플링하여 로그에 남긴다
  int value = analogRead(A0);

  // 부팅 후 경과 시간(ms)을 타임스탬프로 사용
  unsigned long timeMs = millis();

  logData(timeMs, value);

  delay(2000);  // 2초 간격으로 기록
}
