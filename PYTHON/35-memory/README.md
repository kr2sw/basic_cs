# 35: 메모리 관리 (Memory) — gc, weakref, `__slots__`, 순환 참조

## 참조 카운트
CPython은 객체마다 참조 횟수를 세고 0이 되면 즉시 해제합니다. `sys.getrefcount()`로 확인할 수 있습니다.

## 순환 참조 (Circular Reference)
서로를 참조하는 객체는 참조 카운트가 0이 되지 않아 해제되지 않습니다. `gc` 모듈의 가비지 컬렉터가 이를 감지하고 수거합니다.

```python
import gc
gc.collect()
```

## `weakref`
강한 참조를 만들지 않는 참조입니다. 캐시나 콜백에 사용하며, 대상이 해제되면 자동으로 사라집니다.

## `__slots__`
인스턴스 `__dict__`를 만들지 않아 메모리를 크게 절약합니다.

## 실행

```bash
python main.py
```
