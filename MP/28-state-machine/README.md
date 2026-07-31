# 28: 상태 머신 — Event-Driven Design, HSM

## 개요

임베디드 프로그램은 "연속된 처리"보다 **여러 상태 사이의 전이**로 표현하는 것이 자연스러운 경우가 많습니다. **상태 머신(State Machine)** 은 상태, 이벤트, 전이를 명확히 정의해 복잡한 동작을 버그 없이 설계하는 방법입니다.

## 상태 머신의 구성 요소

```
상태:   OFF → ON → BLINK → OFF
이벤트: 버튼 누름, 타임아웃, 센서 알람
전이:   (현재 상태, 이벤트) → 다음 상태
```

- **상태(State)**: 기기가 처한 상황 (OFF, ON, BLINK)
- **이벤트(Event)**: 전이를 일으키는 입력 (버튼, 타이머, 메시지)
- **전이(Transition)**: 어떤 조건에서 어디로 이동할지

## 전이 테이블

복잡한 상태 머신은 `if` 문 대신 **테이블**로 표현하면 검증과 수정이 쉬워집니다.

```python
TABLE = {
    ("OFF",  "BUTTON"): "ON",
    ("ON",   "BUTTON"): "BLINK",
    ("ON",   "TIMEOUT"): "OFF",
    ("BLINK","BUTTON"): "OFF",
}
next_state = TABLE.get((state, event), state)  # 정의 없으면 유지
```

## 계층 상태 머신 (HSM)

상태가 많아지면 **상위 상태(Super-state)** 와 **하위 상태(Sub-state)** 로 계층화합니다. 공통 처리(예: 전원 OFF)는 상위에서 한 번만, 세부 동작은 하위에서 처리합니다.

```
POWERED_ON
 ├── IDLE
 └── RUNNING
POWERED_OFF
```

하위 상태에서 발생한 이벤트가 그 계층에서 처리되지 않으면 상위 계층이 처리합니다. 이를 **이벤트 위임(upward delegation)** 이라고 합니다.

## 왜 상태 머신인가?

- 시각적으로 검증 가능 (상태도, 테이블)
- "불가능한 상태"(예: OFF인데 가열 중)가 원천 차단됨
- 타이밍·하드웨어 버그 분리 → 디버깅 용이
- uasyncio와 결합하면 이벤트 기반 펌웨어의 표준 구조

## 실행/업로드 방법

1. **Thonny IDE**: `MP/28-state-machine/main.py`를 열어 실행(F5). 시리얼에서 전이 로그를 확인합니다.
2. **ampy**:
   ```bash
   ampy --port COM3 put MP/28-state-machine/main.py
   ampy --port COM3 run MP/28-state-machine/main.py
   ```
3. 마지막 루프에서 버튼을 누를 때마다 OFF → ON → BLINK → OFF 순서로 LED가 바뀝니다.

## 핵심 개념 요약

- 상태/이벤트/전이로 동작을 모델링
- 전이 테이블로 복잡도 관리
- HSM으로 공통 처리와 세부 처리를 계층 분리
