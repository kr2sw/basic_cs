Imports System
Imports System.Reflection

Module Program
    Sub Main()
        ' 런타임에 클래스 정보 조회
        Dim type As Type = GetType(Calculator)
        Console.WriteLine($"클래스: {type.Name}")
        Console.WriteLine($"네임스페이스: {type.Namespace}")
        Console.WriteLine($"어셈블리: {type.Assembly.GetName().Name}")

        ' 메서드 정보
        Console.WriteLine("--- 메서드 목록 ---")
        For Each method In type.GetMethods(BindingFlags.Public Or BindingFlags.Instance Or BindingFlags.DeclaredOnly)
            Dim params = method.GetParameters()
            Dim paramStr = String.Join(", ", params.Select(Function(p) $"{p.ParameterType.Name} {p.Name}"))
            Console.WriteLine($"  {method.ReturnType.Name} {method.Name}({paramStr})")

            ' 커스텀 어트리뷰트 확인
            Dim attr = method.GetCustomAttribute(Of DescriptionAttribute)()
            If attr IsNot Nothing Then
                Console.WriteLine($"    설명: {attr.Description}")
            End If
        Next

        ' 동적 객체 생성 및 호출
        Dim calc = Activator.CreateInstance(type)
        Dim result = type.InvokeMember("Add",
            BindingFlags.InvokeMethod Or BindingFlags.Public Or BindingFlags.Instance,
            Nothing, calc, New Object() {10, 20})
        Console.WriteLine($"동적 호출 Add(10, 20) = {result}")

        ' CallByName (VB 전용)
        Dim calc2 As New Calculator()
        Dim sum = CallByName(calc2, "Add", CallType.Method, 30, 40)
        Console.WriteLine($"CallByName Add(30, 40) = {sum}")
    End Sub
End Module

' 커스텀 어트리뷰트 정의
<AttributeUsage(AttributeTargets.Method Or AttributeTargets.Class)>
Public Class DescriptionAttribute
    Inherits Attribute

    Public Property Description As String

    Public Sub New(description As String)
        Me.Description = description
    End Sub
End Class

<Description("간단한 계산기 클래스")>
Public Class Calculator
    <Description("두 수를 더합니다")>
    Public Function Add(a As Integer, b As Integer) As Integer
        Return a + b
    End Function

    <Description("두 수를 곱합니다")>
    Public Function Multiply(a As Integer, b As Integer) As Integer
        Return a * b
    End Function
End Class
