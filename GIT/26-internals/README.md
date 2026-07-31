# 26: Git 내부 — objects, refs, HEAD, packfiles

Git은 내용을 **객체(Object)** 로 저장합니다. 내부를 이해하면 Git 동작이 명확해집니다.

## 객체 4종

| 객체 | 설명 | 생성 |
|------|------|------|
| blob | 파일 내용 | `git hash-object` |
| tree | 디렉터리 구조 | `git mktree` |
| commit | 커밋 정보 (트리, 부모, 작성자) | `git commit-tree` |
| tag | 태그 포인터 | `git tag` |

## refs와 HEAD

- `.git/refs/heads/main`: 브랜치가 가리키는 커밋 SHA-1
- `.git/HEAD`: 현재 브랜치 (`ref: refs/heads/main`)

## hash-object 예시

```bash
echo "hello" | git hash-object --stdin
# 내용과 메타데이터를 SHA-1로 해시
```

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```
