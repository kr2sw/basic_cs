const int LED1_PIN = 13;
const int LED2_PIN = 12;

unsigned long prevMillis1 = 0;
unsigned long prevMillis2 = 0;
const long INTERVAL1 = 500;
const long INTERVAL2 = 150;

bool led1State = false;
bool led2State = false;

void setup() {
  pinMode(LED1_PIN, OUTPUT);
  pinMode(LED2_PIN, OUTPUT);
  Serial.begin(9600);
}

void loop() {
  unsigned long currentMillis = millis();

  if (currentMillis - prevMillis1 >= INTERVAL1) {
    prevMillis1 = currentMillis;
    led1State = !led1State;
    digitalWrite(LED1_PIN, led1State);
    Serial.print("LED1: ");
    Serial.println(led1State ? "ON" : "OFF");
  }

  if (currentMillis - prevMillis2 >= INTERVAL2) {
    prevMillis2 = currentMillis;
    led2State = !led2State;
    digitalWrite(LED2_PIN, led2State);
  }
}
