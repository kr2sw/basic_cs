# 20: 테스팅 (Testing) — unittest, pytest 기본

## assert 문
Python 내장 키워드로 조건이 참인지 검증합니다. `assert 조건, 메시지`

```python
assert 1 + 1 == 2
assert len([1, 2]) == 2, "길이가 2여야 함"
```

## unittest (표준 라이브러리)
`unittest.TestCase`를 상속받아 테스트 클래스를 만들고, `self.assert*` 메서드로 검증합니다.

## pytest (서드파티)
더 간결한 문법을 제공합니다. `assert`를 그대로 사용하고, 파일명이 `test_*.py`면 자동 발견됩니다.

## Fixtures
테스트 실행 전후에 필요한 설정(setUp)과 정리(tearDown)를 제공합니다. pytest에서는 `@pytest.fixture` 데코레이터로 정의합니다.

## Mocking
`unittest.mock.Mock` / `patch`를 사용하여 외부 의존성을 가짜로 대체합니다. 네트워크 호출, 파일 I/O 등을 테스트할 때 유용합니다.
