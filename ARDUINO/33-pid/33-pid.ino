// PID 제어 예제 - 시뮬레이션 플랜트 제어
const int PWM_PIN = 9;   // 제어 출력 표시 (LED 밝기 = 히터 출력)

// PID 이득
float Kp = 2.0;
float Ki = 0.5;
float Kd = 0.1;

// 제어 상태 변수
double setpoint = 30.0;   // 목표 온도
double measured = 25.0;   // 현재 온도 (시뮬레이션)
double integral = 0.0;
double lastError = 0.0;
unsigned long lastTime = 0;

// PID 계산 후 출력 반환
double computePID(float dt) {
  double error = setpoint - measured;

  integral += error * dt;
  integral = constrain(integral, -100, 100);  // 안티와인드업

  double derivative = (error - lastError) / dt;
  lastError = error;

  double output = Kp * error + Ki * integral + Kd * derivative;
  output = constrain(output, 0, 255);  // PWM 0~255

  return output;
}

void setup() {
  pinMode(PWM_PIN, OUTPUT);
  Serial.begin(9600);
  lastTime = millis();

  Serial.println("PID 제어 시작");
  Serial.println("명령: s 35 | kp 3 | ki 0.3 | kd 0.05 | reset");
}

void loop() {
  // 샘플링 시간 계산
  unsigned long now = millis();
  float dt = (now - lastTime) / 1000.0;
  if (dt <= 0) return;
  lastTime = now;

  // PID 출력 계산
  double output = computePID(dt);

  // 플랜트 시뮬레이션: 히터에 의한 상승 + 자연 냉각
  double heating = output / 255.0;
  measured += (heating * 5.0 - (measured - 20.0) * 0.02) * dt;

  // LED 밝기로 제어 출력 표시
  analogWrite(PWM_PIN, output);

  // 시리얼 플로터 출력 (set / meas / out)
  Serial.print("set:");
  Serial.print(setpoint);
  Serial.print(" meas:");
  Serial.print(measured, 2);
  Serial.print(" out:");
  Serial.println(output, 0);

  // 시리얼 명령 처리
  if (Serial.available()) {
    String cmd = Serial.readStringUntil('\n');
    cmd.trim();
    if (cmd.startsWith("s ")) {
      setpoint = cmd.substring(2).toFloat();
      Serial.print("목표값 변경: ");
      Serial.println(setpoint);
    } else if (cmd.startsWith("kp ")) {
      Kp = cmd.substring(3).toFloat();
      Serial.print("Kp: ");
      Serial.println(Kp);
    } else if (cmd.startsWith("ki ")) {
      Ki = cmd.substring(3).toFloat();
      Serial.print("Ki: ");
      Serial.println(Ki);
    } else if (cmd.startsWith("kd ")) {
      Kd = cmd.substring(3).toFloat();
      Serial.print("Kd: ");
      Serial.println(Kd);
    } else if (cmd == "reset") {
      integral = 0;
      lastError = 0;
      Serial.println("적분/미분 초기화");
    }
  }

  delay(50);  // 20Hz 제어 주기
}
