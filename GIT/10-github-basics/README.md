# 10: GitHub 기초

## GitHub 저장소 만들기

1. GitHub 로그인 → **New repository**
2. 저장소 이름 입력
3. Public/Private 선택
4. README, .gitignore, license 선택 (선택)

## 로컬 → GitHub 연결

```bash
# 기존 저장소 연결
git remote add origin https://github.com/사용자/저장소.git
git branch -M main
git push -u origin main

# 또는 clone부터 시작
git clone https://github.com/사용자/저장소.git
cd 저장소
# 작업 후
git add . && git commit -m "메시지"
git push
```

## Issues

- 버그 리포트, 기능 요청, 할 일 관리
- 라벨: bug, enhancement, question, help wanted
- 마일스톤: 버전별 이슈 관리

## Fork

다른 사람의 저장소를 내 계정으로 복사 → 자유롭게 수정 → PR

```bash
# Fork한 저장소 clone
git clone https://github.com/내계정/저장소.git

# 원본 저장소를 upstream으로 추가
git remote add upstream https://github.com/원본/저장소.git
git fetch upstream
git merge upstream/main
```
