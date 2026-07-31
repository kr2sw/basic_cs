namespace BasicCS.Chapter38;

// ---- Option<T>: 값 부재를 타입으로 표현 ----
public readonly struct Option<T>
{
    private readonly T? _value;
    public bool HasValue { get; }
    private Option(T value) { _value = value; HasValue = true; }
    private Option(bool none) { _value = default; HasValue = false; }

    public static Option<T> Some(T value) => new(value);
    public static Option<T> None() => new(false);

    public Option<U> Map<U>(Func<T, U> mapper)
        => HasValue ? Option<U>.Some(mapper(_value!)) : Option<U>.None();

    public Option<U> Bind<U>(Func<T, Option<U>> binder)
        => HasValue ? binder(_value!) : Option<U>.None();

    public T ValueOr(T fallback) => HasValue ? _value! : fallback;

    public override string ToString() => HasValue ? $"Some({_value})" : "None";
}

// ---- Either<L, R>: 성공(Right)/실패(Left) ----
public readonly struct Either<TLeft, TRight>
{
    private readonly TLeft? _left;
    private readonly TRight? _right;
    public bool IsRight { get; }

    private Either(TLeft left, TRight? right, bool isRight)
    {
        _left = left;
        _right = right;
        IsRight = isRight;
    }

    public static Either<TLeft, TRight> Left(TLeft value) => new(value, default, false);
    public static Either<TLeft, TRight> Right(TRight value) => new(default, value, true);

    public TResult Fold<TResult>(Func<TLeft, TResult> onLeft, Func<TRight, TResult> onRight)
        => IsRight ? onRight(_right!) : onLeft(_left!);

    public Either<TLeft, U> MapRight<U>(Func<TRight, U> mapper)
        => IsRight ? Either<TLeft, U>.Right(mapper(_right!)) : Either<TLeft, U>.Left(_left!);

    public override string ToString() => IsRight ? $"Right({_right})" : $"Left({_left})";
}

// ---- 도메인 모델 ----
public record User(string Name, int Age);

static class Program
{
    // 숫자 파싱: 실패 가능성을 Option으로 표현
    static Option<int> Parse(string input)
        => int.TryParse(input, out int n) ? Option<int>.Some(n) : Option<int>.None();

    // Either: 나이 검증 (오류 문자열 | 성공)
    static Either<string, User> ValidateAge(int age)
        => age < 0 ? Either<string, User>.Left("나이가 음수입니다")
           : age > 150 ? Either<string, User>.Left("비현실적인 나이입니다")
           : Either<string, User>.Right(new User("사용자", age));

    // 저장(성공) — 마지막 파이프라인 단계
    static Either<string, string> Save(User user)
        => Either<string, string>.Right($"{user.Name}({user.Age}세) 저장 완료");

    static void Main()
    {
        Console.WriteLine("== Option: 파싱 결과 ==");
        foreach (var input in new[] { "42", "hello" })
        {
            var result = Parse(input)
                .Map(n => n * 2)          // 값이 있을 때만 변환
                .Map(n => $"2배 = {n}");
            Console.WriteLine($"  '{input}' -> {result}");
        }

        Console.WriteLine("\n== Either: 검증 실패/성공 ==");
        foreach (var age in new[] { 25, -5, 200 })
        {
            var outcome = ValidateAge(age)
                .MapRight(u => u with { Name = $"VIP_{u.Name}" })
                .Fold(
                    err => $"실패: {err}",
                    ok => $"성공: {ok}");
            Console.WriteLine($"  나이 {age,4} -> {outcome}");
        }

        Console.WriteLine("\n== 파이프라인: 파싱 -> 검증 -> 저장 ==");
        // 파싱 실패도 Either(Left)로 만드는 조합 함수
        static Either<string, int> ParseAge(string input)
            => int.TryParse(input, out int n)
                ? Either<string, int>.Right(n)
                : Either<string, int>.Left("숫자가 아닙니다");

        foreach (var input in new[] { "30", "-1", "999", "abc" })
        {
            string pipeline = ParseAge(input)
                .MapRight(age => ValidateAge(age))   // 검증 결과(Either) 중첩
                .Fold(err => $"실패: {err}",          // 최상위 Left 처리
                      nested => nested.Fold(         // 중첩 Either 풀기
                          err => $"실패: {err}",
                          user => Save(user).Fold(e => $"실패: {e}", ok => ok)));
            Console.WriteLine($"  '{input}' -> {pipeline}");
        }

        Console.WriteLine("\n== LINQ도 파이프라인 (모나드 유사) ==");
        var numbers = new[] { 1, 2, 3, 4, 5, 6 };
        var evens = numbers
            .Where(n => n % 2 == 0)
            .Select(n => n * n);
        Console.WriteLine($"  짝수의 제곱: [{string.Join(",", evens)}]");
    }
}
