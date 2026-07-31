# 39: 고급 테스팅 (Advanced Testing) — pytest fixture, parametrize, mock

## fixture
테스트에 필요한 사전 준비(setup)와 정리(teardown)를 재사용 가능한 함수로 정의합니다.

```python
import pytest

@pytest.fixture
def db():
    conn = sqlite3.connect(":memory:")
    yield conn          # 테스트에 주입
    conn.close()        # 정리
```

## parametrize
여러 입력 조합으로 같은 테스트를 반복 실행합니다.

```python
@pytest.mark.parametrize("a,b,expected", [(1,2,3), (0,0,0), (-1,1,0)])
def test_add(a, b, expected):
    assert add(a, b) == expected
```

## mock
외부 의존성(네트워크, 시간, 파일)을 가짜로 대체해 테스트를 결정적으로 만듭니다.

## 실행

```bash
python main.py
```

> pytest가 설치되어 있으면 `python -m pytest .` 로 전체 실행할 수 있습니다. 본 파일은 pytest 없이도 동작하도록 자체 실행기를 포함합니다.
