# 36: 암호화 기초 — XOR, 해시 구현, HMAC 개념, 난수

## XOR 암호화

같은 키로 두 번 XOR하면 원문이 복원되는 성질을 이용합니다.

```c
cipher = plain ^ key;    // 암호화
plain  = cipher ^ key;   // 복호화 (같은 연산)
```

- 한 글자 키/패턴 반복은 빈도 분석에 취약 → 실전에 부적합
- 암호학 개념 학습과 간단한 난독화에 사용

## 해시 함수

임의 길이 입력 → 고정 길이 출력. 같은 입력은 같은 출력, 입력이 조금만 달라도 출력이 크게 달라야 합니다.

```c
// FNV-1a 해시
unsigned long hash(const char* s) {
    unsigned long h = 2166136261UL;
    while (*s) { h ^= (unsigned char)*s++; h *= 16777619UL; }
    return h;
}
```

- CRC, FNV, djb2는 **빠른 체크섬**용 (암호학적 해시 아님)
- 실제 암호화에는 SHA-256 등 암호학적 해시 사용

## HMAC 개념

메시지와 키를 함께 해시해 위변조를 확인합니다 (MAC: 메시지 인증 코드).

```c
// 개념: inner = H((key ^ ipad) + msg),  outer = H((key ^ opad) + inner)
```

## 난수

- `rand()`/`srand()`는 예측 가능 → 암호화에 부적합
- POSIX `/dev/urandom`, Windows `CryptGenRandom` 사용 권장
- 본 강의는 LCG(선형 합동 생성기)를 직접 구현해 개념을 보여줍니다

## 실행

```bash
gcc main.c -o main && ./main
```
