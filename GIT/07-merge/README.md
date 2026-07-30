# 07: 브랜치 병합 — merge

## Fast-Forward Merge

브랜치가 직선상에 있을 때, 단순히 포인터를 이동합니다.

```bash
git switch main
git merge feature     # fast-forward 가능
```

## 3-Way Merge

두 브랜치가 각각 다른 커밋을 가질 때, 새 병합 커밋을 만듭니다.

```bash
git switch main
git merge feature     # 3-way merge → 병합 커밋 생성
```

## 병합 옵션

```bash
# 항상 병합 커밋 생성 (fast-forward여도)
git merge --no-ff feature

# 병합 커밋 하나로 squash
git merge --squash feature
# (직접 커밋 필요) git commit -m "Add feature"

# 하나의 브랜치로 rebase 후 merge
git rebase main
git switch main
git merge feature
```

## 병합 취소

```bash
git merge --abort      # 병합 충돌 시 취소
```
