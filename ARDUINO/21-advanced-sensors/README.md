# 21: 고급 센서 — Advanced Sensors

BMP280 기압/온도 센서와 MPU6050 가속도/자이로 센서를 I2C로 읽어 환경 데이터와 자세(姿勢) 데이터를 얻습니다.

## 학습 내용
- I2C 버스 개념과 센서 주소 스캔
- BMP280: 온도, 기압, 고도 계산
- MPU6050: 6축(가속도 + 자이로) 데이터 읽기
- Adafruit 센서 라이브러리 설치와 사용

## I2C 센서 개요

고급 센서 대부분은 I2C 인터페이스를 사용합니다. 같은 버스에 주소가 다른 장치 여러 개를 연결할 수 있습니다.

```cpp
#include <Wire.h>
// 모든 I2C 주소를 스캔하여 연결된 장치 확인
for (byte addr = 1; addr < 127; addr++) {
  Wire.beginTransmission(addr);
  if (Wire.endTransmission() == 0) {
    Serial.println(addr, HEX); // 0x76, 0x68 등이 발견됨
  }
}
```

## BMP280 기압/온도

BMP280은 기압(Pa)과 온도(℃)를 측정하고, 해수면 기압을 알면 고도를 계산할 수 있습니다. 날씨 변화 감지, 고도 측정에 사용됩니다.

```cpp
#include <Adafruit_BMP280.h>
Adafruit_BMP280 bmp;              // I2C 기본 주소 0x76
bmp.begin(0x76);

float t = bmp.readTemperature();            // 섭씨
float p = bmp.readPressure() / 100.0F;      // hPa
float a = bmp.readAltitude(1013.25);        // 미터 (해수면 기압 기준)
```

## MPU6050 6축 센서

MPU6050은 3축 가속도계 + 3축 자이로스코프가 통합된 IMU(Inertial Measurement Unit)입니다. 가속도로 중력 방향(기울기)을, 자이로로 회전 속도를 측정합니다.

```cpp
#include <Adafruit_MPU6050.h>
Adafruit_MPU6050 mpu;             // 기본 주소 0x68
mpu.begin();
mpu.setAccelerometerRange(MPU6050_RANGE_8_G);
mpu.setGyroRange(MPU6050_RANGE_500_DEG);

sensors_event_t a, g, t;
mpu.getEvent(&a, &g, &t);
float ax = a.acceleration.x;       // 가속도 (m/s^2)
float gz = g.gyro.z;               // 각속도 (rad/s)
```

## 센서 데이터 활용 아이디어

- 기압 변화로 비/맑음 예측, 고도계 만들기
- 가속도로 기울기 감지(레벨러, 자세 제어)
- 자이로로 회전 감지(로봇 회전 각도, 스테이빌라이저)

## 회로 연결

두 센서 모두 I2C를 사용하므로 Uno의 A4(SDA), A5(SCL)에 병렬 연결합니다.

| 모듈 | VCC | GND | SDA | SCL |
|------|-----|-----|-----|-----|
| BMP280 | 3.3V | GND | A4 | A5 |
| MPU6050 | 3.3V | GND | A4 | A5 |

> 주의: 두 센서 모두 3.3V 전원을 권장합니다. 주소가 다르므로(0x76, 0x68) 같은 버스 공유가 가능합니다.

## 실행 방법

1. Arduino IDE의 **라이브러리 관리자**에서 `Adafruit BMP280 Library`, `Adafruit MPU6050`을 설치합니다.
2. **도구 → 보드**에서 Arduino Uno를 선택합니다.
3. 이 챕터의 `.ino` 파일을 열고 **업로드** 버튼을 누릅니다.
4. **시리얼 모니터**를 9600 보드레이트로 열면 I2C 스캔 결과와 센서 값이 출력됩니다.
