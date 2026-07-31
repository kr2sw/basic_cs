# 27: Reflection & Dynamic Proxy — 리플렉션과 동적 프록시

## Reflection (리플렉션)

런타임에 클래스의 구조(필드, 메서드, 생성자)를 분석하고 조작하는 기능입니다.

```java
Class<?> cls = obj.getClass();
Method m = cls.getMethod("methodName");
m.invoke(obj, args);          // 동적 메서드 호출
```

| 핵심 클래스 | 역할 |
|------------|------|
| `Class<?>` | 클래스 메타데이터 |
| `Method` | 메서드 호출/정보 |
| `Field` | 필드 읽기/쓰기 |
| `Constructor<?>` | 생성자 호출 |
| `Modifier` | 접근 제어자 확인 |

`setAccessible(true)` 로 private 요소에도 접근할 수 있습니다.

## 동적 프록시 (Dynamic Proxy)

인터페이스의 구현을 런타임에 만들어 로깅/권한/트랜잭션 같은 부가 기능을 주입합니다.

```java
Object proxy = Proxy.newProxyInstance(
    cl, new Class<?>[]{ SomeInterface.class },
    (obj, method, args) -> {
        System.out.println("호출 전");
        Object result = method.invoke(target, args);
        return result;
    });
```

## InvocationHandler

프록시에 실제로 요청이 들어왔을 때 실행되는 핸들러입니다.

```java
public Object invoke(Object proxy, Method method, Object[] args) {
    // 부가 로직 -> 대상 메서드 호출
}
```

- 프록시 객체에 가로채기(AOP) 같은 횡단 관심사를 구현
- 스프링 AOP 의 핵심 원리

## 주의사항

- 리플렉션은 정적 호출보다 느림 (성능 민감한 경로에서 지양)
- 캡슐화를 깨므로 보안/유지보수에 주의

## 실행

```bash
cd JAVA/27-reflection-proxies
javac Main.java && java Main
```
