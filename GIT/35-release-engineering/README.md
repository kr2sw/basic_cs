# 35: 릴리즈 엔지니어링 — SemVer, changelog, 태그, GitHub Releases

릴리즈는 코드를 버전으로 묶어 사용자에게 배포하는 과정입니다.

## SemVer (시맨틱 버전)

```
MAJOR.MINOR.PATCH
1.4.2
```

- **MAJOR**: 하위 호환 없는 변경
- **MINOR**: 하위 호환되는 기능 추가
- **PATCH**: 버그 수정

## 릴리즈 흐름

1. 버전 bump (`1.2.0` → `1.3.0`)
2. changelog 업데이트
3. 태그 생성 (`git tag v1.3.0`)
4. GitHub Releases 작성
5. 배포 트리거

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```
