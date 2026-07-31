# 40: 종합 프로젝트 — Final Project

기초+중급 과정에서 배운 개념을 모두 활용하는 **콘솔 할일 관리 앱**을 만듭니다.
할일을 추가·조회·완료·삭제하고 **JSON 파일로 저장**해 앱을 재시작해도
데이터가 유지됩니다.

## 구현할 기능

- `add "내용"` — 할일 추가
- `list` — 전체 조회 (완료 여부 표시)
- `done <번호>` — 완료 처리
- `del <번호>` — 삭제
- `save` / `exit` — 파일 저장 및 종료

## 사용 기술

- 레코드(`record`) + `enum` — 도메인 모델
- `List<T>` + LINQ — 데이터 조작
- `System.Text.Json` — 파일 직렬화
- `Console` 상호작용 루프 — CLI
- `try-catch` — 오류 처리

```csharp
record Todo(int Id, string Title, bool IsDone);
var todos = JsonSerializer.Deserialize<List<Todo>>(json);
```

## 실행

```bash
dotnet run
```

명령 예시: `add 밀크 티 사기`, `list`, `done 1`, `del 2`, `exit`

## 핵심 요약

- 파일 저장(영속화)으로 데이터가 앱 수명을 넘어 유지됩니다.
- 작은 기능을 함수로 나누어 가독성과 재사용성을 확보합니다.
- 이 프로젝트를 확장해 예외 처리, 검색, 우선순위 등을 추가해 보세요.
