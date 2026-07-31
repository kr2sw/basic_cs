// 40: 종합 프로젝트 — 타입 안전 할일 관리 CLI

// === 1. 도메인 모델 ===
interface Todo {
  readonly id: number;
  title: string;
  completed: boolean;
  readonly createdAt: Date;
}

type CreateTodoDTO = Pick<Todo, "title">;
type TodoStatus = Todo["completed"] extends true ? "완료" : "진행 중";

// === 2. 상태 전이 타입 ===
// 완료된 할일만 다시 미완료로 되돌릴 수 있다는 규칙을 타입으로 표현
type ToggleTodo<T extends Todo> = T extends { completed: true }
  ? { completed: false }
  : { completed: true };

// === 3. 제네릭 저장소 ===
class Repository<T extends { id: number }> {
  protected items: T[] = [];
  private nextId = 1;

  insert(item: Omit<T, "id">): T {
    const row = { ...(item as object), id: this.nextId++ } as T;
    this.items.push(row);
    return row;
  }

  findAll(): readonly T[] {
    return this.items;  // 외부 수정 불가 (readonly)
  }

  findById(id: number): T | undefined {
    return this.items.find((i) => i.id === id);
  }

  update(id: number, patch: Partial<T>): T | undefined {
    const idx = this.items.findIndex((i) => i.id === id);
    if (idx === -1) return undefined;
    const updated = { ...this.items[idx], ...patch, id };
    this.items[idx] = updated;
    return updated;
  }

  remove(id: number): boolean {
    const idx = this.items.findIndex((i) => i.id === id);
    if (idx === -1) return false;
    this.items.splice(idx, 1);
    return true;
  }
}

class TodoRepository extends Repository<Todo> {
  getCompletedCount(): number {
    return this.items.filter((t) => t.completed).length;
  }

  toggle(id: number): Todo | undefined {
    const todo = this.findById(id);
    if (!todo) return undefined;
    const patch: Partial<Todo> = { completed: !todo.completed };
    return this.update(id, patch);
  }
}

// === 4. CLI 앱 ===
class TodoApp {
  private repo = new TodoRepository();

  add(title: string): void {
    const todo = this.repo.insert({ title: title.trim(), completed: false, createdAt: new Date() });
    console.log(`할일 추가됨 [#${todo.id}] ${todo.title}`);
  }

  list(): void {
    const todos = this.repo.findAll();
    if (todos.length === 0) {
      console.log("할일이 없습니다.");
      return;
    }
    console.log("=== 할일 목록 ===");
    for (const todo of todos) {
      const mark = todo.completed ? "✓" : "•";
      const status: TodoStatus = todo.completed ? "완료" : "진행 중";
      console.log(`  ${mark} [#${todo.id}] ${todo.title} (${status})`);
    }
  }

  done(id: number): void {
    const todo = this.repo.toggle(id);
    if (!todo) console.log(`할일 #${id}을(를) 찾을 수 없습니다.`);
    else console.log(`할일 #${id} 상태 변경: ${todo.completed ? "완료" : "진행 중"}`);
  }

  remove(id: number): void {
    console.log(this.repo.remove(id) ? `할일 #${id} 삭제됨` : `할일 #${id} 없음`);
  }

  stats(): void {
    const total = this.repo.findAll().length;
    const done = this.repo.getCompletedCount();
    const rate = total === 0 ? 0 : Math.round((done / total) * 100);
    console.log(`=== 통계 ===`);
    console.log(`전체: ${total}, 완료: ${done}, 진행률: ${rate}%`);
  }
}

// === 5. 데모 실행 (CLI 명령 시뮬레이션) ===
const app = new TodoApp();

// 실제 CLI처럼 인자 처리
const simulate = (input: string) => {
  console.log(`\n$ todo ${input}`);
  const [cmd, arg] = input.split(/\s+(.+)/);
  switch (cmd) {
    case "add": app.add(arg ?? ""); break;
    case "list": app.list(); break;
    case "done": app.done(Number(arg)); break;
    case "remove": app.remove(Number(arg)); break;
    case "stats": app.stats(); break;
    default: console.log("알 수 없는 명령");
  }
};

simulate("add TypeScript 중급 완료하기");
simulate("add README 작성");
simulate("add 데모 테스트");
simulate("list");
simulate("done 1");
simulate("list");
simulate("done 99");   // 없는 ID 처리
simulate("remove 2");
simulate("stats");

// === 6. 타입 검증 ===
type Expect<T extends true> = T;
type Equal<A, B> = (<G>() => G extends A ? 1 : 2) extends (<G>() => G extends B ? 1 : 2) ? true : false;

type DoneTodo = Todo & { completed: true };
type T1 = Expect<Equal<ToggleTodo<DoneTodo>, { completed: false }>>;
type T2 = Expect<Equal<CreateTodoDTO, { title: string }>>;

console.log("\n타입 검증 통과 — 프로젝트 완성!");
