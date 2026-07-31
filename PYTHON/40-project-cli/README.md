# 40: 미니 프로젝트 (Mini Project) — 명령줄 할일 관리 앱

## 프로젝트 소개
argparse + JSON 파일 저장으로 동작하는 할일(todo) 관리 CLI 앱입니다. 지금까지 배운 내용을 종합합니다.

## 사용법

```bash
python main.py add "파이썬 공부"
python main.py list
python main.py done 1
python main.py delete 1
python main.py list --all
```

## 설계 포인트
- `argparse`: 하위 명령(subcommand) 구조 (`add`, `list`, `done`, `delete`)
- 데이터 저장: `todos.json` 파일 (없으면 자동 생성, 실패하면 메모리만 사용)
- 데이터 구조: `{"id": int, "task": str, "done": bool}`
- `DoneStatus` 커스텀 액션: 완료 여부를 사람이 읽기 좋게 출력

## 배운 것의 활용
`dataclass`, `json`, `pathlib`, 예외 처리, 리스트 컴프리헨션, 타입 힌트까지 모두 사용합니다.

## 실행

```bash
python main.py
```
