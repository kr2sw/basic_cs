# 28: 상태 머신 — Event-Driven Design, HSM
# 대상: ESP32 (LED=GPIO2, 버튼=GPIO0) — 어떤 보드에서도 동작
from machine import Pin
import time

led = Pin(2, Pin.OUT)
button = Pin(0, Pin.IN, Pin.PULL_UP)

# --- 단순 상태 머신 (FSM) ----------------------------------------------
# 상태: OFF / ON / BLINK
class LightFSM:
    STATE_OFF, STATE_ON, STATE_BLINK = "OFF", "ON", "BLINK"

    def __init__(self):
        self.state = self.STATE_OFF

    def _enter(self, state):
        """상태 진입 시 실행되는 처리"""
        print(f"→ 진입: {state}")
        if state == self.STATE_OFF:
            led.value(0)
        elif state == self.STATE_ON:
            led.value(1)

    def on_event(self, event):
        """이벤트 기반 전이 테이블"""
        if self.state == self.STATE_OFF:
            if event == "BUTTON":
                self._enter(self.STATE_ON)
        elif self.state == self.STATE_ON:
            if event == "BUTTON":
                self._enter(self.STATE_BLINK)
            elif event == "TIMEOUT_5S":
                self._enter(self.STATE_OFF)
        elif self.state == self.STATE_BLINK:
            if event == "BUTTON":
                self._enter(self.STATE_OFF)
        self.state = self.state  # (위 _enter에서 갱신할 경우 표기용)

    # 위 구현이 복잡하므로 전이 테이블 방식으로 재구성:
    def transition(self, event):
        """전이 테이블: (현재상태, 이벤트) → 다음상태"""
        table = {
            (self.STATE_OFF, "BUTTON"): (self.STATE_ON, None),
            (self.STATE_ON, "BUTTON"): (self.STATE_BLINK, None),
            (self.STATE_ON, "TIMEOUT_5S"): (self.STATE_OFF, None),
            (self.STATE_BLINK, "BUTTON"): (self.STATE_OFF, None),
        }
        nxt, action = table.get((self.state, event), (self.state, None))
        if nxt != self.state:
            self._enter(nxt)
            self.state = nxt
        return nxt


# --- 계층 상태 머신 (HSM) -----------------------------------------------
# 상위 상태 POWERED_ON 아래에 IDLE / RUNNING 이라는 하위 상태가 존재
class HeaterHSM:
    POWERED_OFF = "POWERED_OFF"
    POWERED_ON = "POWERED_ON"
    IDLE = "IDLE"
    RUNNING = "RUNNING"

    def __init__(self):
        self.state = self.POWERED_OFF

    def in_state(self, candidate):
        """상태 계층 검사: 하위 상태가 상위 상태에 포함되는지"""
        if self.state == candidate:
            return True
        if candidate == self.POWERED_ON and self.state in (self.IDLE, self.RUNNING):
            return True
        return False

    def on_event(self, event):
        if self.state == self.POWERED_OFF:
            if event == "POWER":
                print("전원 ON")
                self.state = self.IDLE
        elif self.state in (self.IDLE, self.RUNNING):   # 상위 상태 공통 처리
            if event == "POWER":
                print("전원 OFF")
                self.state = self.POWERED_OFF
            elif event == "START" and self.state == self.IDLE:
                print("RUNNING 진입")
                self.state = self.RUNNING
            elif event == "STOP" and self.state == self.RUNNING:
                print("IDLE 복귀")
                self.state = self.IDLE
        # 상위 상태(POWERED_ON) 처리와 하위 상태 처리의 분리가 HSM의 핵심


def main():
    print("=== 1) FSM: LED 제어 ===")
    fsm = LightFSM()
    # 세 가지 상태 전이 시연
    fsm.transition("BUTTON")       # OFF → ON
    fsm.transition("BUTTON")       # ON → BLINK
    fsm.transition("TIMEOUT_5S")   # BLINK → BLINK (정의 안 됨, 유지)
    fsm.transition("BUTTON")       # BLINK → OFF
    print()

    print("=== 2) HSM: 히터 계층 상태 ===")
    heater = HeaterHSM()
    heater.on_event("POWER")       # OFF → IDLE (POWERED_ON 계층)
    heater.on_event("START")       # IDLE → RUNNING
    heater.on_event("START")       # RUNNING에서 START 무시
    print(f"in_state(POWERED_ON)? {heater.in_state(HeaterHSM.POWERED_ON)}")
    heater.on_event("STOP")        # RUNNING → IDLE
    heater.on_event("POWER")       # IDLE → OFF
    print("HSM 시연 완료")

    print("\n=== 3) 실제 버튼으로 FSM 구동 ===")
    real = LightFSM()
    while True:
        if button.value() == 0:
            real.transition("BUTTON")
            time.sleep_ms(300)     # 디바운스


if __name__ == "__main__":
    main()
