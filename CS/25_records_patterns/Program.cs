namespace BasicCS.Chapter25;

// ---- positional record: 생성자/프로퍼티/Deconstruct 자동 생성 ----
public record Person(string Name, int Age, string Job);

// record는 클래스처럼 상속도 가능
public record Employee(string Name, int Age, string Job, string Department)
    : Person(Name, Age, Job);

static class Program
{
    static string DescribeByPattern(Person p) => p switch
    {
        // relational pattern: 나이 비교
        { Age: >= 60 } => "실버 세대",
        { Age: >= 20 and < 60 } => "성인",
        { Age: < 20 } => "청소년",
        _ => "알 수 없음",
    };

    static string Classify(Person p) => p switch
    {
        // property pattern: 속성 조합
        { Name: "관리자", Age: >= 30 } => "고참 관리자",
        { Job: "개발자", Age: < 30 } => "주니어 개발자",
        { Job: "개발자" } => "개발자",
        _ => "일반 구성원",
    };

    // positional pattern: Deconstruct와 결합
    static string AgeBand(Person p) => p switch
    {
        (_, < 13, _) => "어린이",
        (_, < 20, _) => "십대",
        (_, _, "개발자") => "개발자(나이 무관)",
        _ => "기타",
    };

    // list pattern: 시퀀스 구조 매칭
    static string DescribeSequence(int[] seq) => seq switch
    {
        [] => "빈 배열",
        [1, ..] => "1로 시작",
        [.., 9] => "9로 끝남",
        [var a, var b, ..] when a == b => "첫 두 요소 동일",
        _ => $"길이 {seq.Length}",
    };

    static void Main()
    {
        // ---- 레코드 값 기반 동등성 ----
        var p1 = new Person("홍길동", 30, "개발자");
        var p2 = new Person("홍길동", 30, "개발자");
        Console.WriteLine($"값 기반 동등성: p1 == p2 ? {p1 == p2}");   // True
        Console.WriteLine($"참조 동일성:   ReferenceEquals ? {ReferenceEquals(p1, p2)}");

        // ---- with 표현식: 복사 후 변경 ----
        var p3 = p1 with { Age = 31 };
        Console.WriteLine($"\nwith 표현식: {p3}");

        // ---- ToString 자동 구현 ----
        Console.WriteLine($"ToString: {p1}");

        // ---- 상속 + Deconstruct ----
        var emp = new Employee("김철수", 40, "기획자", "경영지원");
        var (name, age, job) = emp; // Deconstruct 사용
        Console.WriteLine($"\nDeconstruct: {name}, {age}세, {job}");

        // ---- switch 식 패턴 매칭 ----
        var people = new[]
        {
            new Person("홍길동", 30, "개발자"),
            new Person("김영희", 65, "은퇴자"),
            new Person("관리자", 35, "매니저"),
            new Person("박민수", 17, "학생"),
        };
        Console.WriteLine("\n[switch 식] 나이 그룹");
        foreach (var person in people)
            Console.WriteLine($"  {person.Name} ({person.Age}세): {DescribeByPattern(person)}");

        Console.WriteLine("\n[switch 식] 속성 조합 분류");
        foreach (var person in people)
            Console.WriteLine($"  {person.Name}: {Classify(person)}");

        Console.WriteLine("\n[positional pattern]");
        foreach (var person in people)
            Console.WriteLine($"  {person.Name}: {AgeBand(person)}");

        // ---- list pattern ----
        int[][] seqs = { [], new[] { 1, 2, 3 }, new[] { 5, 9 }, new[] { 7, 7, 3 }, new[] { 4, 5, 6 } };
        Console.WriteLine("\n[list pattern]");
        foreach (var seq in seqs)
            Console.WriteLine($"  [{string.Join(",", seq)}] => {DescribeSequence(seq)}");

        // ---- 타입 패턴 + is ----
        object[] objs = { "문자열", 42, 3.14, new Person("테스트", 1, "무직") };
        Console.WriteLine("\n[type pattern]");
        foreach (var o in objs)
        {
            string desc = o switch
            {
                string s => $"string '{s}'",
                int n => $"int {n}",
                double d => $"double {d}",
                Person p => $"Person {p.Name}",
                _ => "기타",
            };
            Console.WriteLine($"  {o.GetType().Name}: {desc}");
        }
    }
}
