const int POT_PIN = A0;
const int LED_PIN = 9;

void setup() {
  pinMode(LED_PIN, OUTPUT);
  Serial.begin(9600);
}

void loop() {
  int sensorValue = analogRead(POT_PIN);
  int brightness = map(sensorValue, 0, 1023, 0, 255);

  analogWrite(LED_PIN, brightness);

  Serial.print("Sensor: ");
  Serial.print(sensorValue);
  Serial.print(" -> Brightness: ");
  Serial.println(brightness);

  delay(50);
}
