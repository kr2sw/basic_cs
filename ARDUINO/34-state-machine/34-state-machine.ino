// 상태 머신(FSM) 예제 - 신호등 + 보행자 버튼
enum State {
  RED,          // 빨강: 정지
  GREEN,        // 초록: 직진
  YELLOW,       // 노랑: 주의
  PEDESTRIAN    // 보행자 우선
};

State state = RED;

const int RED_PIN = 9;
const int YELLOW_PIN = 10;
const int GREEN_PIN = 11;
const int BUTTON_PIN = 2;  // 보행자 버튼 (INPUT_PULLUP)

// 상태별 지속 시간 (ms)
const unsigned long RED_TIME = 5000;
const unsigned long GREEN_TIME = 4000;
const unsigned long YELLOW_TIME = 2000;
const unsigned long PED_TIME = 3000;

unsigned long stateStart = 0;  // 현재 상태 진입 시각

// 상태 전이 함수: 상태 변경 + 진입 시각 기록
void enterState(State newState) {
  state = newState;
  stateStart = millis();

  switch (state) {
    case RED:
      digitalWrite(RED_PIN, HIGH);
      digitalWrite(YELLOW_PIN, LOW);
      digitalWrite(GREEN_PIN, LOW);
      Serial.println("상태: RED (정지)");
      break;
    case GREEN:
      digitalWrite(RED_PIN, LOW);
      digitalWrite(YELLOW_PIN, LOW);
      digitalWrite(GREEN_PIN, HIGH);
      Serial.println("상태: GREEN (직진)");
      break;
    case YELLOW:
      digitalWrite(RED_PIN, LOW);
      digitalWrite(YELLOW_PIN, HIGH);
      digitalWrite(GREEN_PIN, LOW);
      Serial.println("상태: YELLOW (주의)");
      break;
    case PEDESTRIAN:
      // 보행자 우선: 빨간불 + 초록불 점멸
      digitalWrite(RED_PIN, HIGH);
      digitalWrite(YELLOW_PIN, LOW);
      digitalWrite(GREEN_PIN, LOW);
      Serial.println("상태: PEDESTRIAN (보행자 통행)");
      break;
  }
}

void setup() {
  pinMode(RED_PIN, OUTPUT);
  pinMode(YELLOW_PIN, OUTPUT);
  pinMode(GREEN_PIN, OUTPUT);
  pinMode(BUTTON_PIN, INPUT_PULLUP);

  Serial.begin(9600);
  enterState(RED);  // 시작 상태
}

void loop() {
  unsigned long elapsed = millis() - stateStart;

  // 보행자 버튼 감지: 초록 상태에서 누르면 보행자 상태로 전이
  static bool buttonPressed = false;
  if (digitalRead(BUTTON_PIN) == LOW && state == GREEN) {
    buttonPressed = true;
  }

  // 상태 전이 판단
  switch (state) {
    case RED:
      if (elapsed >= RED_TIME) enterState(GREEN);
      break;
    case GREEN:
      if (buttonPressed) {
        buttonPressed = false;   // 요청 소비
        enterState(PEDESTRIAN);
      } else if (elapsed >= GREEN_TIME) {
        enterState(YELLOW);
      }
      break;
    case YELLOW:
      if (elapsed >= YELLOW_TIME) enterState(RED);
      break;
    case PEDESTRIAN:
      if (elapsed >= PED_TIME) enterState(RED);
      break;
  }
}
