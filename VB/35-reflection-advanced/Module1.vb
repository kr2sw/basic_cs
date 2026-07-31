Imports System
Imports System.Linq
Imports System.Reflection
Imports Microsoft.VisualBasic

Module Program
    Sub Main()
        Console.WriteLine("=== 1. Type 정보 탐색 ===")
        Dim t As Type = GetType(Car)
        Console.WriteLine($"타입: {t.FullName}")

        Console.WriteLine("  프로퍼티:")
        For Each p In t.GetProperties()
            Console.WriteLine($"    {p.PropertyType.Name} {p.Name}")
        Next

        Console.WriteLine("  메서드 (이 클래스에 선언된 것만):")
        For Each m In t.GetMethods().Where(Function(x) x.DeclaringType Is GetType(Car))
            Console.WriteLine($"    {m.Name}")
        Next

        Console.WriteLine()
        Console.WriteLine("=== 2. Activator로 객체 생성 ===")
        ' 생성자 인자를 배열로 전달해 런타임에 객체 생성
        Dim car = Activator.CreateInstance(t, "소나타", 0)
        Console.WriteLine($"생성된 객체: {car}")

        Console.WriteLine()
        Console.WriteLine("=== 3. MethodInfo.Invoke 동적 호출 ===")
        Dim accelerate = t.GetMethod("Accelerate")
        accelerate.Invoke(car, {50})
        accelerate.Invoke(car, {30})

        Dim speedProp = t.GetProperty("Speed")
        Console.WriteLine($"현재 속도: {speedProp.GetValue(car)} km/h")

        Console.WriteLine()
        Console.WriteLine("=== 4. CallByName (VB 전용) ===")
        CallByName(car, "Speed", CallType.Set, 100)
        Dim speed = CallByName(car, "Speed", CallType.Get)
        Console.WriteLine($"CallByName 속도: {speed} km/h")
        CallByName(car, "Honk", CallType.Method)

        Console.WriteLine()
        Console.WriteLine("=== 5. 사용자 정의 특성 읽기 ===")
        Dim attr = t.GetCustomAttribute(Of DemoInfoAttribute)()
        If attr IsNot Nothing Then
            Console.WriteLine($"설명: {attr.Description}")
        End If
    End Sub
End Module

<DemoInfo("자동차 클래스 데모")>
Public Class Car
    Public Property Name As String
    Public Property Speed As Integer

    Public Sub New(name As String, speed As Integer)
        Me.Name = name
        Me.Speed = speed
    End Sub

    Public Sub Accelerate(by As Integer)
        Speed += by
        Console.WriteLine($"  가속! {Name} → {Speed} km/h")
    End Sub

    Public Sub Honk()
        Console.WriteLine($"  빵빵! ({Name})")
    End Sub

    Public Overrides Function ToString() As String
        Return $"Car({Name}, {Speed}km/h)"
    End Function
End Class

' 사용자 정의 특성
Public Class DemoInfoAttribute
    Inherits Attribute

    Public ReadOnly Property Description As String

    Public Sub New(description As String)
        Me.Description = description
    End Sub
End Class
