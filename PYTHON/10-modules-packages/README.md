# 10: Modules & Packages — 모듈, 패키지, pip, __name__

## import
다른 파이썬 파일(.py)의 코드를 가져와 사용합니다.

```python
import math
from datetime import datetime
from os import path as osp
```

## __name__ == '__main__'
파일이 직접 실행될 때만 특정 코드가 실행되도록 합니다.

```python
if __name__ == '__main__':
    # 이 파일을 직접 실행했을 때만 실행
    main()
```

## pip
패키지 관리자로 외부 라이브러리를 설치합니다.
```
pip install requests
```

## 모듈 생성
`.py` 파일 자체가 모듈입니다. 폴더에 `__init__.py`를 넣으면 패키지가 됩니다.

## os / sys 기초
- `os.getcwd()`, `os.listdir()`, `os.path.join()`
- `sys.argv` (명령줄 인자), `sys.exit()`, `sys.path`
