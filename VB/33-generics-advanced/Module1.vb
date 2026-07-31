Imports System
Imports System.Collections.Generic

Module Program
    Sub Main()
        Console.WriteLine("=== 1. 공변성 (Covariance, Out) ===")
        ' String 목록은 Object 목록처럼 사용 가능
        Dim strings As IEnumerable(Of String) = New List(Of String)() From {"가", "나"}
        Dim objects As IEnumerable(Of Object) = strings          ' 공변 OK
        For Each o In objects
            Console.WriteLine($"  {o} ({o.GetType().Name})")
        Next

        ' 직접 만든 공변 인터페이스
        Dim producer As IProducer(Of String) = New StringProducer()
        Dim up As IProducer(Of Object) = producer                ' 공변 OK
        Console.WriteLine($"  생성된 값: {up.Create()}")

        Console.WriteLine()
        Console.WriteLine("=== 2. 반공변성 (Contravariance, In) ===")
        ' Object를 받는 소비자는 String도 받을 수 있다
        Dim consumer As IConsumer(Of Object) = New ObjectConsumer()
        Dim stringConsumer As IConsumer(Of String) = consumer   ' 반공변 OK
        stringConsumer.Use("반공변 테스트")

        Console.WriteLine()
        Console.WriteLine("=== 3. 고급 제약 조건 ===")
        Dim created = CreateInstance(Of StringBuilderWrapper)()
        Console.WriteLine($"New 제약 생성: {created.Value}")

        Dim num = GetDefault(Of Integer)()
        Console.WriteLine($"default(T) Integer: {num}")

        Dim pair = New Pair(Of String, Integer)("키", 10)
        Console.WriteLine($"이중 제네릭: {pair.First}/{pair.Second}")
    End Sub

    ' New 제약: 기본 생성자 존재 보장
    Function CreateInstance(Of T As New)() As T
        Return New T()
    End Function

    ' 기본값: 값 타입 0, 참조 타입 Nothing
    Function GetDefault(Of T)() As T
        Return Nothing
    End Function
End Module

Public Class StringBuilderWrapper
    Public ReadOnly Property Value As String

    Public Sub New()
        Me.Value = "기본 생성자로 생성됨"
    End Sub
End Class

' --- 공변 인터페이스: T가 반환 위치에만 등장 ---
Public Interface IProducer(Of Out T)
    Function Create() As T
End Interface

Public Class StringProducer
    Implements IProducer(Of String)

    Public Function Create() As String Implements IProducer(Of String).Create
        Return "생산된 문자열"
    End Function
End Class

' --- 반공변 인터페이스: T가 입력 위치에만 등장 ---
Public Interface IConsumer(Of In T)
    Sub Use(item As T)
End Interface

Public Class ObjectConsumer
    Implements IConsumer(Of Object)

    Public Sub Use(item As Object) Implements IConsumer(Of Object).Use
        Console.WriteLine($"  소비: {item}")
    End Sub
End Class

' 이중 제네릭 타입
Public Class Pair(Of TFirst, TSecond)
    Public ReadOnly Property First As TFirst
    Public ReadOnly Property Second As TSecond

    Public Sub New(first As TFirst, second As TSecond)
        Me.First = first
        Me.Second = second
    End Sub
End Class
