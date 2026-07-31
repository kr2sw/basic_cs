#include <SoftwareSerial.h>

// 피에조 부저 (멜로디)
const int SPEAKER_PIN = 9;

// DFPlayer Mini 연결 (SoftwareSerial)
SoftwareSerial dfp(3, 2);  // RX=3, TX=2

// 음계 주파수 (Hz)
const int NOTE_C4 = 262, NOTE_D4 = 294, NOTE_E4 = 330;
const int NOTE_F4 = 349, NOTE_G4 = 392, NOTE_A4 = 440;
const int NOTE_B4 = 494, NOTE_C5 = 523;

// "학교종" 멜로디: (음, 박자)
int melody[][2] = {
  {NOTE_G4, 1}, {NOTE_G4, 1}, {NOTE_A4, 2},
  {NOTE_A4, 2}, {NOTE_G4, 2},
  {NOTE_A4, 2}, {NOTE_A4, 2}, {NOTE_G4, 2},
  {NOTE_G4, 2}, {NOTE_G4, 2}, {NOTE_A4, 2},
  {NOTE_A4, 2}, {NOTE_G4, 2},
  {NOTE_E4, 3}, {NOTE_C4, 1}, {NOTE_C4, 3}, {NOTE_C4, 1}
};
const int MELODY_LEN = sizeof(melody) / sizeof(melody[0]);
const int BEAT_MS = 400;  // 한 박자 길이

// DFPlayer 명령 전송 (체크섬 포함)
void dfpCommand(byte cmd, int param) {
  byte hi = (param >> 8) & 0xFF;
  byte lo = param & 0xFF;
  int checksum = 0xFFFF - (0x06 + cmd + 0x00 + hi + lo);

  dfp.write(0x7E);
  dfp.write(0xFF);
  dfp.write(0x06);
  dfp.write(cmd);
  dfp.write(0x00);
  dfp.write(hi);
  dfp.write(lo);
  dfp.write(highByte(checksum));
  dfp.write(lowByte(checksum));
  dfp.write(0xEF);
}

void playMelody() {
  for (int i = 0; i < MELODY_LEN; i++) {
    int freq = melody[i][0];
    int beat = melody[i][1];
    if (freq > 0) {
      tone(SPEAKER_PIN, freq, beat * BEAT_MS);
    }
    delay(beat * BEAT_MS + 30);  // 음 사이 짧은 휴지
    noTone(SPEAKER_PIN);
  }
}

void setup() {
  Serial.begin(9600);
  dfp.begin(9600);

  // DFPlayer 볼륨 15로 설정
  dfpCommand(0x06, 15);
  delay(100);

  Serial.println("오디오 제어 준비 완료");
  Serial.println("명령: p=다음곡, s=정지, m=멜로디, v 20=볼륨설정");
}

void loop() {
  if (Serial.available()) {
    String cmd = Serial.readStringUntil('\n');
    cmd.trim();
    if (cmd.length() == 0) return;

    char c = cmd.charAt(0);
    switch (c) {
      case 'p':  // 다음 곡 재생
        dfpCommand(0x0D, 0);
        Serial.println("재생");
        break;
      case 's':  // 정지
        dfpCommand(0x0E, 0);
        Serial.println("정지");
        break;
      case 'm':  // 멜로디
        Serial.println("멜로디 재생");
        playMelody();
        break;
      case 'v':  // 볼륨 설정 (0~30)
        int vol = cmd.substring(1).toInt();
        vol = constrain(vol, 0, 30);
        dfpCommand(0x06, vol);
        Serial.print("볼륨: ");
        Serial.println(vol);
        break;
      default:
        Serial.println("알 수 없는 명령");
        break;
    }
  }
}
