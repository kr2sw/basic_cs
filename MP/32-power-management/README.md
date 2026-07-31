# 32: 전원 관리 — Deep Sleep, Periodic Wake, Current Optimization

## 개요

배터리로 동작하는 IoT 기기에서 **전류 최적화**는 가장 중요한 설계 목표입니다. ESP32의 **딥 슬립(Deep Sleep)** 모드는 CPU와 대부분 주변장치의 전원을 차단해 소비 전류를 수십 mA에서 수십 μA까지 낮춥니다. 이번 레슨에서는 주기적 웨이크와 RTC 메모리를 이용한 상태 유지 방법을 배웁니다.

## ESP32의 절전 모드

| 모드 | 소비 전류 | 동작 |
|------|-----------|------|
| Active | 80~240 mA | CPU 최대 성능 |
| Modem Sleep | 10~20 mA | CPU 동작, Wi-Fi 오프 |
| Light Sleep | 0.8 mA | CPU 정지, 주변장치 정지 |
| **Deep Sleep** | **10~150 μA** | CPU·주변장치 전원 차단, RTC만 동작 |

## 딥 슬립 진입과 주기적 웨이크

```python
import machine

machine.deepsleep(10 * 1000)   # 10초 후 타이머로 자동 웨이크
```

- **타이머 웨이크**: `deepsleep(ms)` 로 지정한 시간 후 RTC 타이머가 깨움
- **외부 핀 웨이크**: `ESP32.wake_on_ext0/ext1` 로 GPIO 신호 감지
- 깨어나면 **콜드 부트처럼** `main.py`가 처음부터 다시 실행됨

## RTC 메모리로 상태 보존

딥 슬립 중에도 **RTC 메모리(8KB)** 는 유지됩니다. 웨이크 횟수 같은 상태를 여기에 저장합니다.

```python
from machine import RTC
rtc = RTC()
rtc.memory(b"\x03")          # 저장
count = rtc.memory()[0]      # 복원
```

## 웨이크 원인 확인

```python
from machine import machine
if machine.reset_cause() == machine.DEEPSLEEP_RESET:
    print("딥 슬립에서 깨어남")
```

`reset_cause()`로 콜드 부트와 웨이크를 구분해 초기화를 다르게 할 수 있습니다.

## 전류 최적화 체크리스트

- 쓰지 않는 **ADC를 끄고**, 사용 시 `deinit()`
- Wi-Fi/BT는 작업이 끝나면 즉시 `active(False)`
- LED는 켜는 시간을 최소화
- 딥 슬립 주기를 늘릴수록 평균 전류가 감소
- 아날로그 핀은 플로팅 방지(외부 풀다운)

## 실행/업로드 방법

1. **Thonny IDE**: `MP/32-power-management/main.py`를 보드에 저장합니다. 딥 슬립 중에는 REPL이 끊기므로 **ampy로 업로드 후 리셋**하는 방식을 권장합니다.
2. **ampy**:
   ```bash
   ampy --port COM3 put MP/32-power-management/main.py
   ```
3. 보드 리셋 후 시리얼 모니터에서 "누적 웨이크 횟수"가 1씩 증가하는 것을 확인합니다. 배터리 전류계로 슬립 전후 소비 전류를 측정해보세요.

## 핵심 개념 요약

- 딥 슬립: CPU/주변장치 차단으로 소비 전류를 수십 μA까지
- `deepsleep(ms)` 타이머 웨이크, `reset_cause()`로 원인 구분
- RTC 메모리로 슬립 후에도 상태 유지
