// 31: Express + TS 심화 — 타입 안전 라우터
// 실행: npm install express @types/express && npx ts-node index.ts
// (예제 코드는 express 미설치 환경에서도 타입 개념을 보여주도록 미니 라우터로 작성)

// === 1. 핸들러와 응답 타입 ===
interface UserDto {
  id: number;
  name: string;
  email: string;
}

interface TodoDto {
  id: number;
  title: string;
  completed: boolean;
}

type JsonHandler<T> = (req: FakeRequest) => T;

// === 2. 미니 라우터 (express Router 유사) ===
interface FakeRequest {
  params: Record<string, string>;
  query: Record<string, string>;
  body: unknown;
}

interface FakeResponse {
  statusCode: number;
  body: unknown;
  json(data: unknown): this;
  status(code: number): this;
}

interface Route {
  method: "GET" | "POST" | "PUT" | "DELETE";
  path: string;
  handler: (req: FakeRequest, res: FakeResponse) => void;
}

class Router {
  private routes: Route[] = [];

  get<const P extends string>(path: P, handler: (req: FakeRequest, res: FakeResponse) => void): this {
    this.routes.push({ method: "GET", path, handler });
    return this;
  }

  post(path: string, handler: (req: FakeRequest, res: FakeResponse) => void): this {
    this.routes.push({ method: "POST", path, handler });
    return this;
  }

  handle(method: string, url: string): { status: number; body: unknown } {
    const urlPath = url.split("?")[0];
    const route = this.routes.find(
      (r) => r.method === method && this.matchPath(r.path, urlPath)
    );

    if (!route) return { status: 404, body: { error: "Not Found" } };

    const params = this.extractParams(route.path, urlPath);
    const req: FakeRequest = { params, query: {}, body: {} };
    const res: FakeResponse = {
      statusCode: 200,
      body: null,
      json(data: unknown) {
        this.body = data;
        return this;
      },
      status(code: number) {
        this.statusCode = code;
        return this;
      },
    };

    route.handler(req, res);
    return { status: res.statusCode, body: res.body };
  }

  private matchPath(pattern: string, path: string): boolean {
    const p = pattern.replace(/:\w+/g, "[^/]+");
    return new RegExp(`^${p}$`).test(path);
  }

  private extractParams(pattern: string, path: string): Record<string, string> {
    const keys = [...pattern.matchAll(/:(\w+)/g)].map((m) => m[1]);
    const values = path.split("/").slice(pattern.split("/").length - keys.length);
    return Object.fromEntries(keys.map((k, i) => [k, values[i] ?? ""]));
  }
}

// === 3. 타입 안전 핸들러 ===
const users: UserDto[] = [
  { id: 1, name: "Alice", email: "a@e.com" },
  { id: 2, name: "Bob", email: "b@e.com" },
];

const todos: TodoDto[] = [
  { id: 1, title: "타입 공부", completed: false },
  { id: 2, title: "예제 작성", completed: true },
];

const router = new Router();

// GET /users/:id → UserDto | 404
router.get("/users/:id", (req, res) => {
  const user = users.find((u) => u.id === Number(req.params.id));
  if (!user) return res.status(404).json({ error: "사용자 없음" });
  res.json(user);
});

// GET /todos → TodoDto[]
router.get("/todos", (_req, res) => {
  res.json(todos.filter((t) => !t.completed));
});

// POST /todos → TodoDto
router.post("/todos", (req, res) => {
  const body = req.body as Partial<TodoDto>;
  const todo: TodoDto = { id: todos.length + 1, title: body.title ?? "새 할일", completed: false };
  todos.push(todo);
  res.status(201).json(todo);
});

// === 4. 실행 ===
console.log("GET /users/1 →", JSON.stringify(router.handle("GET", "/users/1")));
console.log("GET /users/99 →", JSON.stringify(router.handle("GET", "/users/99")));
console.log("GET /todos →", JSON.stringify(router.handle("GET", "/todos")));
console.log("GET /unknown →", JSON.stringify(router.handle("GET", "/unknown")));

// === 5. 타입 안전성 검증 (컴파일 시점) ===
type Expect<T extends true> = T;
type Equal<A, B> = (<G>() => G extends A ? 1 : 2) extends (<G>() => G extends B ? 1 : 2) ? true : false;

const sampleResponse = router.handle("GET", "/todos");
type ResponseBody = typeof sampleResponse.body;
type Test1 = Expect<Equal<ResponseBody, unknown>>;

console.log("\n타입 안전 라우터 데모 완료!");
