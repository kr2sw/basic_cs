# 28: bisect 디버깅 — 이진 탐색, 자동 실행

`git bisect`는 커밋을 이진 탐색하여 **버그를 처음 도입한 커밋**을 빠르게 찾아냅니다.

## 기본 사용법

```bash
git bisect start
git bisect bad          # 현재 커밋이 버그 있음
git bisect good <sha>   # 알고 있는 정상 커밋
# 중간 커밋에서 테스트 후 bad/good 결정 → 반복
git bisect reset        # 원래 상태로 복귀
```

## 자동 실행

```bash
git bisect run <테스트 명령>
# exit 0 → good, exit 1 → bad 로 자동 판정
```

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```
