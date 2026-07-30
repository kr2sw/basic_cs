# 05: 되돌리기 — reset, restore, revert

## 변경 단계별 되돌리기

```bash
# Working Directory 변경 취소 (아직 add 안 함)
git restore 파일.txt
git checkout -- 파일.txt       # (구식)

# Staging 취소 (add만 하고 commit 안 함)
git restore --staged 파일.txt
git reset HEAD 파일.txt        # (구식)

# 마지막 커밋 수정 (메시지 변경/파일 추가)
git commit --amend

# Staging 취소 + WD 변경까지 취소
git reset --hard HEAD
```

## reset의 3가지 모드

```bash
git reset --soft HEAD~1    # 커밋만 취소, staging/WD 유지
git reset --mixed HEAD~1   # 커밋+staging 취소, WD 유지 (기본)
git reset --hard HEAD~1    # 전부 취소 (⚠️ 변경 사항 삭제)
```

## revert (안전한 되돌리기)

reset은 이력을 삭제하지만, revert는 새 커밋을 만듭니다.

```bash
git revert HEAD             # 최근 커밋 되돌리기
git revert 커밋해시          # 특정 커밋 되돌리기
git revert HEAD~3..HEAD     # 범위 되돌리기
```

⚠️ `reset --hard`는 변경 사항이 영구 삭제되므로 주의!
