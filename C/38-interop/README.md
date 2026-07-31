# 38: 언어 간 연동 — C ABI, extern "C", Python ctypes 호출 개념

## C ABI (Application Binary Interface)

바이너리 수준에서 함수를 호출하는 규칙입니다. C는 사실상의 표준이어서 다른 언어들이 C ABI를 통해 C 라이브러리를 호출합니다.

- 함수 이름, 인자/반환 타입, 구조체 메모리 배치가 핵심
- 플랫폼별 차이: 구조체 패딩, 호출 규약(cdecl/stdfast), 엔디언

## extern "C" (C++에서 C 함수 호출)

C++는 이름 장식(name mangling) 때문에 `#ifdef __cplusplus` 가드가 필요합니다.

```c
#ifdef __cplusplus
extern "C" {
#endif

int add(int a, int b);   // C ABI로 내보냄

#ifdef __cplusplus
}
#endif
```

## Python ctypes

```python
import ctypes
lib = ctypes.CDLL("./libmylib.so")   # 또는 .dll
lib.add.argtypes = [ctypes.c_int, ctypes.c_int]
lib.add.restype = ctypes.c_int
print(lib.add(3, 4))
```

- C 구조체는 `ctypes.Structure`로 재정의해 전달
- 문자열은 `bytes`/`c_char_p` 주의 필요 (뮤터블 여부)

## 구조체 패딩과 이식성

```c
#pragma pack(push, 1)   // 패딩 제거 → 바이너리 호환에 유리
struct Header { char magic[2]; uint32_t len; };
#pragma pack(pop)
```

본 강의 main.c는 표준 C로 ABI 개념(패딩, 엔디언)을 시연합니다.

## 실행

```bash
gcc main.c -o main && ./main
```
