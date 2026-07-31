// 39: 타입 안전 이벤트와 상태 머신 — 이벤트 맵, FSM

type Expect<T extends true> = T;
type Equal<A, B> = (<G>() => G extends A ? 1 : 2) extends (<G>() => G extends B ? 1 : 2) ? true : false;

// === 1. 타입 안전 이벤트 맵 ===
type EventMap = {
  userLogin: { userId: number; name: string };
  message: { room: string; text: string };
  connectionChange: { online: boolean };
};

type EventName = keyof EventMap;

// === 2. 제네릭 이벤트 버스 ===
class EventBus<M extends Record<string, unknown>> {
  private listeners = new Map<keyof M, Array<(payload: never) => void>>();

  on<K extends keyof M>(event: K, handler: (payload: M[K]) => void): this {
    const list = this.listeners.get(event) ?? [];
    list.push(handler as (payload: never) => void);
    this.listeners.set(event, list);
    return this;
  }

  emit<K extends keyof M>(event: K, payload: M[K]): void {
    const list = this.listeners.get(event) ?? [];
    for (const handler of list) handler(payload as never);
  }
}

const bus = new EventBus<EventMap>();

bus.on("userLogin", (payload) => {
  // payload: { userId: number; name: string } 자동 추론
  console.log(`로그인: ${payload.name} (${payload.userId})`);
});
bus.on("message", (payload) => {
  console.log(`[${payload.room}] ${payload.text}`);
});

bus.emit("userLogin", { userId: 1, name: "Alice" });
bus.emit("message", { room: "chat", text: "안녕하세요!" });
// bus.emit("userLogin", { userId: "x" });  // Error: 타입 불일치

// === 3. 상태 머신 (FSM) 정의 ===
type FetchState = "idle" | "loading" | "success" | "error";
type FetchEvent = { type: "FETCH" } | { type: "RESOLVE"; data: string } | { type: "REJECT"; error: string } | { type: "RETRY" };

type Transitions = {
  idle: "FETCH";
  loading: "RESOLVE" | "REJECT";
  success: "FETCH";
  error: "RETRY" | "FETCH";
};

// === 4. 타입 안전 상태 머신 ===
type AllowedEvent<S extends FetchState> = Transitions[S];

class FetchMachine {
  private _state: FetchState = "idle";
  data: string | null = null;
  error: string | null = null;

  get state(): FetchState {
    return this._state;
  }

  dispatch<E extends FetchEvent>(event: E): void {
    // 잘못된 전이는 타입 수준에서 차단하되, 런타임에서도 검증
    const allowed = this.allowedEvents(this._state);
    if (!allowed.includes(event.type)) {
      throw new Error(`상태 ${this._state}에서 ${event.type} 불가`);
    }

    switch (event.type) {
      case "FETCH":
        this._state = "loading";
        break;
      case "RESOLVE":
        this._state = "success";
        this.data = event.data;
        break;
      case "REJECT":
        this._state = "error";
        this.error = event.error;
        break;
      case "RETRY":
        this._state = "idle";
        break;
    }
    this.log();
  }

  private allowedEvents(state: FetchState): FetchEvent["type"][] {
    const map: Record<FetchState, FetchEvent["type"][]> = {
      idle: ["FETCH"],
      loading: ["RESOLVE", "REJECT"],
      success: ["FETCH"],
      error: ["RETRY", "FETCH"],
    };
    return map[state];
  }

  private log(): void {
    console.log(`  상태: ${this._state}` + (this.data ? ` (데이터: ${this.data})` : "") + (this.error ? ` (오류: ${this.error})` : ""));
  }
}

// === 5. FSM 실행 ===
console.log("FSM 시나리오:");
const machine = new FetchMachine();
machine.dispatch({ type: "FETCH" });
machine.dispatch({ type: "RESOLVE", data: "안녕" });
machine.dispatch({ type: "FETCH" });
machine.dispatch({ type: "REJECT", error: "네트워크 오류" });
machine.dispatch({ type: "RETRY" });

try {
  machine.dispatch({ type: "RESOLVE", data: "x" });  // idle에서 RESOLVE는 불가
} catch (e) {
  console.log("잘못된 전이 차단:", (e as Error).message);
}

// === 6. 상태별 타입 전환 (discriminated union) ===
type LoadingState = { status: "loading"; progress: number };
type DoneState = { status: "done"; result: string };
type AsyncState = LoadingState | DoneState;

function render(state: AsyncState): string {
  switch (state.status) {
    case "loading": return `로딩 중... ${state.progress}%`;
    case "done": return `완료: ${state.result}`;
  }
}

console.log("\n렌더:", render({ status: "loading", progress: 50 }));
console.log("렌더:", render({ status: "done", result: "성공" }));

// === 타입 검증 ===
type T1 = Expect<Equal<AllowedEvent<"loading">, { type: "RESOLVE"; data: string } | { type: "REJECT"; error: string }>>;

console.log("\n타입 안전 이벤트/FSM 데모 완료!");
