# 34: 상태 머신 — State Machine

enum으로 상태를 정의하고, 상태 간 전이(transition)를 설계하는 FSM(Finite State Machine)을 구현합니다.

## 학습 내용
- 상태 머신(FSM) 개념
- enum으로 상태 정의
- 상태 전이와 이벤트 처리
- millis() 기반 비블로킹 상태 동작
- 시나리오 설계 (신호등 / 세탁기 / 자동 문)

## FSM 개념

FSM은 "지금 어떤 상태인지"를 기억하고, 이벤트(버튼, 시간)에 따라 다음 상태로 전이하는 구조입니다. 동시에 처리해야 할 일이 복잡해질수록 강력합니다.

```
(빨강) --5초--> (초록) --4초--> (노랑) --2초--> (빨강)
```

```cpp
enum State { IDLE, RUNNING, DONE };
State state = IDLE;

switch (state) {
  case IDLE:    // 대기 중
  case RUNNING: // 동작 중
  case DONE:    // 완료
}
```

## 상태 + 타이머 설계

상태마다 `millis()`로 시작 시각을 기록하면 `delay()` 없이 시간 경과를 처리할 수 있습니다.

```cpp
unsigned long stateStart = 0;

void enterState(State newState) {
  state = newState;
  stateStart = millis();  // 새 상태 진입 시각 기록
}

if (millis() - stateStart >= 5000) {
  enterState(NEXT_STATE);  // 5초 후 전이
}
```

## 시나리오 설계 순서

1. **상태 목록 만들기** — 예: 신호등 {RED, GREEN, YELLOW}
2. **전이 조건 정의** — 어떤 이벤트/시간에 바뀌는가
3. **각 상태의 동작 작성** — LED 켜기, 모터 돌리기
4. **전이 함수 작성** — 상태 변경과 타이머 리셋

## 회로 연결 (신호등 시나리오)

| 부품 | Arduino Uno |
|------|-------------|
| 빨간 LED (+220Ω) | D9 |
| 노란 LED (+220Ω) | D10 |
| 초록 LED (+220Ω) | D11 |
| 보행자 버튼 (선택) | D2, 반대쪽 GND |

## 실행 방법

1. 이 챕터의 `.ino`를 업로드합니다.
2. 시리얼 모니터(9600)에 신호등 상태 전이가 로그로 출력됩니다.
3. LED가 빨강(5초) → 초록(4초) → 노랑(2초) → 빨강 순서로 반복합니다.
4. 버튼을 누르면 보행자 요청 상태로 전이합니다.

## 응용 아이디어

- 세탁기/전자레인지 단계(세척→헹굼→탈수) 제어
- 장애물 회피 로봇(전진→감지→후진→회전)
- 40장(종합 프로젝트)의 로봇 제어 시나리오
