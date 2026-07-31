# 31: 히스토리 편집 — filter-branch, filter-repo, 대규모 수정

과거 커밋을 일괄 수정할 때 사용하는 도구입니다. `git filter-repo`가 권장되며 `filter-branch`는 레거시입니다.

## filter-repo

```bash
pip install git-filter-repo
git filter-repo --path secret.txt --invert-paths   # 파일 삭제
git filter-repo --path-glob '*.log' --invert-paths # 확장자 삭제
git filter-repo --email-callback '...'             # 이메일 변경
```

- 히스토리 전체를 다시 작성하므로 **원격 저장소를 지우고 재생성**해야 합니다.
- 혼자 작업하는 브랜치에만 사용하세요.

## 시크릿 제거 절차

1. 필터 도구로 히스토리에서 제거
2. 원격 저장소에서 시크릿을 revoke/교체
3. 팀원에게 새 클론 권장

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```
