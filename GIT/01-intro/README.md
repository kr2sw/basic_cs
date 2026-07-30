# 01: Git 소개와 저장소 만들기

Git은 분산 버전 관리 시스템(DVCS)으로, 파일 변경 이력을 추적하고 협업을 지원합니다.

## 주요 개념

- **Repository (저장소)**: 파일과 이력을 저장하는 공간
- **Commit (커밋)**: 변경 사항의 스냅샷
- **Working Directory**: 실제 작업 중인 파일들
- **Staging Area**: 커밋할 준비가 된 파일들

## 명령어

```bash
# 새 저장소 만들기
git init

# 저장소 복제
git clone <url>

# 상태 확인
git status

# 파일 추가 (staging)
git add 파일명
git add .        # 전체 추가

# 커밋
git commit -m "메시지"

# 전체 흐름
echo "# My Project" > README.md
git add README.md
git commit -m "Initial commit"
```
