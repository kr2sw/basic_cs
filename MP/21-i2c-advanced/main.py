# 21: 고급 I2C — 다중 디바이스, 스캔, 레지스터 직접 접근
# 대상: ESP32 (SDA=GPIO21, SCL=GPIO22)
from machine import Pin, I2C
from time import sleep_ms, ticks_ms

# MPU6050 관련 레지스터 주소 (datasheet 참조)
MPU6050_ADDR = 0x68          # 기본 주소 (AD0=0)
WHO_AM_I     = 0x75          # 식별자 레지스터, 항상 0x68 반환
PWR_MGMT_1   = 0x6B          # 전원 관리 레지스터
SMPLRT_DIV   = 0x19          # 샘플 레이트 분주기
CONFIG       = 0x1A          # 필터 설정
GYRO_CONFIG  = 0x1B          # 자이로 풀스케일
ACCEL_CONFIG = 0x1C          # 가속도 풀스케일
ACCEL_XOUT_H = 0x3B          # 가속도 X 상위 바이트 (0x3B~0x40)
GYRO_XOUT_H  = 0x43          # 자이로 X 상위 바이트 (0x43~0x48)

i2c = I2C(0, scl=Pin(22), sda=Pin(21), freq=400_000)


def read_u8(addr, reg):
    """1바이트 레지스터 읽기"""
    i2c.writeto(addr, bytes([reg]))
    return i2c.readfrom(addr, 1)[0]


def write_u8(addr, reg, value):
    """1바이트 레지스터 쓰기"""
    i2c.writeto(addr, bytes([reg, value]))


def read_s16(addr, reg):
    """2바이트 부호 있는 정수 읽기 (빅엔디언)"""
    data = read_regs(addr, reg, 2)
    value = (data[0] << 8) | data[1]
    return value - 65536 if value >= 32768 else value


def read_regs(addr, reg, n):
    """n바이트 연속 읽기"""
    i2c.writeto(addr, bytes([reg]))
    return i2c.readfrom(addr, n)


def scan_bus():
    """버스에 연결된 모든 I2C 디바이스 주소 출력"""
    devices = i2c.scan()
    print(f"발견된 디바이스 {len(devices)}개: {[hex(d) for d in devices]}")
    return devices


def init_mpu6050():
    """MPU6050 레지스터 직접 설정"""
    who = read_u8(MPU6050_ADDR, WHO_AM_I)
    print(f"WHO_AM_I = 0x{who:02X} (기대값 0x68)")
    if who != 0x68:
        raise RuntimeError("MPU6050을 찾을 수 없습니다")

    write_u8(MPU6050_ADDR, PWR_MGMT_1, 0x01)   # 클록 소스를 자이로 X로, 슬립 해제
    write_u8(MPU6050_ADDR, SMPLRT_DIV, 0x00)   # 샘플링 분주 1
    write_u8(MPU6050_ADDR, CONFIG, 0x03)       # DLPF 44Hz 저역 통과 필터
    write_u8(MPU6050_ADDR, GYRO_CONFIG, 0x00)  # 자이로 ±250 dps
    write_u8(MPU6050_ADDR, ACCEL_CONFIG, 0x00) # 가속도 ±2g
    print("MPU6050 초기화 완료")


def read_accel():
    """가속도 원시값을 g 단위로 변환 (±2g → 16384 LSB/g)"""
    ax = read_s16(MPU6050_ADDR, ACCEL_XOUT_H) / 16384.0
    ay = read_s16(MPU6050_ADDR, ACCEL_XOUT_H + 2) / 16384.0
    az = read_s16(MPU6050_ADDR, ACCEL_XOUT_H + 4) / 16384.0
    return ax, ay, az


def read_gyro():
    """자이로 원시값을 °/s 단위로 변환 (±250 dps → 131 LSB/dps)"""
    gx = read_s16(MPU6050_ADDR, GYRO_XOUT_H) / 131.0
    gy = read_s16(MPU6050_ADDR, GYRO_XOUT_H + 2) / 131.0
    gz = read_s16(MPU6050_ADDR, GYRO_XOUT_H + 4) / 131.0
    return gx, gy, gz


def main():
    devices = scan_bus()
    if MPU6050_ADDR not in devices:
        print("MPU6050(0x68) 없음 — 배선과 주소를 확인하세요")
        return

    init_mpu6050()
    start = ticks_ms()

    # 5초 동안 초당 20회 샘플링
    count = 0
    while ticks_ms() - start < 5000:
        ax, ay, az = read_accel()
        gx, gy, gz = read_gyro()
        print(f"accel=({ax:6.2f}, {ay:6.2f}, {az:6.2f}) g  "
              f"gyro=({gx:6.1f}, {gy:6.1f}, {gz:6.1f}) deg/s")
        count += 1
        sleep_ms(50)

    print(f"총 {count}회 샘플링 완료")
    # 누르면 재시작하는 단순한 방식이므로 끝
    main()


if __name__ == "__main__":
    main()
