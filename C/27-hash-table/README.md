# 27: 해시 테이블 — 해시 함수, 체이닝, 오픈 어드레싱

## 해시 함수

키를 배열 인덱스로 변환하는 함수입니다. 좋은 해시는 균등 분포와 빠른 계산을 목표로 합니다.

```c
// djb2 (Daniel J. Bernstein)
unsigned long hash(const char* str) {
    unsigned long h = 5381;
    while (*str) h = h * 33 + (unsigned char)*str++;
    return h;
}
```

충돌(collision): 서로 다른 키가 같은 인덱스에 매핑되는 현상.

## 충돌 해결 1: 체이닝 (Chaining)

각 슬롯에 연결 리스트를 두고, 충돌하면 리스트에 이어 붙입니다.

```c
struct Slot { KeyValue* head; };   // 연결 리스트
```

- 구현 간단, 테이블이 꽉 차지 않음
- 해시가 나쁘면 특정 버킷에 몰림 (편향)

## 충돌 해결 2: 오픈 어드레싱 (Open Addressing)

모든 데이터를 테이블에 직접 저장하고, 빈 슬롯을 찾을 때까지 탐사합니다.

```c
int idx = hash(key) % size;
while (table[idx].occupied) idx = (idx + 1) % size;  // 선형 탐사
```

- 삭제 시 **톰스톤(tombstone)** 표시가 필요
- 테이블이 채워질수록 성능 저하 → 부하율(load factor) 관리

## 실행

```bash
gcc main.c -o main && ./main
```
