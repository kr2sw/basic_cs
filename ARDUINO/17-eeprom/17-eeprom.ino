#include <EEPROM.h>

struct Config {
  int brightness;
  int mode;
  char name[16];
};

void setup() {
  Serial.begin(9600);

  Config myConfig;
  EEPROM.get(0, myConfig);

  if (myConfig.brightness < 0 || myConfig.brightness > 255) {
    Serial.println("First run - initializing EEPROM");
    myConfig.brightness = 128;
    myConfig.mode = 1;
    strcpy(myConfig.name, "Arduino");
    EEPROM.put(0, myConfig);
  }

  Serial.print("Brightness: ");
  Serial.println(myConfig.brightness);
  Serial.print("Mode: ");
  Serial.println(myConfig.mode);
  Serial.print("Name: ");
  Serial.println(myConfig.name);

  myConfig.brightness = 200;
  myConfig.mode = 2;
  EEPROM.put(0, myConfig);
  Serial.println("Updated and saved!");
}

void loop() {
}
