# 34: 성능 최적화 (Performance) — cProfile, timeit, lru_cache, 최적화 기법

## 측정 먼저
최적화의 첫 단계는 측정입니다. `timeit`은 짧은 코드 조각, `cProfile`은 전체 프로그램의 병목 지점을 찾습니다.

```python
import timeit
timeit.timeit(lambda: sum(range(1000)), number=10000)
```

## `functools.lru_cache`
결과를 캐시해 반복 계산을 줄입니다. 순수 함수(같은 입력 -> 같은 출력)에 적용하면 큰 효과가 있습니다. 대표적으로 재귀 함수에 사용합니다.

## 일반적인 최적화 기법
- 멤버십 검사: 리스트 대신 세트
- 전역 변수 대신 지역 변수
- 반복문 밖에서 속성/메서드 조회 줄이기
- 지연 평가(제너레이터)로 메모리 절약

## 실행

```bash
python main.py
```
