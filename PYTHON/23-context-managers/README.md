# 23: 컨텍스트 매니저 (Context Managers) — `contextlib`, `@contextmanager`, async context

## 컨텍스트 매니저란?
`with` 문과 함께 사용되어 리소스의 설정(setup)과 정리(cleanup)를 보장합니다. 파일, 네트워크 연결, 락 등을 안전하게 관리합니다.

```python
with open("data.txt") as f:
    data = f.read()
```

## `__enter__` / `__exit__`
클래스에 이 두 메서드를 구현하면 직접 컨텍스트 매니저를 만들 수 있습니다. `__exit__`에서 예외를 처리하거나 `False`를 반환해 예외를 전파할 수 있습니다.

## `@contextmanager` (contextlib)
`yield` 한 번으로 컨텍스트 매니저를 간단하게 만들 수 있습니다. `yield` 앞은 설정, 뒤는 정리 코드입니다.

## `ExitStack`
여러 컨텍스트 매니저를 동적으로 한 번에 관리합니다.

## 비동기 컨텍스트 매니저
`__aenter__` / `__aexit__`를 구현하면 `async with`로 사용할 수 있습니다.

## 실행

```bash
python main.py
```
