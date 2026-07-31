# 32: CI/CD 설계 — 파이프라인, 게이트, 아티팩트

CI/CD 파이프라인은 코드 변경을 자동으로 검증·빌드·배포하는 프로세스입니다.

## 단계 구성

1. **Lint/Format** — 코드 스타일 검사
2. **Unit Test** — 단위 테스트
3. **Build** — 산출물 생성
4. **Integration/E2E Test** — 통합 테스트
5. **Artifact 저장** — 빌드 산출물 보관
6. **Deploy** — 스테이징 → 승인 → 프로덕션

## 게이트 (Gate)

이전 단계가 통과해야 다음 단계 실행 (`needs`, `fail-fast`).

## 아티팩트 (Artifact)

빌드 결과물을 업로드해 다음 잡에서 재사용:

```yaml
- uses: actions/upload-artifact@v4
  with:
    name: build-output
    path: dist/
```

## 실행

```powershell
powershell -ExecutionPolicy Bypass -File demo.ps1
```
