# 40: 종합 프로젝트 — 타입 안전 할일 관리 CLI

중급 과정의 모든 개념(제네릭, 조건부 타입, DTO, 상태 관리, 테스트)을 결합한 최종 프로젝트입니다.

## 프로젝트 개요

파일 기반으로 동작하는 **할일 관리 CLI**를 만듭니다.

- `add "할일 제목"` — 할일 추가
- `list` — 전체 목록 (완료 여부 포함)
- `done <id>` — 완료 처리
- `remove <id>` — 삭제
- `stats` — 통계 (전체/완료/진행률)

## 적용된 중급 개념

- `interface`/`type` 기반 도메인 모델
- `readonly` 불변 데이터, DTO 분리
- 상태 전환을 타입으로 제한
- 제네릭 저장소(Repository)
- 타입 검증 유틸리티

## 실행

```bash
cd TYPESCRIPT/40-final-project
npx ts-node index.ts
```
