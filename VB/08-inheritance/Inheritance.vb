Imports System

Module Program
    Sub Main()
        Dim animals As Animal() = {New Dog("바둑이"), New Cat("나비")}

        For Each a In animals
            Console.WriteLine($"{a.Name}: ", False)

            a.Speak()

            ' 타입 확인
            If TypeOf a Is Dog Then
                Console.WriteLine($"{a.Name}은/는 개입니다.")
            End If
        Next

        ' 생성자 체인 확인
        Dim e1 As New Manager("김철수", "개발팀")
        Console.WriteLine($"{e1.Name} - {e1.Department}")
    End Sub
End Module

' MustInherit = 추상 클래스
Public MustInherit Class Animal
    Public Property Name As String

    Public Sub New(name As String)
        Me.Name = name
    End Sub

    ' MustOverride = 추상 메서드
    Public MustOverride Sub Speak()

    Public Overridable Sub Move()
        Console.WriteLine("움직입니다.")
    End Sub
End Class

Public Class Dog
    Inherits Animal

    Public Sub New(name As String)
        MyBase.New(name)
    End Sub

    Public Overrides Sub Speak()
        Console.WriteLine("멍멍!")
    End Sub

    Public Overrides Sub Move()
        Console.WriteLine("네 발로 달립니다.")
    End Sub
End Class

Public Class Cat
    Inherits Animal

    Public Sub New(name As String)
        MyBase.New(name)
    End Sub

    Public Overrides Sub Speak()
        Console.WriteLine("야옹~")
    End Sub
End Class

' 생성자 체인
Public Class Employee
    Public Property Name As String

    Public Sub New(name As String)
        Me.Name = name
        Console.WriteLine($"Employee 생성: {name}")
    End Sub
End Class

Public Class Manager
    Inherits Employee

    Public Property Department As String

    Public Sub New(name As String, department As String)
        MyBase.New(name)
        Me.Department = department
        Console.WriteLine($"Manager 생성: {name}, {department}")
    End Sub
End Class
