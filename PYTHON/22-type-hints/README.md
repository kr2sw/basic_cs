# 22: 타입 힌트 (Type Hints) — TypedDict, Protocol, NewType, mypy

## 기본 타입 힌트
`int`, `str`, `List[int]`, `Optional[str]`, `Union`, `Dict[str, int]` 등으로 매개변수와 반환값의 타입을 명시합니다. 힌트는 런타임 동작을 바꾸지 않고 정적 분석 도구(mypy)가 검사합니다.

```python
def add(a: int, b: int) -> int:
    return a + b
```

## `TypedDict`
딕셔너리의 키와 값 타입을 사전처럼 정의합니다. 특히 JSON 데이터를 다룰 때 유용합니다.

## `NewType`
기본 타입과 구별되는 새 타입을 만들어 실수로 섞이는 것을 막습니다.

## `Protocol`
구조적 서브타이핑을 지원합니다. 상속 없이 특정 메서드/속성만 가진 객체면 타입으로 인정합니다. "덕 타이핑"을 정적으로 검사합니다.

## mypy 체크
`mypy main.py` 명령으로 타입 오류를 검사합니다. `# type: ignore` 주석으로 특정 줄의 검사를 건너뛸 수 있습니다.

## 실행

```bash
python main.py
```
