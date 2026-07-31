# 35: 보안 — Encryption, Key Management, Secure Boot Concepts

## 개요

IoT 기기가 인터넷에 연결되면 **보안**이 선택이 아닌 필수입니다. 이번 레슨에서는 해시/메시지 인증(HMAC), 대칭 암호화(AES), 키 관리, 보안 부팅 개념을 배웁니다. 무선 네트워크에서 데이터를 전송할 때 암호화 없이는 누구나 내용을 읽고 변조할 수 있습니다.

## 위협 모델

- **도청(Eavesdropping)**: 평문 전송을 가로채 읽기 → 암호화로 방어
- **변조(Tampering)**: 데이터를 바꿔치기 → MAC/서명으로 방어
- **위장(Spoofing)**: 가짜 기기로 위장 → 인증으로 방어
- **재생(Replay)**: 녹음한 패킷 재전송 → 시퀀스 번호/타임스탬프로 방어

## 해시와 HMAC

```python
import hashlib, hmac
h = hashlib.sha256(b"data").digest()          # 32바이트 해시
mac = hmac.new(b"secret-key", b"data", "sha256").digest()
```

- **해시**: 일방향 함수, 무결성 검증
- **HMAC**: 비밀 키를 넣은 해시 → 수신자가 키를 가진 상대만 만들 수 있음

## AES 대칭 암호화

```python
import cryptolib
key = b"0123456789abcdef"              # 16바이트 키 (AES-128)
cipher = cryptolib.aes(key, 1)         # 1 = CBC 모드
enc = cipher.encrypt(data16)           # 16바이트 블록 단위
```

- 같은 키로 암호화/복호화, 키 공유가 관건
- MCU용 MicroPython 펌웨어에 따라 `cryptolib` 또는 `cryptography` 사용

## 키 관리

키를 코드에 하드코딩하면 누구나 역추적할 수 있습니다.

```python
# 나쁜 예
KEY = "my-secret-password"

# 좋은 예: 설정 파일 + 파일 권한 제한
with open("/secret/key.bin", "rb") as f:
    key = f.read()
```

- 기기별 고유 키는 플래시의 보안 영역(efuse)에 저장
- 키가 유출되면 갱신할 수 있도록 키 로테이션 설계

## 보안 부팅 (Secure Boot)

펌웨어가 위조/변조되지 않았는지 부팅 시 검증하는 개념입니다.

- 부트로더가 펌웨어의 서명을 공개키로 검증
- 검증 실패 시 부팅 거부
- OTA 업데이트는 서명 확인 후에만 적용 (39장과 연결)

## 실행/업로드 방법

1. **Thonny IDE**: `MP/35-security/main.py`를 실행(F5). `cryptolib` 미지원 펌웨어면 해시/HMAC 부분만 동작합니다.
2. **ampy**:
   ```bash
   ampy --port COM3 put MP/35-security/main.py
   ampy --port COM3 run MP/35-security/main.py
   ```
3. 시리얼에서 해시/HMAC/AES 출력을 확인합니다.

## 핵심 개념 요약

- 암호화는 기밀성, HMAC/서명은 무결성·인증, 시퀀스는 재생 방지
- AES-128 CBC로 데이터 암호화, 키는 하드코딩 금지
- 보안 부팅으로 펌웨어 위변조 차단
