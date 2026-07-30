#include <IRremote.h>

const int RECV_PIN = 11;
IRrecv irrecv(RECV_PIN);
decode_results results;

const int LED_PIN = 13;

void setup() {
  Serial.begin(9600);
  irrecv.enableIRIn();
  pinMode(LED_PIN, OUTPUT);
  Serial.println("IR Receiver ready");
}

void loop() {
  if (irrecv.decode(&results)) {
    Serial.print("IR Code: 0x");
    Serial.println(results.value, HEX);

    switch (results.value) {
      case 0xFF00BF00:  // CH- (예시 코드, 실제 리모컨에 맞게 수정)
        digitalWrite(LED_PIN, HIGH);
        Serial.println("LED ON");
        break;
      case 0xFF807F00:  // CH+ (예시)
        digitalWrite(LED_PIN, LOW);
        Serial.println("LED OFF");
        break;
    }

    irrecv.resume();
  }
}
