const int LED_PINS[] = {3, 5, 6, 9, 10, 11};
const int LED_COUNT = 6;

int pattern[] = {1, 0, 1, 0, 1, 0};
int brightness[] = {0, 50, 100, 150, 200, 255};

void setup() {
  for (int i = 0; i < LED_COUNT; i++) {
    pinMode(LED_PINS[i], OUTPUT);
  }
}

void loop() {
  for (int i = 0; i < LED_COUNT; i++) {
    digitalWrite(LED_PINS[i], pattern[i]);
  }
  delay(500);

  for (int i = 0; i < LED_COUNT; i++) {
    pattern[i] = !pattern[i];
  }
  delay(500);

  for (int i = 0; i < LED_COUNT; i++) {
    analogWrite(LED_PINS[i], brightness[i]);
  }
  delay(2000);

  for (int i = 0; i < LED_COUNT; i++) {
    analogWrite(LED_PINS[i], 0);
  }
  delay(500);
}
