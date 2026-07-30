# 11: Pull Request

PR(Pull Request)은 변경 사항을 검토하고 병합하는 GitHub의 협업 방식입니다.

## PR 워크플로

```bash
# 1. 기능 브랜치 생성
git switch -c feature/new-feature

# 2. 작업 후 커밋
git add .
git commit -m "Add new feature"

# 3. 원격에 푸시
git push -u origin feature/new-feature

# 4. GitHub에서 PR 생성 (웹 UI)
#    - base: main ← compare: feature/new-feature
```

## PR 템플릿

```markdown
## 개요
이 PR이 해결하는 문제를 설명합니다.

## 변경 사항
- [x] 새로운 기능 추가
- [ ] 버그 수정
- [ ] 리팩토링

## 테스트
- [ ] 유닛 테스트 통과
- [ ] 수동 테스트 완료

## 관련 이슈
Closes #123
```

## 코드 리뷰

- **Comment**: 일반 의견
- **Approve**: 승인
- **Request Changes**: 수정 요청
- GitHub Actions로 자동 테스트/린트 체크 가능

## 머지 옵션

| 옵션 | 설명 |
|------|------|
| Create a merge commit | 병합 커밋 생성 (기본) |
| Squash and merge | 모든 커밋을 하나로 압축 |
| Rebase and merge | 커밋을 그대로 재배치 |
