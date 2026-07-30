# 12: 태그와 릴리즈

## 태그

릴리즈 버전을 표시하는 참조입니다.

```bash
# Lightweight 태그 (주석 없음)
git tag v1.0.0

# Annotated 태그 (주석 포함, 권장)
git tag -a v1.0.0 -m "Release version 1.0.0"

# 특정 커밋에 태그
git tag -a v1.0.0 커밋해시 -m "Release 1.0.0"

# 태그 목록
git tag
git tag -l "v1.*"

# 태그 상세
git show v1.0.0
```

## 시맨틱 버저닝 (SemVer)

```
vMAJOR.MINOR.PATCH
v1.0.0   → 최초 릴리즈
v1.2.0   → 새로운 기능 (하위 호환)
v1.2.3   → 버그 수정
v2.0.0   → 하위 호환 깨짐
```

## 태그 푸시

```bash
# 특정 태그 푸시
git push origin v1.0.0

# 모든 태그 푸시
git push --tags
```

## GitHub Releases

GitHub 웹 UI에서 태그를 릴리즈로 전환 가능:
- 릴리즈 노트 작성
- 바이너리 파일 첨부 (.exe, .zip 등)
- 자동으로 zip/tarball 제공
