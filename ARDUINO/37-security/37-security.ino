// XOR 암호화 + EEPROM 키 저장 + 인증 토큰 예제
#include <EEPROM.h>

const int LED_PIN = 13;

const int KEY_LEN_ADDR = 0;
const int KEY_DATA_ADDR = 1;
const int MAX_KEY_LEN = 32;

const char* DEFAULT_KEY = "arduino-secret";
const char* EXPECTED_TOKEN = "token123";

String encryptionKey = "";  // 로드된 키

// 문자열을 XOR 암호화하여 16진수 문자열로 반환
String xorEncrypt(String plain) {
  String result = "";
  int keyLen = encryptionKey.length();
  if (keyLen == 0) return "";

  for (int i = 0; i < plain.length(); i++) {
    byte enc = (byte)plain[i] ^ (byte)encryptionKey[i % keyLen];
    if (enc < 16) result += "0";
    result += String(enc, HEX);
  }
  return result;
}

// 16진수 문자열을 원문으로 복호화
String xorDecrypt(String hex) {
  String result = "";
  int keyLen = encryptionKey.length();
  if (keyLen == 0) return "";

  for (int i = 0; i + 1 < hex.length(); i += 2) {
    String pair = hex.substring(i, i + 2);
    byte enc = (byte)strtol(pair.c_str(), NULL, 16);
    char dec = (char)(enc ^ (byte)encryptionKey[(i / 2) % keyLen]);
    result += dec;
  }
  return result;
}

void saveKey(String key) {
  EEPROM.write(KEY_LEN_ADDR, key.length());
  for (int i = 0; i < key.length(); i++) {
    EEPROM.write(KEY_DATA_ADDR + i, key.charAt(i));
  }
#if defined(ESP8266) || defined(ESP32)
  EEPROM.commit();  // ESP 계열은 commit으로 플래시에 반영
#endif
  Serial.print("키 저장 완료: ");
  Serial.println(key);
}

String loadKey() {
  int len = EEPROM.read(KEY_LEN_ADDR);
  if (len <= 0 || len > MAX_KEY_LEN) return DEFAULT_KEY;  // 초기 상태
  String key = "";
  for (int i = 0; i < len; i++) {
    key += (char)EEPROM.read(KEY_DATA_ADDR + i);
  }
  return key;
}

void setup() {
  Serial.begin(9600);
  pinMode(LED_PIN, OUTPUT);

  // 저장된 키 로드 (없으면 기본 키)
  String saved = loadKey();
  if (saved.length() == 0) {
    saveKey(DEFAULT_KEY);
    encryptionKey = DEFAULT_KEY;
  } else {
    encryptionKey = saved;
    Serial.print("키 로드: ");
    Serial.println(encryptionKey);
  }

  Serial.println("보안 예제 준비 완료");
  Serial.println("명령: encrypt <문장> | auth <토큰> | setkey <새 키>");
}

void loop() {
  if (Serial.available()) {
    String cmd = Serial.readStringUntil('\n');
    cmd.trim();
    if (cmd.length() == 0) return;

    if (cmd.startsWith("encrypt ")) {
      String plain = cmd.substring(8);
      String cipher = xorEncrypt(plain);
      String decrypted = xorDecrypt(cipher);
      Serial.print("원문  : ");
      Serial.println(plain);
      Serial.print("암호화: ");
      Serial.println(cipher);
      Serial.print("복호화: ");
      Serial.println(decrypted);
    } else if (cmd.startsWith("auth ")) {
      String token = cmd.substring(5);
      token.trim();
      if (token == EXPECTED_TOKEN) {
        Serial.println("인증 성공! LED ON");
        digitalWrite(LED_PIN, HIGH);
        delay(3000);
        digitalWrite(LED_PIN, LOW);
      } else {
        Serial.println("인증 실패! 토큰 불일치");
      }
    } else if (cmd.startsWith("setkey ")) {
      String newKey = cmd.substring(7);
      if (newKey.length() > 0 && newKey.length() <= MAX_KEY_LEN) {
        saveKey(newKey);
        encryptionKey = newKey;
      } else {
        Serial.println("키 길이는 1~32자");
      }
    } else {
      Serial.println("알 수 없는 명령");
    }
  }
}
