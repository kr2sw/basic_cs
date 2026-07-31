#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_BMP280.h>
#include <Adafruit_MPU6050.h>

// I2C 센서 객체 생성
Adafruit_BMP280 bmp;   // 기압/온도 센서 (주소 0x76)
Adafruit_MPU6050 mpu;  // 6축 가속도/자이로 센서 (주소 0x68)

void scanI2C() {
  Serial.println("\n[I2C 주소 스캔]");
  for (byte addr = 1; addr < 127; addr++) {
    Wire.beginTransmission(addr);
    if (Wire.endTransmission() == 0) {
      Serial.print("  발견: 0x");
      Serial.println(addr, HEX);
    }
  }
  Serial.println("[스캔 완료]");
}

void setup() {
  Serial.begin(9600);
  while (!Serial) { delay(10); }

  scanI2C();

  // BMP280 초기화
  if (!bmp.begin(0x76)) {
    Serial.println("BMP280 초기화 실패! 배선 확인");
  } else {
    Serial.println("BMP280 초기화 성공");
  }

  // MPU6050 초기화
  if (!mpu.begin()) {
    Serial.println("MPU6050 초기화 실패! 배선 확인");
  } else {
    Serial.println("MPU6050 초기화 성공");
    mpu.setAccelerometerRange(MPU6050_RANGE_8_G);
    mpu.setGyroRange(MPU6050_RANGE_500_DEG);
  }
}

void loop() {
  Serial.println("====================");

  // --- BMP280 데이터 ---
  float temp = bmp.readTemperature();
  float press = bmp.readPressure() / 100.0F;        // hPa 단위
  float alt = bmp.readAltitude(1013.25);            // 해수면 기압 기준 고도

  Serial.print("BMP280  온도: ");
  Serial.print(temp);
  Serial.print(" C,  기압: ");
  Serial.print(press);
  Serial.print(" hPa,  고도: ");
  Serial.print(alt);
  Serial.println(" m");

  // --- MPU6050 데이터 ---
  sensors_event_t accel, gyro, tempEvent;
  mpu.getEvent(&accel, &gyro, &tempEvent);

  Serial.print("MPU6050 가속도 X: ");
  Serial.print(accel.acceleration.x, 2);
  Serial.print(" Y: ");
  Serial.print(accel.acceleration.y, 2);
  Serial.print(" Z: ");
  Serial.println(accel.acceleration.z, 2);

  Serial.print("MPU6050 자이로  X: ");
  Serial.print(gyro.gyro.x, 2);
  Serial.print(" Y: ");
  Serial.print(gyro.gyro.y, 2);
  Serial.print(" Z: ");
  Serial.println(gyro.gyro.z, 2);

  // 가속도로 기울기 계산 (중력 가속도 9.8 m/s^2 기준)
  float tiltX = atan2(accel.acceleration.y, accel.acceleration.z) * 180.0 / PI;
  float tiltY = atan2(-accel.acceleration.x, accel.acceleration.z) * 180.0 / PI;
  Serial.print("기울기  X: ");
  Serial.print(tiltX);
  Serial.print(" deg,  Y: ");
  Serial.println(tiltY);

  delay(2000);
}
