const int LED_PIN = 13;
const int INTERRUPT_PIN = 2;

volatile bool ledState = false;
volatile int buttonCount = 0;

void handleInterrupt() {
  ledState = !ledState;
  buttonCount++;
}

void setup() {
  pinMode(LED_PIN, OUTPUT);
  pinMode(INTERRUPT_PIN, INPUT_PULLUP);
  Serial.begin(9600);

  attachInterrupt(digitalPinToInterrupt(INTERRUPT_PIN), handleInterrupt, FALLING);

  Serial.println("Interrupt ready - press button on pin 2");
}

void loop() {
  digitalWrite(LED_PIN, ledState);

  if (buttonCount > 0) {
    noInterrupts();
    int count = buttonCount;
    buttonCount = 0;
    interrupts();

    Serial.print("Button pressed: ");
    Serial.println(count);
  }
}
