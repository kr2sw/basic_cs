# 39: C 디자인 패턴 — 상태 머신, 옵저버, 리소스 풀

## 상태 머신 (State Machine)

객체의 상태에 따라 동작이 달라지는 패턴입니다. 상태 전이표(transition table)로 구현합니다.

```c
typedef enum { GREEN, YELLOW, RED } LightState;

void onTick(LightState* s) {
    if (*s == GREEN) *s = YELLOW;
    else if (*s == YELLOW) *s = RED;
    else *s = GREEN;
}
```

- 게임 AI, 프로토콜 파서, 주문 처리에 널리 사용
- `switch` 기반 또는 함수 포인터 테이블 기반으로 구현

## 옵저버 (Observer)

상태가 변하면 등록된 관찰자(리스너)에게 알리는 패턴입니다. 이벤트 처리의 기초입니다.

```c
typedef struct { void (*onEvent)(const char* msg); } Listener;
```

- GUI 이벤트, 센서 알림, 로그 시스템에 사용
- `struct` + 함수 포인터 배열로 구현

## 리소스 풀 (Resource Pool)

생성 비용이 큰 객체를 미리 만들어 두고 빌려주는 패턴입니다. 사용 후 반납하면 재사용합니다.

```c
typedef struct { int used; int connectionId; } Connection;
```

- DB 커넥션, 스레드 풀, 소켓 풀에 사용
- 점유/반납 상태 추적과 부족 시 대기/오류 처리가 핵심

## 실행

```bash
gcc main.c -o main && ./main
```
