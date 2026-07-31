# 40: 실전 프로젝트 — 고급 이미지 필터 앱

20장의 기본 필터를 확장해 **블러, 세피아, 대비, 엣지 검출**까지 포함한 완성형 이미지 필터 앱을 만듭니다. 픽셀 데이터는 WASM 메모리에서 처리되므로 각 필터는 `memory.copy` 없이 바로 연산합니다.

## 프로젝트 구조

```
40-final-project/
├── index.html      # 메인 페이지 (캔버스 + 슬라이더)
├── filter.wat      # WAT 소스 (필터 함수들)
└── filter.wasm     # wat2wasm filter.wat -o filter.wasm
```

## 기술 스택

| 구성 요소 | 역할 |
|-----------|------|
| Canvas 2D | 이미지 로드/표시, `getImageData`로 픽셀 추출 |
| `Uint8Array` | WASM 메모리와 JS 데이터를 복사 없이 공유 |
| WASM 메모리 | 픽셀(RGBA) 버퍼 + 블러 스크래치 버퍼 |
| `performance.now` | 필터별 처리 시간 측정 |

## 필터 함수

| 함수 | 설명 |
|------|------|
| `grayscale` | 픽셀을 흑백으로 (0.299R + 0.587G + 0.114B) |
| `sepia` | 세피아 톤 변환 (가중 합 + 클램프) |
| `brightness` | 밝기 델타 적용 (0~255 클램프) |
| `contrast` | 대비 계수 적용 (고정소수점 백분율) |
| `boxBlur` | 분리 가능한 박스 블러 (가로/세로 2패스) |
| `edgeDetect` | 차분 기반 엣지 검출 (임계값 이진화) |

## 필터 원리

```wat
;; 세피아: 각 채널을 가중치 합으로 변환 후 255로 클램프
R' = (R*393 + G*769 + B*189) / 1000
G' = (R*349 + G*686 + B*168) / 1000
B' = (R*272 + G*534 + B*131) / 1000
```

```wat
;; 대비: (v - 128) * factor / 100 + 128  (factor는 백분율)
(i32.add
  (i32.const 128)
  (i32.div_s
    (i32.mul (i32.sub (local.get $v) (i32.const 128)) (local.get $factor))
    (i32.const 100)))
```

## 실행

```bash
wat2wasm filter.wat -o filter.wasm
npx http-server .
```

브라우저에서 이미지를 선택하고 필터별 슬라이더를 조절해보세요. 슬라이더를 움직일 때마다 WASM 함수가 재호출되며 실시간으로 반영됩니다.
