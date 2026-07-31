# 27: 고급 리베이스 — rerere, autosquash, fixup

리베이스(rebase)는 커밋 히스토리를 깨끗하게 정리하는 강력한 도구입니다.

## rerere (Reuse Recorded Resolution)

충돌 해결을 기록해뒀다가 같은 충돌이 재발하면 자동으로 다시 적용합니다.

```bash
git config --global rerere.enabled true
```

## autosquash + fixup

커밋 메시지가 `fixup! <제목>`으로 시작하면 `rebase -i --autosquash` 시 해당 커밋에 자동으로 합쳐집니다.

## 인터랙티브 리베이스

```bash
git rebase -i HEAD~3
# pick / squash / fixup / reword / drop 으로 히스토리 정리
```

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```
