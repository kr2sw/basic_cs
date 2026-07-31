// A4988 스테퍼 모터 드라이버 제어
const int STEP_PIN = 8;
const int DIR_PIN = 9;
const int ENABLE_PIN = 10;

// 1/16 마이크로스텝 + 1.8도 모터 기준
const int STEPS_PER_REV = 3200;

// 한 스텝 실행 (방향 true = 시계 방향)
void doStep(bool dir) {
  digitalWrite(DIR_PIN, dir);
  digitalWrite(STEP_PIN, HIGH);
  delayMicroseconds(800);   // 펄스 폭: 낮을수록 빠름
  digitalWrite(STEP_PIN, LOW);
  delayMicroseconds(800);
}

// 지정한 스텝 수만큼 회전 (음수는 반시계 방향)
void rotateSteps(long steps) {
  bool dir = (steps >= 0);
  long absSteps = steps < 0 ? -steps : steps;

  for (long i = 0; i < absSteps; i++) {
    doStep(dir);
  }
}

// 각도(도)로 회전
void rotateDegrees(float degrees) {
  long steps = (long)(STEPS_PER_REV * degrees / 360.0);
  rotateSteps(steps);
}

void setup() {
  pinMode(STEP_PIN, OUTPUT);
  pinMode(DIR_PIN, OUTPUT);
  pinMode(ENABLE_PIN, OUTPUT);
  digitalWrite(ENABLE_PIN, LOW);  // 드라이버 활성화

  Serial.begin(9600);
  Serial.println("스테퍼 모터 제어 준비 완료");
  Serial.println("명령 예: f 90 (정방향 90도), b 180 (역방향 180도)");
}

void loop() {
  if (Serial.available()) {
    // "f 90" 또는 "b 180" 형태의 명령 파싱
    String cmd = Serial.readStringUntil('\n');
    cmd.trim();
    if (cmd.length() == 0) return;

    char dirChar = cmd.charAt(0);
    String numStr = cmd.substring(1);
    numStr.trim();
    float degrees = numStr.toFloat();
    if (degrees <= 0) {
      Serial.println("잘못된 각도입니다.");
      return;
    }

    if (dirChar == 'f' || dirChar == 'F') {
      Serial.print("정방향 ");
      Serial.print(degrees);
      Serial.println("도 회전");
      rotateDegrees(degrees);
    } else if (dirChar == 'b' || dirChar == 'B') {
      Serial.print("역방향 ");
      Serial.print(degrees);
      Serial.println("도 회전");
      rotateDegrees(-degrees);
    } else {
      Serial.println("알 수 없는 명령 (f 또는 b 사용)");
    }
    Serial.println("완료");
  }
}
