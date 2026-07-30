Imports System

Module Program
    Sub Main()
        Dim drawables As IDrawable() = {New Circle(5), New Rectangle(4, 3)}

        For Each d In drawables
            d.Draw()
            If TypeOf d Is IArea Then
                Dim area = CType(d, IArea)
                Console.WriteLine($"  면적: {area.GetArea():F2}")
            End If
        Next

        ' 인터페이스를 매개변수로
        PrintInfo(New Circle(3))
        PrintInfo(New Rectangle(2, 6))
    End Sub

    Sub PrintInfo(ByVal drawable As IDrawable)
        drawable.Draw()
    End Sub
End Module

' 인터페이스 정의
Public Interface IDrawable
    Sub Draw()
    ReadOnly Property Color As String
End Interface

Public Interface IArea
    Function GetArea() As Double
End Interface

' 하나의 클래스가 여러 인터페이스 구현
Public Class Circle
    Implements IDrawable, IArea

    Public Property Radius As Double

    Public Sub New(radius As Double)
        Me.Radius = radius
    End Sub

    Public Sub Draw() Implements IDrawable.Draw
        Console.WriteLine($"원 그리기 (반지름: {Radius})")
    End Sub

    Public ReadOnly Property Color As String Implements IDrawable.Color
        Get
            Return "Red"
        End Get
    End Property

    Public Function GetArea() As Double Implements IArea.GetArea
        Return Math.PI * Radius * Radius
    End Function
End Class

Public Class Rectangle
    Implements IDrawable, IArea

    Public Property Width As Double
    Public Property Height As Double

    Public Sub New(width As Double, height As Double)
        Me.Width = width
        Me.Height = height
    End Sub

    Public Sub Draw() Implements IDrawable.Draw
        Console.WriteLine($"사각형 그리기 ({Width} x {Height})")
    End Sub

    Public ReadOnly Property Color As String Implements IDrawable.Color
        Get
            Return "Blue"
        End Get
    End Property

    Public Function GetArea() As Double Implements IArea.GetArea
        Return Width * Height
    End Function
End Class
