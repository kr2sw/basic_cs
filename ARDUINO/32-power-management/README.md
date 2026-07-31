# 32: 전원 관리 — Power Management

배터리로 동작하는 IoT 장치를 위해 딥 슬립, 인터럽트 웨이크, 배터리 전압 측정을 다룹니다. ESP32의 전원 관리 기능을 중심으로 합니다.

## 학습 내용
- 전원 소모의 이해와 절전 전략
- ESP32 딥 슬립(Deep Sleep)과 웨이크 원인
- 인터럽트(EXT0/터치)와 타이머로 깨우기
- 배터리 전압 측정(ADC)

## 절전 전략

배터리 수명을 늘리려면 "안 자는 시간"을 최소화해야 합니다. 동작 시간을 줄이고, 불필요한 주변장치(LED, 센서)를 끄는 것이 기본입니다.

```
대기 전류 예시 (ESP32 기준)
실행(연결)  : 수십~수백 mA
Modem Sleep : ~20 mA
딥 슬립     : ~10 µA
```

## 딥 슬립 설정

`esp_deep_sleep_start()`를 호출하면 CPU가 멈추고 최소 전력으로 대기합니다. 웨이크 조건을 미리 등록합니다.

```cpp
esp_sleep_enable_timer_wakeup(30 * 1000000ULL);  // 30초 후
esp_deep_sleep_start();                          // 절전 시작
```

## 웨이크 원인 확인

재부팅된 후 `esp_sleep_get_wakeup_cause()`로 깨어난 이유를 알 수 있습니다. (타이머 / 외부 인터럽트 / 콜드 부팅)

```cpp
esp_sleep_wakeup_cause_t cause = esp_sleep_get_wakeup_cause();
if (cause == ESP_SLEEP_WAKEUP_TIMER) {
  Serial.println("타이머로 깨어남");
} else if (cause == ESP_SLEEP_WAKEUP_EXT0) {
  Serial.println("버튼으로 깨어남");
}
```

## 인터럽트 웨이크

버튼을 눌렀을 때만 깨어나게 하려면 EXT0 웨이크를 사용합니다. 웨이크 핀은 RTC 영역 GPIO여야 합니다(ESP32: 0,2,4,12~15,25~27,32~39).

```cpp
esp_sleep_enable_ext0_wakeup(GPIO_NUM_4, LOW);  // GPIO4 LOW 시 웨이크
```

## 배터리 전압 측정

ADC(아날로그)로 배터리 전압을 읽고 전원 분배기 비율로 실제 전압을 환산합니다. ESP32의 ADC는 입력 범위가 제한되어 있어 분배기가 필요합니다.

```cpp
int raw = analogRead(BATTERY_PIN);
float voltage = raw / 4095.0 * 3.3 * (DIVIDER_RATIO);
```

## 회로 연결 (ESP32)

| 부품 | ESP32 |
|------|-------|
| 버튼 (웨이크) | GPIO4, 반대쪽 GND |
| 배터리 분배기 출력 | GPIO34 (ADC) |

> 배터리 분배기: 배터리(+) → 100kΩ → GPIO34 → 100kΩ → GND. 두 저항이 같으면 측정 전압의 2배가 실제 전압입니다.

## 실행 방법

1. 이 챕터는 ESP32 전용입니다. **보드 매니저**에서 ESP32 패키지를 설치합니다.
2. `.ino` 파일을 ESP32 보드로 업로드합니다.
3. 시리얼 모니터(115200)에서 웨이크 원인과 배터리 전압을 확인합니다.
4. 동작 후 10초간 딥 슬립에 들어갔다가 다시 깨어나는 과정이 반복됩니다. 버튼을 누르면 즉시 깨어납니다.
5. 딥 슬립 중 전류를 측정하려면 멀티미터를 전원에 직렬로 연결합니다.

## 응용 아이디어

- 배터리 잔량을 주기적으로 서버로 전송하는 센서 노드
- 동작 감지(인터럽트) 시에만 켜지는 카메라/센서
- 26장(MQTT)과 결합한 저전력 IoT 노드
