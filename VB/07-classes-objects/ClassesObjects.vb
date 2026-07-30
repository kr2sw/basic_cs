Imports System

Module Program
    Sub Main()
        ' 객체 생성
        Dim p1 As New Person("Alice", 25)
        p1.Introduce()

        ' 속성 사용
        p1.Name = "Bob"
        p1.Age = 30
        Console.WriteLine($"이름: {p1.Name}, 나이: {p1.Age}")

        ' Shared 멤버
        Console.WriteLine($"총 인원: {Person.Count}")

        ' 자동 구현 속성
        Dim c1 As New Car("Tesla")
        c1.Speed = 100
        Console.WriteLine($"{c1.Model} 속도: {c1.Speed} km/h")

        ' 읽기 전용 속성
        Console.WriteLine($"생성 시간: {p1.CreatedAt}")
    End Sub
End Module

' 클래스 정의
Public Class Person
    ' Shared(정적) 필드
    Public Shared Count As Integer = 0

    ' 속성 (자동 구현)
    Public Property Name As String

    ' 속성 (수동 구현)
    Private _age As Integer
    Public Property Age As Integer
        Get
            Return _age
        End Get
        Set(value As Integer)
            If value >= 0 Then _age = value
        End Set
    End Property

    ' 읽기 전용 속성
    Public ReadOnly Property CreatedAt As Date

    ' 생성자
    Public Sub New(name As String, age As Integer)
        Me.Name = name
        Me.Age = age
        Me.CreatedAt = Date.Now
        Count += 1
    End Sub

    ' 메서드
    Public Sub Introduce()
        Console.WriteLine($"안녕하세요, 저는 {Name}이고 {Age}세입니다.")
    End Sub
End Class

' 자동 구현 속성 예제
Public Class Car
    Public Property Model As String
    Public Property Speed As Integer

    Public Sub New(model As String)
        Me.Model = model
    End Sub
End Class
