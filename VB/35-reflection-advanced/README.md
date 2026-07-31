# 35: 고급 리플렉션 — Type, Activator, 동적 호출

## 소개

실행 중(runtime)에 타입 정보를 조사하고 객체를 만들고 메서드를 호출하는 리플렉션(Reflection)을 심화합니다. DI 컨테이너, ORM, 테스트 러너, 플러그인 시스템의 기반 기술입니다.

## 주요 개념

### 1. Type — 타입 정보 조사

`GetType()`으로 타입을 얻고 멤버를 열거합니다.

```vb
Dim t As Type = GetType(Car)
For Each p In t.GetProperties()
    Console.WriteLine($"{p.PropertyType.Name} {p.Name}")
Next
```

### 2. Activator — 동적 객체 생성

이름으로 찾은 타입을 실행 중에 인스턴스화합니다. 생성자 인자를 배열로 전달할 수 있습니다.

```vb
Dim car = Activator.CreateInstance(t, "소나타", 0)
```

### 3. MethodInfo.Invoke — 동적 호출

메서드 정보를 얻어 인자를 넘겨 실행합니다.

```vb
Dim accelerate = t.GetMethod("Accelerate")
accelerate.Invoke(car, {50})
Dim speed = t.GetProperty("Speed").GetValue(car)
```

### 4. CallByName — VB 전용 동적 호출

속성/메서드를 문자열 이름으로 접근합니다. `CallType`으로 Get/Set/Method를 구분합니다.

```vb
CallByName(car, "Speed", CallType.Set, 100)
Dim speed = CallByName(car, "Speed", CallType.Get)
CallByName(car, "Honk", CallType.Method)
```

### 5. 사용자 정의 특성(Attribute) 읽기

`GetCustomAttribute(Of T)()`로 메타데이터를 읽습니다.

```vb
Dim attr = t.GetCustomAttribute(Of DemoInfoAttribute)()
```

## 실행

```bash
dotnet run
```

## 정리

- `Type` = 타입 정보, `Activator` = 객체 생성, `MethodInfo.Invoke` = 동적 호출.
- `CallByName`은 문자열 기반의 VB 전용 동적 접근입니다.
- 리플렉션은 유연하지만 성능 오버헤드가 있으므로 필요할 때만 사용합니다.
- 컴파일 시 타입을 모르는 플러그인/직렬화 시나리오에 적합합니다.
