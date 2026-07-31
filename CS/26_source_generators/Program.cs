using System.Text;

namespace BasicCS.Chapter26;

/*
 * 실제 Roslyn 소스 생성기 구조 (별도 프로젝트, NuGet 필요):
 *
 * [Generator]
 * public class AutoNotifyGenerator : IIncrementalGenerator
 * {
 *     public void Initialize(IncrementalGeneratorInitializationContext context)
 *     {
 *         // 1. 분석할 대상(어트리뷰트가 붙은 심볼)을 등록
 *         var provider = context.SyntaxProvider.ForAttributeWithMetadataName(...);
 *         // 2. 변경분만 계산 (incremental)
 *         var compiled = provider.Select((node, _) => node);
 *         // 3. 코드 생성
 *         context.RegisterSourceOutput(compiled, (spc, model) =>
 *         {
 *             spc.AddSource("Generated.g.cs", SourceText.From(code, Encoding.UTF8));
 *         });
 *     }
 * }
 *
 * 생성되는 코드는 대상 클래스의 partial 선언에 합쳐집니다.
 */

// ---- 어트리뷰트 정의 (생성기가 감지할 마커) ----
[AttributeUsage(AttributeTargets.Class)]
public sealed class AutoToStringAttribute : Attribute;

// ---- 소스 생성기 시뮬레이터 ----
// 실제로는 Roslyn이 컴파일 시점에 AST를 분석하지만,
// 여기서는 리플렉션 + 코드 텍스트 생성으로 동작을 재현합니다.
public static class SourceGeneratorSimulator
{
    // 어트리뷰트가 붙은 타입을 찾아 partial 메서드용 코드 텍스트를 만든다
    public static string GenerateToString(Type type)
    {
        var sb = new StringBuilder();
        sb.AppendLine($"// 자동 생성 코드 (source generator 출력)");
        sb.AppendLine($"public partial class {type.Name}");
        sb.AppendLine("{");
        sb.AppendLine($"    public override string ToString()");
        sb.AppendLine($"        => \"{type.Name} [\" + string.Join(\", \", (");
        var props = type.GetProperties()
                        .Select(p => $"{p.Name}={{ {p.Name} }}");
        sb.AppendLine($"            new[] {{ {string.Join(", ", props)} }}");
        sb.AppendLine($"        )) + \"]\";");
        sb.AppendLine("}");
        return sb.ToString();
    }
}

// ---- 대상: partial 클래스. ToString은 소스 생성기가 만든다고 가정 ----
[AutoToString]
public partial class Order
{
    public int Id { get; set; }
    public string Customer { get; set; } = "";
    public decimal Total { get; set; }
}

// partial: 수동으로 작성하는 나머지 부분 (파일이 나뉜 것처럼 동작)
public partial class Order
{
    public string Display() => $"주문#{Id} / {Customer}";
}

static class Program
{
    static void Main()
    {
        // ---- 1. 리플렉션으로 어트리뷰트 감지 ----
        var type = typeof(Order);
        bool hasMarker = type.IsDefined(typeof(AutoToStringAttribute), false);
        Console.WriteLine($"[감지] Order에 AutoToString 어트리뷰트? {hasMarker}");

        // ---- 2. 소스 생성기가 만드는 코드 텍스트 확인 ----
        Console.WriteLine("\n[생성된 소스 코드]");
        Console.WriteLine(SourceGeneratorSimulator.GenerateToString(type));

        // ---- 3. 생성 코드가 컴파일 타임에 partial로 합쳐졌다고 가정 ----
        // (실제로는 생성기가 내보낸 ToString이 여기에 있다고 칩시다)
        var order = new Order { Id = 7, Customer = "홍길동", Total = 150000m };
        Console.WriteLine($"[partial 통합 결과] Display: {order.Display()}");

        // ---- 4. 수동 컴파일 대체 예: System.Text.Json의 소스 생성기 모드 ----
        // JsonSerializerContext 소스 생성기 예제 (주석)
        Console.WriteLine("\n[참고] System.Text.Json은 [JsonSerializable] 어트리뷰트로");
        Console.WriteLine("소스 생성기 기반 직렬화를 지원합니다 (리플렉션 대비 고성능).");
    }
}
