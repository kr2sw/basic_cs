namespace BasicCS.Chapter18;

using System.Diagnostics;
using System.Reflection;

// ──────────────────────────────────────────────
// 사용자 정의 특성(Custom Attribute)
// ──────────────────────────────────────────────
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false)]
public class DescriptionAttribute : Attribute
{
    public string Text { get; }
    public string? Version { get; set; }

    public DescriptionAttribute(string text)
    {
        Text = text;
    }
}

// ──────────────────────────────────────────────
// 예제용 클래스 (리플렉션 대상)
// ──────────────────────────────────────────────
[Description("샘플 계산기 클래스", Version = "1.0")]
public class SampleCalculator
{
    // public 필드
    public string CalculatorName = "BasicCalc";

    // private 필드 (리플렉션으로 접근)
    private int _precision = 2;

    public int Precision => _precision;

    // 속성
    public double LastResult { get; private set; }

    // 생성자
    public SampleCalculator()
    {
        LastResult = 0;
    }

    // 일반 메서드
    public int Add(int a, int b)
    {
        LastResult = a + b;
        return a + b;
    }

    // Obsolete 특성 예제
    [Obsolete("이 메서드는 더 이상 사용되지 않습니다. Add()를 사용하세요.")]
    public int OldAdd(int a, int b) => a + b;

    // Conditional 특성 예제 (DEBUG 심볼이 정의된 경우에만 호출됨)
    [Conditional("DEBUG")]
    public void LogDebug(string message)
    {
        Console.WriteLine($"[DEBUG] {message}");
    }

    // private 메서드 (리플렉션으로 접근)
    private string GetVersionInfo() => "v1.0.0";

    [Description("두 수를 곱합니다", Version = "1.1")]
    public int Multiply(int a, int b) => a * b;
}

// ──────────────────────────────────────────────
// 특성이 적용된 다른 클래스
// ──────────────────────────────────────────────
[Description("인사말 처리기")]
public class Greeter
{
    public string Greet(string name) => $"안녕하세요, {name}님!";
}

[Obsolete("Greeter 클래스로 대체되었습니다.")]
public class OldGreeter
{
    public string SayHello(string name) => $"Hello, {name}!";
}

internal class ReflectionAttributes
{
    static void Main()
    {
        Console.WriteLine("=== 리플렉션(Reflection) 및 특성(Attribute) 예제 ===\n");

        // ──────────────────────────────────────────────
        // 1. Type 클래스 (GetType(), typeof())
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 1. Type 클래스 ───");

        // typeof() 연산자 사용
        Type stringType = typeof(string);
        Console.WriteLine($"typeof(string): FullName={stringType.FullName}, IsClass={stringType.IsClass}");

        // GetType() 메서드 사용
        int number = 42;
        Type intType = number.GetType();
        Console.WriteLine($"42.GetType(): FullName={intType.FullName}, IsValueType={intType.IsValueType}");

        // Type 정보
        Type calcType = typeof(SampleCalculator);
        Console.WriteLine($"typeof(SampleCalculator): Name={calcType.Name}, Namespace={calcType.Namespace}");
        Console.WriteLine($"  IsPublic={calcType.IsPublic}, IsAbstract={calcType.IsAbstract}\n");

        // ──────────────────────────────────────────────
        // 2. Type.GetProperties()
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 2. PropertyInfo ───");

        PropertyInfo[] properties = calcType.GetProperties(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Console.WriteLine($"SampleCalculator의 속성 목록:");
        foreach (PropertyInfo prop in properties)
        {
            Console.WriteLine($"  - {prop.Name} (Type: {prop.PropertyType.Name}, 읽기:{prop.CanRead}, 쓰기:{prop.CanWrite})");
        }
        Console.WriteLine();

        // ──────────────────────────────────────────────
        // 3. Type.GetMethods()
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 3. MethodInfo ───");

        MethodInfo[] methods = calcType.GetMethods(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly);
        Console.WriteLine($"SampleCalculator의 메서드 목록 (DeclaredOnly):");
        foreach (MethodInfo method in methods)
        {
            var paramInfos = method.GetParameters();
            string paramStr = string.Join(", ", paramInfos.Select(p => $"{p.ParameterType.Name} {p.Name}"));
            Console.WriteLine($"  - {method.ReturnType.Name} {method.Name}({paramStr})");
        }
        Console.WriteLine();

        // ──────────────────────────────────────────────
        // 4. Type.GetFields()
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 4. FieldInfo ───");

        FieldInfo[] fields = calcType.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
        Console.WriteLine($"SampleCalculator의 필드 목록:");
        foreach (FieldInfo field in fields)
        {
            Console.WriteLine($"  - {field.FieldType.Name} {field.Name} (IsPublic={field.IsPublic})");
        }
        Console.WriteLine();

        // ──────────────────────────────────────────────
        // 5. 사용자 정의 특성 (DescriptionAttribute)
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 5. 사용자 정의 특성 ───");

        // 클래스에 적용된 특성 읽기
        DescriptionAttribute? classDesc = calcType.GetCustomAttribute<DescriptionAttribute>();
        Console.WriteLine($"SampleCalculator 설명: {classDesc?.Text}, 버전: {classDesc?.Version}");

        // 메서드에 적용된 특성 읽기
        MethodInfo? multiplyMethod = calcType.GetMethod("Multiply");
        DescriptionAttribute? methodDesc = multiplyMethod?.GetCustomAttribute<DescriptionAttribute>();
        Console.WriteLine($"Multiply 메서드 설명: {methodDesc?.Text}, 버전: {methodDesc?.Version}\n");

        // ──────────────────────────────────────────────
        // 6. [Obsolete] 특성 사용
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 6. [Obsolete] 특성 ───");

        // 리플렉션으로 Obsolete 특성 확인
        MethodInfo? oldAddMethod = calcType.GetMethod("OldAdd");
        ObsoleteAttribute? obsoleteAttr = oldAddMethod?.GetCustomAttribute<ObsoleteAttribute>();
        Console.WriteLine($"OldAdd에 Obsolete: {(obsoleteAttr != null)}");
        if (obsoleteAttr != null)
            Console.WriteLine($"  오류 메시지: {obsoleteAttr.Message}");

#pragma warning disable CS0618 // [Obsolete] 데모용
        Type oldGreeterType = typeof(OldGreeter);
#pragma warning restore CS0618
        ObsoleteAttribute? classObsolete = oldGreeterType.GetCustomAttribute<ObsoleteAttribute>();
        Console.WriteLine($"OldGreeter에 Obsolete: {(classObsolete != null)}");
        if (classObsolete != null)
            Console.WriteLine($"  메시지: {classObsolete.Message}\n");

        // ──────────────────────────────────────────────
        // 7. [Conditional] 특성
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 7. [Conditional] 특성 ───");

        MethodInfo? logMethod = calcType.GetMethod("LogDebug");
        ConditionalAttribute? conditionalAttr = logMethod?.GetCustomAttribute<ConditionalAttribute>();
        Console.WriteLine($"LogDebug에 Conditional: {(conditionalAttr != null)}");
        if (conditionalAttr != null)
            Console.WriteLine($"  조건부 심볼: {conditionalAttr.ConditionString}");

        // DEBUG 모드에서만 호출됨
        var calculator = new SampleCalculator();
        calculator.LogDebug("이 메시지는 DEBUG 모드에서만 출력됩니다.");
        Console.WriteLine();

        // ──────────────────────────────────────────────
        // 8. Attribute.GetCustomAttribute (정적 메서드)
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 8. Attribute.GetCustomAttribute (정적) ───");

        Attribute? attr = Attribute.GetCustomAttribute(calcType, typeof(DescriptionAttribute));
        if (attr is DescriptionAttribute da)
            Console.WriteLine($"[정적] Description: {da.Text}, Version: {da.Version}\n");

        // ──────────────────────────────────────────────
        // 9. Activator.CreateInstance
        // ──────────────────────────────────────────────
        Console.WriteLine("─── 9. Activator.CreateInstance ───");

        // 타입으로 인스턴스 생성 (기본 생성자)
        object? instance = Activator.CreateInstance(typeof(SampleCalculator));
        if (instance is SampleCalculator calc)
        {
            Console.WriteLine($"Activator로 생성된 인스턴스 타입: {calc.GetType().Name}");

            // 리플렉션으로 메서드 호출
            MethodInfo? addMethod = calcType.GetMethod("Add");
            if (addMethod != null)
            {
                object? result = addMethod.Invoke(calc, new object[] { 10, 20 });
                Console.WriteLine($"리플렉션으로 Add(10, 20) 호출 결과: {result}");
            }

            // 리플렉션으로 private 필드 읽기
            FieldInfo? precisionField = calcType.GetField("_precision", BindingFlags.NonPublic | BindingFlags.Instance);
            if (precisionField != null)
            {
                int precValue = (int)precisionField.GetValue(calc)!;
                Console.WriteLine($"리플렉션으로 private 필드 _precision 읽기: {precValue}");
            }

            // 리플렉션으로 private 메서드 호출
            MethodInfo? versionMethod = calcType.GetMethod("GetVersionInfo", BindingFlags.NonPublic | BindingFlags.Instance);
            if (versionMethod != null)
            {
                string? versionInfo = versionMethod.Invoke(calc, null) as string;
                Console.WriteLine($"리플렉션으로 private 메서드 호출: {versionInfo}");
            }
        }

        Console.WriteLine("\n=== 리플렉션 및 특성 예제 종료 ===");
    }
}
