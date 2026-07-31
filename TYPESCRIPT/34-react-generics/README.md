# 34: React 제네릭 — 다형성 컴포넌트, 제네릭 훅

React 컴포넌트에 제네릭을 사용하면 **다형성(Polymorphic) 컴포넌트**와 **재사용 가능한 훅**을 만들 수 있습니다.

## 제네릭 훅

```typescript
function useApi<T>(fetcher: () => Promise<T>) {
  const [data, setData] = useState<T | null>(null);
  // ...
  return { data, loading };
}
```

## 다형성 컴포넌트

`as` prop을 받아 렌더링 태그를 바꾸면서도 타입을 유지하는 컴포넌트:

```typescript
function Box<C extends React.ElementType>({ as, children }: { as: C } & ComponentPropsWithoutRef<C>) {
  const Tag = as;
  return <Tag>{children}</Tag>;
}
```

`index.ts`에서 제네릭 훅과 컴포넌트 타입을 정의하고 검증합니다.

## 실행

```bash
cd TYPESCRIPT/34-react-generics
npx ts-node index.ts
```
