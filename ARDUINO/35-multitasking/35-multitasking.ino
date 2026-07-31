// millis() 기반 멀티태스킹 스케줄러 예제
struct Task {
  void (*func)();        // 실행할 작업 함수
  unsigned long interval; // 실행 주기 (ms)
  unsigned long lastRun;  // 마지막 실행 시각
};

const int LED_PIN = 13;
const int SENSOR_PIN = A0;
const int BUTTON_PIN = 2;

int sensorValue = 0;
int buttonCount = 0;

// 작업 함수들
void blink() {
  digitalWrite(LED_PIN, !digitalRead(LED_PIN));
}

void readSensor() {
  sensorValue = analogRead(SENSOR_PIN);
  // 버튼도 함께 확인 (즉시 응답 가능함을 보여주기)
  if (digitalRead(BUTTON_PIN) == LOW) {
    buttonCount++;
  }
}

void printStatus() {
  Serial.print("[");
  Serial.print(millis());
  Serial.print("ms] LED=");
  Serial.print(digitalRead(LED_PIN) ? "ON" : "OFF");
  Serial.print(", 센서=");
  Serial.print(sensorValue);
  Serial.print(", 버튼횟수=");
  Serial.println(buttonCount);
}

// 스케줄러 테이블: 각 작업과 주기
Task tasks[] = {
  { blink,       500, 0 },   // 0.5초마다 LED 점멸
  { readSensor,  2000, 0 },  // 2초마다 센서 갱신
  { printStatus, 5000, 0 },  // 5초마다 상태 출력
};
const int TASK_COUNT = sizeof(tasks) / sizeof(tasks[0]);

void setup() {
  pinMode(LED_PIN, OUTPUT);
  pinMode(BUTTON_PIN, INPUT_PULLUP);
  Serial.begin(9600);
  Serial.println("멀티태스킹 시작 - 각 작업이 다른 주기로 실행됩니다");
}

void loop() {
  unsigned long now = millis();

  // 모든 작업을 순회하며 주기가 된 것만 실행
  for (int i = 0; i < TASK_COUNT; i++) {
    if (now - tasks[i].lastRun >= tasks[i].interval) {
      tasks[i].lastRun = now;
      tasks[i].func();  // 해당 작업 실행
    }
  }

  // 버튼 응답은 루프에서 매번 확인하므로 지연 없음
  // (작업 함수에도 포함했지만 즉각 반응 예시)
}
