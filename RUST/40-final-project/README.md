# 40: 종합 프로젝트 — CLI 할일 관리 앱 (파일 저장)

중급 과정의 모든 개념을 종합한 최종 프로젝트입니다.

## 요구사항

- 파일 기반 영속 저장
- 커맨드 파싱 (ch38 재사용)
- 커스텀 에러 타입 (ch25)
- 반복자로 목록 처리 (ch23)
- 테스트 (ch37)

## 기능

- `add` 할 일 추가 (태그, 우선순위)
- `list` 목록 보기 (필터/정렬)
- `done` 완료 처리
- `remove` 삭제
- `stats` 통계

## 실행

```bash
cd RUST/40-final-project
cargo run -- add "Rust 복습" --priority high --tag study
cargo run -- list
cargo run -- list --done
cargo run -- done 1
cargo run -- stats
```
