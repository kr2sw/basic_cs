#include <SoftwareSerial.h>
#include <TinyGPSPlus.h>

// GPS 모듈 연결 (TX → 핀2, RX → 핀3)
SoftwareSerial gpsSerial(2, 3);
TinyGPSPlus gps;

unsigned long lastPrint = 0;

void printGPSInfo() {
  Serial.println("====================");

  // 위치 (위성 고정 시에만 유효)
  if (gps.location.isValid()) {
    Serial.print("위도: ");
    Serial.println(gps.location.lat(), 6);
    Serial.print("경도: ");
    Serial.println(gps.location.lng(), 6);
  } else {
    Serial.println("위치: 위성 신호 대기 중...");
  }

  // 고도, 속도
  if (gps.altitude.isValid()) {
    Serial.print("고도: ");
    Serial.print(gps.altitude.meters());
    Serial.println(" m");
  }
  if (gps.speed.isValid()) {
    Serial.print("속도: ");
    Serial.print(gps.speed.kmph());
    Serial.println(" km/h");
  }

  // UTC 시간 (한국은 +9시간)
  if (gps.time.isValid() && gps.date.isValid()) {
    char buf[32];
    snprintf(buf, sizeof(buf), "UTC %04d-%02d-%02d %02d:%02d:%02d",
             gps.date.year(), gps.date.month(), gps.date.day(),
             gps.time.hour(), gps.time.minute(), gps.time.second());
    Serial.println(buf);
  }

  Serial.print("추적 위성 수: ");
  Serial.println(gps.satellites.value());
}

void setup() {
  Serial.begin(115200);
  gpsSerial.begin(9600);  // 대부분의 GPS 모듈 기본 보드레이트

  Serial.println("GPS 수신 시작 (위성 고정 대기 중...)");
}

void loop() {
  // GPS 모듈에서 나오는 NMEA 문장을 한 글자씩 파싱
  while (gpsSerial.available() > 0) {
    char c = gpsSerial.read();
    if (gps.encode(c)) {
      // 문장 하나가 완성되면 최신 데이터가 반영됨
    }
  }

  // 2초마다 현재 상태 출력
  if (millis() - lastPrint > 2000) {
    lastPrint = millis();
    printGPSInfo();
  }
}
