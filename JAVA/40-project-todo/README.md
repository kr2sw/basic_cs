# 40: Final Project — 콘솔 기반 할일 관리 앱

## 프로젝트 소개

지금까지 배운 내용을 종합해 **콘솔 할일 관리 앱**을 만듭니다.

## 요구사항

- 할일 추가 / 목록 조회 / 완료 처리 / 삭제
- 우선순위(상/중/하)와 카테고리(업무, 개인, 학습...) 지원
- 완료 여부 필터링, 정렬
- 계층 구조로 설계 (Controller → Service → Repository)

## 설계

```
TodoController(메뉴/입출력)
    ↓
TodoService(비즈니스 로직)
    ↓
TodoRepository(데이터 저장)
    ↓
Todo (도메인 record)
```

## 구현 포인트

- `record Todo` 로 불변 도메인 모델
- `Optional` 로 조회 결과 처리
- `Stream` + `Comparator` 로 정렬/필터
- `enum Priority`, `enum Category` 로 상태 표현

## 실행

```bash
cd JAVA/40-project-todo
javac Main.java && java Main
```

인터렉티브 모드로 실행됩니다.

```bash
java Main demo
```

`demo` 인자를 주면 모든 기능을 자동으로 시연합니다.
