# 29: 서브모듈/서브트리 — 추가, 업데이트, 권장 사례

프로젝트 안에 다른 저장소를 포함하는 두 가지 방법입니다.

## 서브모듈 (Submodule)

```bash
git submodule add <url> libs/shared
git submodule update --init --recursive   # 클론 후
git submodule foreach git pull            # 전체 업데이트
```

- 저장소 안에 다른 저장소의 특정 커밋을 가리키는 포인터
- 부모/자식 독립 버전 관리 가능
- 장점: 명시적 버전 고정 / 단점: 동기화 관리 필요

## 서브트리 (Subtree)

```bash
git subtree add --prefix=vendor/lib <url> main --squash
git subtree pull --prefix=vendor/lib <url> main
```

- 실제 파일이 부모 저장소에 복사됨
- 단일 저장소처럼 관리, 포인터 불필요

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```
