// 34: React 제네릭 — 다형성 컴포넌트, 제네릭 훅
// (실제 React 렌더링 없이 타입 구조를 모델링합니다)

type Expect<T extends true> = T;
type Equal<A, B> = (<G>() => G extends A ? 1 : 2) extends (<G>() => G extends B ? 1 : 2) ? true : false;

// === 1. useState 제네릭 ===
type UseState<T> = [T, (next: T | ((prev: T) => T)) => void];

function useState<T>(initial: T): UseState<T> {
  let value = initial;
  const setter = (next: T | ((prev: T) => T)) => {
    value = typeof next === "function" ? (next as (p: T) => T)(value) : next;
  };
  return [value, setter];
}

const [count, setCount] = useState<number>(0);
const [name, setName] = useState<string>("");
type CountState = typeof count;  // number
type NameState = typeof name;    // string

// === 2. 제네릭 훅: useApi ===
interface ApiResult<T> {
  data: T | null;
  loading: boolean;
  error: string | null;
}

function useApi<T>(fetcher: () => T): ApiResult<T> {
  try {
    return { data: fetcher(), loading: false, error: null };
  } catch (e) {
    return { data: null, loading: false, error: (e as Error).message };
  }
}

interface User { id: number; name: string; }
const { data: userData } = useApi<User>(() => ({ id: 1, name: "Alice" }));
type UserData = typeof userData;  // User | null

// === 3. 제네릭 훅: useLocalStorage 유사 ===
function usePersistentState<T>(key: string, initial: T): [T, (v: T) => void] {
  const [value, set] = useState<T>(initial);
  return [value, (v: T) => set(v)];
}

// === 4. 다형성 컴포넌트 모델링 ===
// React: function Box<C extends ElementType>({ as, ...props }: { as: C } & ComponentPropsWithoutRef<C>)
interface PolymorphicProps<C extends string> {
  as: C;
  children: string;
}

function createElement<C extends string>(props: PolymorphicProps<C>): string {
  const { as, children } = props;
  return `<${as}>${children}</${as}>`;
}

const div = createElement({ as: "div", children: "안녕" });
const span = createElement({ as: "span", children: "타입" });
console.log("다형성 컴포넌트:", div, "|", span);

// === 5. 제네릭 컴포넌트 props (Compound) ===
interface ListProps<T> {
  items: T[];
  renderItem: (item: T, index: number) => string;
}

function renderList<T>(props: ListProps<T>): string {
  return props.items.map((item, i) => props.renderItem(item, i)).join("\n");
}

const html = renderList<User>([
  { id: 1, name: "A" },
  { id: 2, name: "B" },
], (user) => `- ${user.name} (#${user.id})`);
console.log("제네릭 리스트:\n" + html);

// === 6. 이벤트 핸들러 제네릭 ===
type ChangeHandler<T extends HTMLElement> = (target: T) => void;
const onInput: ChangeHandler<HTMLInputElement> = (el) => console.log("input:", el.value);
// const onWrong: ChangeHandler<HTMLInputElement> = (el) => console.log(el.href);  // Error

// === 타입 검증 ===
type T1 = Expect<Equal<CountState, number>>;
type T2 = Expect<Equal<NameState, string>>;
type T3 = Expect<Equal<UserData, User | null>>;

console.log("\n모든 타입 검증 통과!");
