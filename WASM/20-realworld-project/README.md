# 20: 실전 프로젝트 — WASM 이미지 필터

WASM을 사용하여 브라우저에서 이미지의 픽셀 데이터를 실시간 처리하는 그레이스케일 필터를 구현합니다.

## 프로젝트 구조

```
20-realworld-project/
├── index.html      # 메인 페이지
├── filter.wat      # WAT 소스 (WASM 이미지 필터)
└── filter.wasm     # 컴파일된 WASM (wat2wasm filter.wat -o filter.wasm)
```

## WASM 함수

| 함수 | 설명 |
|------|------|
| `grayscale` | RGB 픽셀을 그레이스케일로 변환 |
| `brightness` | 밝기 조절 |
| `invert` | 색상 반전 |
| `threshold` | 이진화 |

## 실행

```bash
wat2wasm filter.wat -o filter.wasm
npx http-server .
```

브라우저에서 이미지 파일을 선택하고 필터를 적용해보세요.
