# 27: 데코레이터 심화 — 메서드/프로퍼티 데코레이터, DI 컨테이너

데코레이터(Decorator)는 클래스/메서드/프로퍼티/파라미터에 기능을 추가하는 함수입니다. Angular, NestJS, typeORM 등에서 의존성 주입(DI)을 구현하는 핵심입니다.

## 데코레이터 종류

- **클래스 데코레이터**: 클래스 생성자에 적용
- **메서드 데코레이터**: 메서드에 적용
- **프로퍼티/파라미터 데코레이터**: 속성과 파라미터에 적용

```typescript
function log(target: any, propertyKey: string, descriptor: PropertyDescriptor) {
  const original = descriptor.value;
  descriptor.value = function (...args: any[]) {
    console.log(`호출: ${propertyKey}(${args})`);
    return original.apply(this, args);
  };
}
```

## DI 컨테이너

데코레이터로 메타데이터를 수집하고, 리플렉션으로 생성자 파라미터를 주입하는 컨테이너를 만들 수 있습니다.

`index.ts`에서 실제 동작을 확인하세요.

## 실행

```bash
cd TYPESCRIPT/27-decorators-deep
npx ts-node index.ts
```

> 참고: 데코레이터는 `tsconfig.json`에 `"experimentalDecorators": true` 설정이 필요합니다.
