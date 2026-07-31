// ESP32 전원 관리 예제 (딥 슬립 + 인터럽트/타이머 웨이크)
#if defined(ESP32)
#include <esp_sleep.h>
#else
#error "이 챕터(32)는 ESP32 보드 전용 예제입니다."
#endif

// 웨이크 버튼 핀 (RTC GPIO 권장 핀 중 선택)
const int WAKEUP_PIN = 4;      // GPIO4: LOW로 눌리면 웨이크
const int BATTERY_PIN = 34;    // ADC1 채널 (전압 분배기 입력)
const int LED_PIN = 2;         // 내장 LED

// 배터리 분배기 비율 (상/하 저항이 같으면 2.0)
const float DIVIDER_RATIO = 2.0;
const float ADC_MAX_VOLT = 3.3;

const int SLEEP_SECONDS = 10;  // 딥 슬립 시간

void printWakeupReason() {
  esp_sleep_wakeup_cause_t cause = esp_sleep_get_wakeup_cause();
  switch (cause) {
    case ESP_SLEEP_WAKEUP_TIMER:
      Serial.println("웨이크 원인: 타이머 (10초 경과)");
      break;
    case ESP_SLEEP_WAKEUP_EXT0:
      Serial.println("웨이크 원인: 외부 인터럽트 (버튼)");
      break;
    default:
      Serial.println("웨이크 원인: 콜드 부팅");
      break;
  }
}

float readBatteryVoltage() {
  int raw = analogRead(BATTERY_PIN);
  float voltage = (raw / 4095.0) * ADC_MAX_VOLT * DIVIDER_RATIO;
  return voltage;
}

void setup() {
  Serial.begin(115200);
  delay(500);  // 시리얼 안정화

  pinMode(LED_PIN, OUTPUT);
  digitalWrite(LED_PIN, LOW);

  printWakeupReason();

  // 배터리 전압 측정 및 출력
  float bat = readBatteryVoltage();
  Serial.print("배터리 전압: ");
  Serial.print(bat);
  Serial.println(" V");

  // 동작: 1초간 LED 점멸으로 깨어났음을 표시
  digitalWrite(LED_PIN, HIGH);
  delay(300);
  digitalWrite(LED_PIN, LOW);

  // 딥 슬립 준비 (10초 타이머 웨이크)
  Serial.print(SLEEP_SECONDS);
  Serial.println("초 후 딥 슬립에 들어갑니다.");
  Serial.println("버튼(GPIO4)을 누르면 즉시 깨어납니다.");
  delay(1000);

  // 웨이크 조건 등록
  esp_sleep_enable_timer_wakeup(SLEEP_SECONDS * 1000000ULL);
  esp_sleep_enable_ext0_wakeup(GPIO_NUM_4, LOW);

  // 딥 슬립 시작
  esp_deep_sleep_start();
}

void loop() {
  // 딥 슬립에 들어가면 여기에 도달하지 않음
}
