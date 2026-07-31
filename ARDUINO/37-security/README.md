# 37: 보안 — Security

XOR 암호화, 키 저장(EEPROM), 인증 토큰으로 시리얼 통신 데이터를 보호하는 기초 보안을 다룹니다.

## 학습 내용
- XOR 암호화/복호화 원리
- 키를 EEPROM에 저장 (비휘발성)
- 메시지 암호화 후 전송
- 인증 토큰 검증 (간단한 핸드셰이크)

## XOR 암호화

XOR은 비트가 다르면 1이 되는 연산입니다. 같은 키로 두 번 XOR하면 원문이 되므로, 암호화와 복호화가 동일한 함수로 가능합니다.

```
원문 0xA5 XOR 키 0x3C = 0x99 (암호문)
암호문 0x99 XOR 키 0x3C = 0xA5 (원문 복원)
```

```cpp
byte xorByte(byte data, byte key) {
  return data ^ key;
}
```

## 문자열 XOR과 16진수 인코딩

문자열을 키로 XOR하면 0~255 모든 바이트가 나올 수 있어 시리얼로 보내기 어렵습니다. 그래서 결과를 16진수 문자열로 바꿔 전송합니다.

```cpp
String toHex(byte* data, int len) {
  String s = "";
  for (int i = 0; i < len; i++) {
    if (data[i] < 16) s += "0";
    s += String(data[i], HEX);
  }
  return s;
}
```

## 키 저장

키는 매번 입력하기 번거로우므로 EEPROM에 한 번 저장해두면 재부팅 후에도 사용할 수 있습니다. (17장에서 배운 기법)

```cpp
const char* DEFAULT_KEY = "arduino-secret";
EEPROM.write(0, strlen(DEFAULT_KEY));       // 길이 저장
for (int i = 0; i < strlen(DEFAULT_KEY); i++) {
  EEPROM.write(1 + i, DEFAULT_KEY[i]);      // 키 저장
}
EEPROM.commit();  // ESP 계열은 commit 필요
```

## 인증 토큰

장치 간 통신에서 상대가 아는 토큰(비밀 값)을 비교해 인증합니다. 토큰을 직접 보내기보다 암호화해서 보내는 것이 안전합니다.

```cpp
String received = "token123";   // 수신한 토큰
String expected = "token123";   // 저장된 기대값
if (received == expected) authenticated = true;
```

## 회로 연결

| 부품 | Arduino Uno |
|------|-------------|
| 버튼 | D2, 반대쪽 GND (토큰 인증 실행) |
| LED | D13 (인증 성공 시 점등) |

## 실행 방법

1. 이 챕터의 `.ino`를 업로드합니다.
2. 시리얼 모니터(9600)에 `encrypt hello`를 입력하면 XOR 암호화된 16진수와 복호화 결과가 출력됩니다.
3. `auth token123`을 입력하면 저장된 토큰과 비교해 인증 성공/실패가 출력되고 LED가 켜집니다.
4. `setkey newkey`로 EEPROM에 저장된 키를 변경할 수 있습니다.

> 이 예제는 학습용입니다. 실제 통신에서는 AES 같은 검증된 알고리즘과 TLS를 사용해야 합니다.

## 응용 아이디어

- 시리얼/블루투스 명령 데이터 난독화
- MQTT(26장) 메시지 암호화
- 간단한 장치 등록(페어링) 절차
