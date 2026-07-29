# 05 메서드(Methods)

C#에서 메서드를 정의하고 다양한 파라미터 전달 방식을 학습합니다.

## 주요 개념

- 기본 메서드 정의 및 반환값
- `ref` / `out` 파라미터
- `params` 가변 인자
- 선택적(optional) 매개변수와 명명된(named) 인자
- 식 본문 메서드 (expression-bodied, `=>`)
- 지역 함수 (local function)
- 메서드 오버로딩

## 예제 코드

```csharp
static int Add(int a, int b) => a + b;
static void Increment(ref int x) => x++;
static void TryDivide(int a, int b, out double quot, out int rem)
{
    quot = (double)a / b;
    rem = a % b;
}
static int SumAll(params int[] numbers) => numbers.Sum();
```

## 실행 방법

```bash
dotnet run --project ../05_methods
```

## 핵심 요약

- `ref`는 호출자 변수를 직접 수정하고, `out`은 반환값 외 추가 출력이 필요할 때 사용합니다.
- `params` 키워드로 가변 개수의 인자를 전달할 수 있습니다.
- 메서드 오버로딩으로 같은 이름의 여러 메서드를 정의할 수 있습니다.
