// 종합 프로젝트: 기상 관측소
// 온습도(DHT) + 기압(BMP280) + RTC + SD 로깅 + OLED
#include <DHT.h>
#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BMP280.h>
#include "RTClib.h"
#include <SPI.h>
#include <SD.h>
#include <Adafruit_GFX.h>
#include <Adafruit_SSD1306.h>

// --- 핀 설정 ---
const int DHT_PIN = 2;
const int SD_CS = 10;

// --- 센서 객체 ---
DHT dht(DHT_PIN, DHT22);          // DHT22 (DHT11이면 DHT11로 변경)
Adafruit_BMP280 bmp;
RTC_DS3231 rtc;
Adafruit_SSD1306 display(128, 64, &Wire, -1);

// --- 스케줄러 태스크 구조체 ---
struct Task {
  void (*func)();
  unsigned long interval;
  unsigned long lastRun;
};

// --- 전역 상태 ---
float temp = 0, hum = 0, press = 0;
bool sensorsOk = false;
bool sdOk = false;
bool rtcOk = false;
bool oledOk = false;

// RTC 타임스탬프 문자열 만들기
String timestamp() {
  DateTime now = rtc.now();
  char buf[20];
  snprintf(buf, sizeof(buf), "%04d-%02d-%02d %02d:%02d:%02d",
           now.year(), now.month(), now.day(),
           now.hour(), now.minute(), now.second());
  return String(buf);
}

// --- 태스크 1: 센서 샘플링 (2초) ---
void sampleSensors() {
  temp = dht.readTemperature();
  hum = dht.readHumidity();
  press = bmp.readPressure() / 100.0F;

  // DHT 읽기 실패 처리
  if (isnan(temp) || isnan(hum)) {
    Serial.println("DHT 읽기 실패");
    return;
  }
}

// --- 태스크 2: OLED 갱신 (1초) ---
void updateDisplay() {
  if (!oledOk) return;

  display.clearDisplay();
  display.setTextSize(1);
  display.setTextColor(SSD1306_WHITE);
  display.setCursor(0, 0);
  display.println("== Weather Station ==");
  display.setCursor(0, 14);
  display.print("Temp: ");
  display.print(temp);
  display.println(" C");
  display.setCursor(0, 26);
  display.print("Hum : ");
  display.print(hum);
  display.println(" %");
  display.setCursor(0, 38);
  display.print("Pres: ");
  display.print(press);
  display.println(" hPa");
  display.setCursor(0, 52);
  if (rtcOk) display.print(timestamp().substring(5));
  else display.print("RTC 없음");
  display.display();
}

// --- 태스크 3: SD 로그 저장 (30초) ---
void logToSD() {
  if (!sdOk) return;

  File dataFile = SD.open("weather.csv", FILE_WRITE);
  if (dataFile) {
    dataFile.print(timestamp());
    dataFile.print(",");
    dataFile.print(temp);
    dataFile.print(",");
    dataFile.print(hum);
    dataFile.print(",");
    dataFile.println(press);
    dataFile.close();
    Serial.print("로그 저장: ");
    Serial.println(timestamp());
  } else {
    Serial.println("SD 파일 열기 실패");
  }
}

// --- 스케줄러 테이블 ---
Task tasks[] = {
  { sampleSensors, 2000, 0 },
  { updateDisplay, 1000, 0 },
  { logToSD,      30000, 0 },
};
const int TASK_COUNT = sizeof(tasks) / sizeof(tasks[0]);

void setup() {
  Serial.begin(9600);
  while (!Serial) { delay(10); }

  Serial.println("== 기상 관측소 시작 ==");

  // 센서 초기화
  dht.begin();
  sensorsOk = bmp.begin(0x76);
  Serial.println(sensorsOk ? "BMP280 OK" : "BMP280 실패");

  rtcOk = rtc.begin();
  if (rtcOk && rtc.lostPower()) {
    rtc.adjust(DateTime(F(__DATE__), F(__TIME__)));  // 1회 동기화
    Serial.println("RTC 시각 동기화");
  }
  Serial.println(rtcOk ? "RTC OK" : "RTC 실패");

  sdOk = SD.begin(SD_CS);
  if (sdOk) {
    if (!SD.exists("weather.csv")) {
      File f = SD.open("weather.csv", FILE_WRITE);
      if (f) {
        f.println("time,temp,hum,press");  // CSV 헤더
        f.close();
      }
    }
    Serial.println("SD OK");
  } else {
    Serial.println("SD 실패 (FAT32 확인)");
  }

  oledOk = display.begin(SSD1306_SWITCHCAPVCC, 0x3C);
  if (oledOk) {
    display.clearDisplay();
    display.display();
    Serial.println("OLED OK");
  }

  Serial.println("작동 시작 (2초 샘플링 / 1초 표시 / 30초 로깅)");
}

void loop() {
  unsigned long now = millis();
  for (int i = 0; i < TASK_COUNT; i++) {
    if (now - tasks[i].lastRun >= tasks[i].interval) {
      tasks[i].lastRun = now;
      tasks[i].func();
    }
  }
}
