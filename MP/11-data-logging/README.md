# 11일차: 데이터 로깅

## 개념 소개

이 수업에서는 마이크로비트 보드에 데이터를 저장하는 방법을 배우게 됩니다. 주요 개념은 다음과 같습니다:

1. **데이터 로깅**: 센서 데이터를 주기적으로 저장하고 기록하는 것
2. **파일 쓰기**: SD 카드에 데이터 파일 생성 및 쓰기
3. **CSV 형식**: 깔끔하고 읽기 쉬운 데이터 구조
4. **데이터 보존**: 보드 재시작 후에도 데이터 유지
5. **자동화**: 조건에 따른 자동 데이터 수집

## 예시 코드

```python
def current_time():
    from microbit import sleep
    import pybytes
    # 마이크로비트가 지원하는 시간 형식
    return str(microbit.running_time())
def log_temperature_to_csv():
    # 현재 온도(예시) 가져오기
    temperature = 22.5
    timestamp = current_time()
    filename = "temperature_log.csv"

    try:
        # CSV 파일에 데이터 추가
        with open(filename, 'a') as f:
            if f.tell() == 0:
                f.write("시간,온도(°C)\n")
            f.write(f"{timestamp},{temperature}\n")
        # LED로 기록 성공 알림
        for i in range(3):
            microbit.display.scroll("LOG OK")
            sleep(200)
    except Exception as e:
        microbit.display.scroll("ERR")
        sleep(500)
def main():
    global temperature_log_running
    temperature_log_running = True
    while temperature_log_running:
        log_temperature_to_csv()
        sleep(5000)  # 5초마다 기록

temperature_log_running = False

main()
```

## 키 개념

- `open(filename, 'a')`: 파일을 추가 모드로 열기
- `f.write()`: 파일에 텍스트 쓰기
- `f.tell()`: 파일 포인터 위치 확인 (헤더 작성용)
- JSON 파일의 CSV 형식: 쉼표로 구분된 값

## 실행 방법

1. 마이크로비트를 USB로 컴퓨터에 연결
2. main.py 파일을 보드에 복사
3. 보드를 컴퓨터에서 읽기/쓰기 가능한 상태로 마운트
4. 데이터 로그가 CSV 파일로 SD 카드에 저장됩니다

## 개선 제안

- 센서 추가 (온도, 습도, 소리 등)
- 루프 간격 조정
- 파일 크기 제한 추가
- 하나의 파일에 여러 센서 데이터 저장