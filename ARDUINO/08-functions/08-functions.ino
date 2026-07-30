const int LED_PIN = 13;

void blink(int pin, int times, int delayMs) {
  for (int i = 0; i < times; i++) {
    digitalWrite(pin, HIGH);
    delay(delayMs);
    digitalWrite(pin, LOW);
    if (i < times - 1) delay(delayMs);
  }
}

int mapToPwm(int value, int fromLow, int fromHigh, int toLow, int toHigh) {
  return (value - fromLow) * (toHigh - toLow) / (fromHigh - fromLow) + toLow;
}

void setup() {
  pinMode(LED_PIN, OUTPUT);
  Serial.begin(9600);
}

void loop() {
  blink(LED_PIN, 3, 200);
  delay(1000);
  blink(LED_PIN, 5, 100);
  delay(1000);

  for (int sensor = 0; sensor <= 1023; sensor += 256) {
    int pwm = mapToPwm(sensor, 0, 1023, 0, 255);
    Serial.print(sensor);
    Serial.print(" -> ");
    Serial.println(pwm);
  }

  delay(2000);
}
