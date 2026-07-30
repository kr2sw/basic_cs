#include <Wire.h>
#include <LiquidCrystal_I2C.h>

LiquidCrystal_I2C lcd(0x27, 16, 2);

int count = 0;

void setup() {
  lcd.init();
  lcd.backlight();
  lcd.print("Hello, Arduino!");
}

void loop() {
  lcd.setCursor(0, 1);
  lcd.print("Count: ");
  lcd.print(count);
  count++;
  delay(1000);
}
