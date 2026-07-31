"""
40: 미니 프로젝트 — 명령줄 할일 관리 앱 (argparse + JSON 저장)
사용법:
    python main.py add "파이썬 공부"
    python main.py list
    python main.py done 1
    python main.py delete 1
    python main.py list --all
"""
import argparse
import json
import sys
from dataclasses import dataclass, asdict, field
from pathlib import Path

DATA_FILE = Path(__file__).parent / "todos.json"


@dataclass
class Todo:
    id: int
    task: str
    done: bool = False


class TodoStore:
    """JSON 파일에 할일을 저장/불러옵니다."""
    def __init__(self, path: Path = DATA_FILE):
        self.path = path
        self.todos: list[Todo] = self._load()

    def _load(self) -> list[Todo]:
        try:
            data = json.loads(self.path.read_text(encoding="utf-8"))
            return [Todo(**item) for item in data]
        except (FileNotFoundError, json.JSONDecodeError):
            return []

    def save(self):
        data = [asdict(t) for t in self.todos]
        try:
            self.path.write_text(
                json.dumps(data, ensure_ascii=False, indent=2),
                encoding="utf-8",
            )
        except OSError:
            print("  [경고] 저장 실패 - 파일 대신 메모리에만 보관합니다.")

    def next_id(self) -> int:
        return (max((t.id for t in self.todos), default=0)) + 1

    def add(self, task: str) -> Todo:
        todo = Todo(id=self.next_id(), task=task)
        self.todos.append(todo)
        self.save()
        return todo

    def mark_done(self, todo_id: int) -> bool:
        for todo in self.todos:
            if todo.id == todo_id:
                todo.done = True
                self.save()
                return True
        return False

    def delete(self, todo_id: int) -> bool:
        before = len(self.todos)
        self.todos = [t for t in self.todos if t.id != todo_id]
        if len(self.todos) != before:
            self.save()
            return True
        return False

    def list_all(self, include_done: bool = True):
        for todo in self.todos:
            if include_done or not todo.done:
                yield todo


def format_todo(todo: Todo) -> str:
    mark = "✓" if todo.done else " "
    return f"[{mark}] #{todo.id} {todo.task}"


def build_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(prog="todo", description="할일 관리 CLI 앱")
    sub = parser.add_subparsers(dest="command", required=True)

    p_add = sub.add_parser("add", help="할일 추가")
    p_add.add_argument("task", help="할일 내용")

    sub.add_parser("list", help="목록 보기").add_argument(
        "--all", action="store_true", help="완료한 항목도 표시")

    p_done = sub.add_parser("done", help="완료 처리")
    p_done.add_argument("id", type=int, help="할일 번호")

    p_del = sub.add_parser("delete", help="할일 삭제")
    p_del.add_argument("id", type=int, help="할일 번호")

    return parser


def main(argv=None) -> int:
    args = build_parser().parse_args(argv)
    store = TodoStore()

    if args.command == "add":
        todo = store.add(args.task)
        print(f"추가됨: {format_todo(todo)}")

    elif args.command == "list":
        todos = list(store.list_all(include_done=args.all))
        if not todos:
            print("할일이 없습니다. 'todo add ...'로 추가하세요.")
            return 0
        for todo in todos:
            print(format_todo(todo))
        done = sum(1 for t in todos if t.done)
        print(f"\n총 {len(todos)}건, 완료 {done}건")

    elif args.command == "done":
        if store.mark_done(args.id):
            print(f"#{args.id} 완료 처리되었습니다.")
        else:
            print(f"#{args.id} 번호가 없습니다.")
            return 1

    elif args.command == "delete":
        if store.delete(args.id):
            print(f"#{args.id} 삭제되었습니다.")
        else:
            print(f"#{args.id} 번호가 없습니다.")
            return 1

    return 0


if __name__ == "__main__":
    demo = ["add", "파이썬 중급 강의 듣기"]
    demo += ["add", "복습하기"]
    print("=== 데모 실행 ===")
    main(demo)
    main(["done", "1"])
    main(["list"])
    main(["delete", "2"])
    main(["list", "--all"])
    print("\n직접 사용해 보세요:")
    print('  python main.py add "할일 내용"')
    print("  python main.py list --all")
    sys.exit(0)
