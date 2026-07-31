# 31: 고급 데이터 포맷 (Data Formats) — csv, openpyxl, configparser, pickle

## csv (표준 라이브러리)
`csv.reader`/`csv.writer`로 표 형식 데이터를 다룹니다. `DictReader`/`DictWriter`는 헤더를 키로 사용합니다.

## openpyxl (Excel)
`.xlsx` 파일을 읽고 쓸 수 있는 서드파티 라이브러리입니다. `pip install openpyxl`. 아래 예제는 주석으로 제공됩니다.

## configparser
INI 형식 설정 파일을 읽고 씁니다. 프로그램 설정값 관리에 유용합니다.

```python
import configparser
config = configparser.ConfigParser()
config.read("app.ini")
```

## pickle
Python 객체를 직렬화(byte로 변환)합니다. `pickle.loads`/`dumps`. 보안 문제로 신뢰할 수 없는 데이터에는 쓰지 마세요.

## 실행

```bash
python main.py
```
