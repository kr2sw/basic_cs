# 36: 보안 — 서명 커밋(GPG), 시크릿 스캔, credential

Git/GitHub 보안 모범 사례를 배웁니다.

## 서명 커밋 (GPG)

```bash
gpg --gen-key                       # 키 생성
git config user.signingkey <KEY>
git commit -S -m "서명 커밋"
git config commit.gpgsign true      # 항상 서명
```

GitHub에 공개 키를 등록하면 커밋에 "Verified" 배지가 표시됩니다.

## 시크릿 스캔

- GitHub Secret Scanning: 저장소에 커밋된 시크릿 자동 감지
- `gh secret-scanning` 등으로 확인
- 커밋 전 `.gitignore`와 pre-commit 훅으로 사전 차단

## Credential 관리

- `git credential-manager` 사용 (Windows 기본)
- 개인 액세스 토큰(PAT)은 범위를 최소화하고 만료일 설정
- 절대 커밋에 토큰/비밀번호를 포함하지 말 것

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```
