const int ENA = 9;
const int IN1 = 7;
const int IN2 = 8;

void setup() {
  pinMode(ENA, OUTPUT);
  pinMode(IN1, OUTPUT);
  pinMode(IN2, OUTPUT);
  Serial.begin(9600);
  Serial.println("Motor control ready");
}

void motorForward(int speed) {
  digitalWrite(IN1, HIGH);
  digitalWrite(IN2, LOW);
  analogWrite(ENA, speed);
}

void motorBackward(int speed) {
  digitalWrite(IN1, LOW);
  digitalWrite(IN2, HIGH);
  analogWrite(ENA, speed);
}

void motorStop() {
  digitalWrite(IN1, LOW);
  digitalWrite(IN2, LOW);
  analogWrite(ENA, 0);
}

void loop() {
  Serial.println("Forward slow");
  motorForward(100);
  delay(2000);

  Serial.println("Forward fast");
  motorForward(255);
  delay(2000);

  Serial.println("Stop");
  motorStop();
  delay(1000);

  Serial.println("Backward");
  motorBackward(150);
  delay(2000);

  Serial.println("Stop");
  motorStop();
  delay(2000);
}
