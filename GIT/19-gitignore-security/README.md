# 19: .gitignore와 보안

## .gitignore 패턴

```gitignore
# 주석은 #으로 시작

# 특정 파일
secrets.json

# 디렉토리 전체
node_modules/

# 모든 .log 파일
*.log

# 예외 (!로 시작)
!important.log

# 특정 경로
/config/local.env

# glob 패턴
build/**/*.exe
doc/*.pdf
??.txt                 # 2글자 .txt 파일
[abc].txt              # a.txt, b.txt, c.txt
```

## 보안 주의사항

### 절대 커밋하지 말 것
```
.env, .env.local          # 환경 변수
*.key, *.pem, *.pfx       # 인증서/키
secrets.*                 # 시크릿
config.yml, appsettings.*.json  # (민감 정보 포함 시)
*.sql                     # (DB 덤프)
*.csv                     # (개인정보 포함 시)
```

### 실수로 커밋했을 때

```bash
# 민감 파일 삭제 (이력에서 완전 제거)
git filter-branch --force --index-filter \
  "git rm --cached --ignore-unmatch 파일명" \
  --prune-empty --tag-name-filter cat -- --all

# GitHub Secret Scanning에 의해 자동 감지
# git filter-repo 사용 권장
```

## .gitattributes

파일별 속성 설정 (줄바꿈, diff 등)

```gitattributes
# 텍스트 파일 LF 자동 변환
*.txt text eol=lf
*.cs text eol=crlf

# 바이너리 파일 diff 제외
*.dll binary
*.png binary

# 언어별 diff 설정
*.cs diff=csharp
*.py diff=python
```
