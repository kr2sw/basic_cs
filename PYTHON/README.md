# Python 기초 강의 (20개 챕터)

Python 프로그래밍 언어의 기초부터 고급 개념까지 학습할 수 있는 예제 모음입니다.

## 역사

Python은 1991년 네덜란드의 Guido van Rossum이 CWI 연구소에서 개발하기 시작했습니다. "Monty Python's Flying Circus"에서 이름을 따왔으며, 코드 가독성과 개발자 생산성을 최우선 목표로 설계되었습니다. 1994년 Python 1.0, 2000년 Python 2.0, 2008년 Python 3.0이 출시되었습니다. Python 2와 3의 분열은 오랫동안 혼란을 야기했으나, 2020년 Python 2의 공식 지원이 종료되면서 Python 3.x로 완전히 전환되었습니다. 현재는 데이터 과학, 머신러닝, 웹 개발, 자동화 등 다양한 분야에서 가장 인기 있는 언어 중 하나입니다.

## 특징

- **간결한 문법**: 들여쓰기 기반 블록 구조, 읽기 쉬운 코드
- **인터프리터 언어**: 컴파일 없이 바로 실행 가능, REPL 지원
- **동적 타이핑**: 변수 타입 선언 불필요 (3.6+ 타입 힌트 지원)
- **풍부한 표준 라이브러리**: "Batteries Included" 철학
- **방대한 생태계**: PyPI(60만+ 패키지), pip 패키지 매니저
- **멀티 패러다임**: 절차적, 객체 지향, 함수형 프로그래밍 모두 지원
- **GLI(Global Interpreter Lock)**: CPython의 동시성 제약, multiprocessing으로 극복

## 실행

```bash
cd PYTHON/01-hello-world && python main.py
```

## 목차

| # | 주제 | 설명 |
|---|------|------|
| 01 | Hello World | 기본 출력, 변수, 주석, 들여쓰기 |
| 02 | Variables | 변수, 숫자형, bool, None, 형변환 |
| 03 | Control Flow | if/elif/else, for, while, break/continue |
| 04 | Lists & Tuples | 리스트 메서드, 슬라이싱, 리스트 컴프리헨션 |
| 05 | Dicts & Sets | 딕셔너리, 세트, 컴프리헨션, 뷰 객체 |
| 06 | Functions | 함수 정의, 매개변수, lambda, closure, decorator |
| 07 | Strings | 문자열 메서드, 포맷팅, f-string, 정규표현식 |
| 08 | File I/O | open/close, with 문, read/write/csv/json |
| 09 | Exceptions | try/except/finally, raise, 사용자 정의 예외 |
| 10 | Modules & Packages | import, __name__, __init__.py, pip |
| 11 | OOP | 클래스, 인스턴스, self, @property |
| 12 | Inheritance | 상속, super, MRO, 다중 상속 |
| 13 | Decorators | @decorator, functools.wraps, 클래스 데코레이터 |
| 14 | Iterators & Generators | __iter__, __next__, yield, Generator Expression |
| 15 | Comprehensions | 리스트/딕셔너리/세트 컴프리헨션, 중첩 |
| 16 | Lambda & Map/Filter | lambda, map, filter, reduce |
| 17 | Date & Time | datetime, timedelta, time, strftime |
| 18 | JSON & APIs | json, requests, REST API, API 인증 |
| 19 | venv & pip | 가상환경, pip install, requirements.txt |
| 20 | Testing | unittest, pytest, fixture, mock |
