# 29: 오디오 — Audio

피에조 부저로 멜로디를 만들고, DFPlayer MP3 모듈로 사운드 재생과 볼륨을 제어합니다.

## 학습 내용
- `tone()`으로 주파수 생성
- 멜로디와 리듬 표현
- DFPlayer Mini MP3 모듈 제어 (UART 명령)
- 볼륨 제어와 곡 재생

## tone()과 멜로디

`tone(pin, frequency, duration)`은 핀에서 지정 주파수를 출력합니다. 노트를 주파수 배열로, 리듬을 박자 배열로 표현하면 멜로디가 됩니다.

```cpp
const int NOTE_C4 = 262;
tone(SPEAKER_PIN, NOTE_C4, 400);
delay(400);
noTone(SPEAKER_PIN);  // 소리 정지
```

## DFPlayer Mini

DFPlayer는 SD 카드에 넣은 MP3를 재생하는 모듈입니다. UART(시리얼)로 명령을 보냅니다. 명령 프레임은 시작 바이트, 명령, 파라미터, 체크섬으로 구성됩니다.

```
프레임: 0x7E FF [명령] [피드백] [파라미터 상/하] [체크섬] 0xEF
체크섬 = 0xFFFF - (명령+피드백+파라미터 상+파라미터 하)
```

주요 명령: `0x06` 볼륨, `0x0D` 재생, `0x0E` 정지, `0x0F` 폴더/파일 재생, `0x16` 다음 곡.

## 회로 연결

### 피에조 부저
| 부저 | Arduino Uno |
|------|-------------|
| + | D9 |
| - | GND |

### DFPlayer Mini
| DFPlayer | Arduino Uno |
|----------|-------------|
| VCC | 5V |
| GND | GND |
| RX | D2 (SoftwareSerial TX) |
| TX | D3 (SoftwareSerial RX) |
| SPK+ / SPK- | 스피커 |

> SD 카드에 mp3 파일을 `01.mp3`, `02.mp3`... 이름으로 넣고 FAT32로 포맷합니다. BUSY 핀(재생 중 표시)은 선택 사항입니다.

## 실행 방법

1. 이 챕터의 `.ino`를 업로드합니다.
2. 시리얼 모니터(9600)에서 명령을 입력합니다.
   - `p` → 다음 곡 재생
   - `s` → 정지
   - `v 15` → 볼륨 15 설정
   - `m` → 멜로디 재생 (피에조 부저)
3. 스피커에서 음악이, 부저에서 멜로디가 들립니다.

## 응용 아이디어

- 감지 시 효과음 재생(센서 + DFPlayer)
- 볼륨 노브로 실시간 볼륨 조절
- 파형(sine)을 그려 원하는 효과음 합성
