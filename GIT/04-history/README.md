# 04: 변경 이력 관리 — log, diff, blame

## 커밋 로그 보기

```bash
git log                        # 전체 로그
git log --oneline              # 한 줄로
git log --oneline --graph      # 브랜치 그래프
git log -5                     # 최근 5개
git log --since="2024-01-01"   # 날짜 필터
git log --author="이름"        # 작성자 필터
git log --grep="버그"          # 메시지 검색
git log -- 파일명              # 특정 파일 로그
```

## 변경 내용 보기

```bash
git diff HEAD~..HEAD           # 최근 커밋 변경
git diff 커밋해시1..커밋해시2  # 두 커밋 비교
git show 커밋해시              # 특정 커밋 상세
git show HEAD                  # 최근 커밋 상세
```

## 파일 별명 추적

```bash
git blame 파일.txt              # 각 줄의 마지막 수정자
git blame -L 10,20 파일.txt     # 10~20번째 줄만
```

## 포맷 지정

```bash
git log --pretty=format:"%h %an %ar %s"
# %h: 짧은 해시, %an: 작성자, %ar: 상대적 시간, %s: 제목
```
