#include <Wire.h>

const int SLAVE_ADDR = 8;
int counter = 0;

void setup() {
  Wire.begin();
  Serial.begin(9600);
  Serial.println("I2C Master ready");
}

void loop() {
  Wire.beginTransmission(SLAVE_ADDR);
  Wire.write("Count: ");
  Wire.write(counter);
  Wire.endTransmission();

  Wire.requestFrom(SLAVE_ADDR, 1);
  while (Wire.available()) {
    char response = Wire.read();
    Serial.print("Slave response: ");
    Serial.println(response);
  }

  counter++;
  delay(1000);
}
