# 35: 보안 — 암호화, 키 관리, 보안 부팅 개념
# 대상: ESP32/Pico (cryptolib 지원 펌웨어 권장)
import hashlib
import hmac
import os
import time

# --- 1) 해시 (SHA-256) --------------------------------------------------------
def demo_hash():
    print("=== 1. SHA-256 해시 ===")
    data = b"temperature:22.5"
    digest = hashlib.sha256(data).digest()
    print(f"  원문    : {data.decode()}")
    print(f"  해시    : {digest.hex()}")
    # 데이터가 1비트만 바뀌어도 해시가 완전히 달라짐
    changed = hashlib.sha256(b"temperature:22.6").hexdigest()
    print(f"  변경 시 : {changed}")
    print("  무결성 검증: 해시 비교로 데이터 변조 감지")
    print()


# --- 2) HMAC (키 있는 해시: 무결성 + 인증) ---------------------------------------
def demo_hmac():
    print("=== 2. HMAC-SHA256 ===")
    shared_key = b"device-shared-secret-01"
    message = b"cmd:relay_on,id:3"
    tag = hmac.new(shared_key, message, "sha256").digest()
    print(f"  메시지 : {message.decode()}")
    print(f"  MAC    : {tag.hex()}")

    # 수신자가 같은 키로 다시 계산해 비교 → 위조/변조 감지
    expected = hmac.new(shared_key, message, "sha256").digest()
    print(f"  검증   : {'통과' if hmac.compare_digest(tag, expected) else '실패'}")
    print()


# --- 3) AES 암호화 (cryptolib 사용) ----------------------------------------------
def pad16(data):
    """AES는 16바이트 블록 단위 — PKCS7 패딩"""
    pad = 16 - (len(data) % 16)
    return data + bytes([pad] * pad)


def unpad16(data):
    pad = data[-1]
    return data[:-pad]


def demo_aes():
    print("=== 3. AES-128 CBC 암호화 ===")
    try:
        import cryptolib
    except ImportError:
        print("  cryptolib 미지원 펌웨어 — 암호화 부분 건너뜀")
        return

    key = b"0123456789abcdef"                 # 16바이트 키 (실전은 키 저장소)
    iv = os.urandom(16)                       # 초기화 벡터 (매번 랜덤)
    plain = pad16(b"sensor:temp:21.3")

    cipher = cryptolib.aes(key, 1, iv)        # 1 = CBC 모드
    encrypted = cipher.encrypt(plain)
    print(f"  평문      : {plain[:16]}")
    print(f"  암호문    : {encrypted.hex()}")

    cipher2 = cryptolib.aes(key, 1, iv)       # 복호화는 새 객체
    decrypted = unpad16(cipher2.decrypt(encrypted))
    print(f"  복호화    : {decrypted.decode()}")
    print()


# --- 4) 키 관리 원칙 --------------------------------------------------------------
def demo_key_management():
    print("=== 4. 키 관리 원칙 ===")
    print("  - 키를 소스코드에 하드코딩하지 않기")
    print("  - 보드 플래시 보안 영역(efuse)/RTC에 키 저장")
    print("  - 키 로테이션: 유출 시 즉시 교체 가능하도록 설계")
    print("  - 설정 파일은 개인키 처럼 취급 (권한 제한)")
    try:
        with open("/secret/key.bin", "wb") as f:
            f.write(b"0123456789abcdef")
        print("  - /secret/key.bin 에 키 저장 완료 (예시)")
    except OSError:
        print("  - /secret 디렉터리가 없어 키 저장 생략")
    print()


# --- 5) 보안 부팅 개념 --------------------------------------------------------------
def demo_secure_boot():
    print("=== 5. 보안 부팅 개념 ===")
    print("  부팅 순서: BootROM → Bootloader → App")
    print("  - Bootloader는 App 펌웨어의 디지털 서명을 공개키로 검증")
    print("  - 서명 불일치 → 부팅 거부 (위변조 펌웨어 실행 차단)")
    print("  - OTA 업데이트 시에도 서명 검증 후 플래시 기록")
    print()


def main():
    print("=== IoT 보안 데모 시작 ===\n")
    demo_hash()
    demo_hmac()
    demo_aes()
    demo_key_management()
    demo_secure_boot()
    print("완료")


if __name__ == "__main__":
    main()
