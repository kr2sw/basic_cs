const int PINS[] = {3, 5, 6, 9, 10, 11};
const int PIN_COUNT = 6;

void setup() {
  for (int i = 0; i < PIN_COUNT; i++) {
    pinMode(PINS[i], OUTPUT);
  }
}

void loop() {
  for (int i = 0; i < PIN_COUNT; i++) {
    digitalWrite(PINS[i], HIGH);
    delay(100);
    digitalWrite(PINS[i], LOW);
  }

  for (int i = PIN_COUNT - 2; i >= 1; i--) {
    digitalWrite(PINS[i], HIGH);
    delay(100);
    digitalWrite(PINS[i], LOW);
  }
}
