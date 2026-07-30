# 15: Cherry-pick과 Revert

## Cherry-pick

특정 커밋만 골라서 현재 브랜치에 적용합니다.

```bash
# 특정 커밋 하나만 가져오기
git cherry-pick a1b2c3d

# 여러 커밋 가져오기
git cherry-pick a1b2c3d e5f6g7h

# 범위 가져오기
git cherry-pick a1b2c3d..e5f6g7h

# 커밋 메시지 수정
git cherry-pick --edit a1b2c3d

# cherry-pick 중 충돌 해결
git cherry-pick --continue   # 충돌 해결 후 계속
git cherry-pick --abort      # 취소
git cherry-pick --skip       # 해당 커밋 스킵
```

## 사용 사례

- 핫픽스를 release 브랜치에만 적용
- 특정 기능만 다른 브랜치로 이동
- 실수로 잘못된 브랜치에 커밋했을 때

## Revert (재수행)

```bash
# 커밋 되돌리기 (새 커밋 생성)
git revert a1b2c3d

# revert 되돌리기
git revert a1b2c3d        # 원래 커밋 revert
git revert $(git rev-parse a1b2c3d)  # 또는 이렇게
```
