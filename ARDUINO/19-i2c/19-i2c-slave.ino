#include <Wire.h>
#include <LiquidCrystal_I2C.h>

LiquidCrystal_I2C lcd(0x27, 16, 2);
int receivedValue = 0;
int ledState = LOW;

void setup() {
  pinMode(LED_BUILTIN, OUTPUT);
  lcd.init();
  lcd.backlight();
  lcd.print("I2C Slave ready");

  Wire.begin(8);
  Wire.onReceive(handleReceive);
  Wire.onRequest(handleRequest);
}

void loop() {
  digitalWrite(LED_BUILTIN, ledState);

  lcd.setCursor(0, 1);
  lcd.print("Value: ");
  lcd.print(receivedValue);
}

void handleReceive(int bytes) {
  while (Wire.available()) {
    char c = Wire.read();
    if (c >= '0' && c <= '9') {
      receivedValue = receivedValue * 10 + (c - '0');
    }
  }
  ledState = !ledState;
}

void handleRequest() {
  Wire.write('K');
}
