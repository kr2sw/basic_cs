# 31: GPS — GPS Receiver

GPS 모듈로 NMEA 문장을 수신하고 TinyGPSPlus로 위치, 속도, 시간을 파싱합니다.

## 학습 내용
- GPS와 NMEA 프로토콜
- TinyGPSPlus 라이브러리
- 위치(위도/경도), 속도, 고도, 시간 파싱
- 위성 수신 상태 확인

## GPS와 NMEA

GPS 모듈은 `$GPGGA`, `$GPRMC` 같은 **NMEA 문장**을 1Hz(매초)로 출력합니다. 사람이 읽긴 어렵지만 라이브러리가 자동으로 파싱해줍니다.

```
$GPRMC,061524.000,A,3721.556,N,12703.601,E,0.5,180.0,310726,,,A*66
```

이 문장에는 시간, 유효성(A), 위도(37°21.556'N), 경도(127°03.601'E), 속도, 진행 방향이 담겨 있습니다.

## TinyGPSPlus

`gps.encode(c)`에 문자를 한 글자씩 넣으면 문장이 완성될 때 내부 데이터가 갱신됩니다.

```cpp
#include <TinyGPSPlus.h>
TinyGPSPlus gps;

while (gpsSerial.available() > 0) {
  if (gps.encode(gpsSerial.read())) {
    // 새 문장 파싱 완료
  }
}
```

## 데이터 읽기

각 필드는 `isValid()`로 유효한지 확인한 뒤 사용합니다.

```cpp
if (gps.location.isValid()) {
  gps.location.lat();      // 위도
  gps.location.lng();      // 경도
}
if (gps.speed.isValid()) gps.speed.kmph();  // km/h
if (gps.time.isValid()) gps.time.hour();    // UTC 시간
if (gps.altitude.isValid()) gps.altitude.meters();
```

> GPS 시각은 UTC(세계 협정시)입니다. 한국은 +9시간입니다.

## 회로 연결 (NEO-6M 등 UART GPS)

| GPS 모듈 | Arduino Uno |
|----------|-------------|
| VCC | 5V (또는 3.3V) |
| GND | GND |
| TX | D2 (SoftwareSerial RX) |
| RX | D3 (SoftwareSerial TX, 선택) |

> 위성 신호를 받으려면 **창가나 야외**에서 확인해야 합니다. 고정이 안 되면 안테나 방향과 위치를 바꿔 보세요. 시리얼 모니터에서 `GPGGA` 문장이 나오는지 먼저 확인합니다.

## 실행 방법

1. **라이브러리 관리자**에서 `TinyGPSPlus`를 설치합니다.
2. 이 챕터의 `.ino`를 업로드하고 시리얼 모니터(115200)를 엽니다.
3. 위성이 잠기면(3D Fix) 위치/속도/고도가 표시됩니다.
4. 지도에서 확인하려면 출력된 위도/경도를 구글 지도에 입력해 보세요.

## 응용 아이디어

- 이동 경로 SD 카드 로깅(23장) — 트래커
- 속도 측정으로 자전거/차량 주행 기록
- 지오펜스(특정 반경 이탈 감지)로 알림
