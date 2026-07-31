Imports System
Imports System.Collections.Generic

Module Program
    Sub Main()
        ' 실제 xUnit/NUnit 코드는 README 참고
        ' 여기서는 콘솔 미니 테스트 러너로 개념을 재현한다

        Dim tests As New List(Of TestCase)()

        ' 1. 기본 팩트 테스트 (xUnit [Fact] / NUnit [Test])
        Dim testAdd As Action = Sub()
                                    Assert.Equal(5, New Calculator().Add(2, 3))
                                End Sub
        tests.Add(New TestCase("Add는 두 수를 더한다", testAdd))

        Dim testDivide As Action = Sub()
                                       Assert.Equal(3, New Calculator().Divide(9, 3))
                                   End Sub
        tests.Add(New TestCase("Divide는 나눗셈을 한다", testDivide))

        ' 2. 예외 검증 테스트
        Dim testThrows As Action = Sub()
                                       Assert.Throws(Sub() New Calculator().Divide(1, 0))
                                   End Sub
        tests.Add(New TestCase("0으로 나누면 예외", testThrows))

        ' 3. Mock으로 의존성 대체 + 호출 검증
        Dim testMock As Action = Sub()
                                     Dim mailer As New FakeEmailSender()
                                     Dim service As New NotificationService(mailer)
                                     service.Notify("user@example.com", "환영합니다")
                                     Assert.True(mailer.SentCount = 1)
                                     Assert.Equal("user@example.com", mailer.LastTo)
                                 End Sub
        tests.Add(New TestCase("mock으로 이메일 전송 검증", testMock))

        ' 4. 파라미터 테스트 (xUnit [Theory] / NUnit [TestCase])
        Dim cases = {2, 3, 4, 5}
        Dim expected = {4, 9, 16, 25}
        For i = 0 To cases.Length - 1
            tests.Add(SquareTest(cases(i), expected(i)))
        Next

        ' 5. 미니 러너 실행
        Console.WriteLine("=== 테스트 실행 ===")
        Dim passed = 0
        Dim failed = 0
        For Each test In tests
            Try
                test.Action()
                passed += 1
                Console.WriteLine($"  [PASS] {test.Name}")
            Catch ex As AssertFailedException
                failed += 1
                Console.WriteLine($"  [FAIL] {test.Name} → {ex.Message}")
            End Try
        Next
        Console.WriteLine()
        Console.WriteLine($"결과: {passed}개 통과, {failed}개 실패")
    End Sub

    ' 파라미터를 매 호출마다 캡처하기 위해 헬퍼 메서드 사용
    Private Function SquareTest(n As Integer, expected As Integer) As TestCase
        Return New TestCase($"파라미터 테스트: {n}의 제곱", Sub() Assert.Equal(expected, n * n))
    End Function
End Module

' --- 미니 테스트 러너 ---
Public Class TestCase
    Public Property Name As String
    Public Property Action As Action

    Public Sub New(name As String, action As Action)
        Me.Name = name
        Me.Action = action
    End Sub
End Class

Public Class AssertFailedException
    Inherits Exception

    Public Sub New(message As String)
        MyBase.New(message)
    End Sub
End Class

Public Module Assert
    Public Sub Equal(Of T)(expected As T, actual As T)
        If Not EqualityComparer(Of T).Default.Equals(expected, actual) Then
            Throw New AssertFailedException($"expected={expected}, actual={actual}")
        End If
    End Sub

    Public Sub True(condition As Boolean)
        If Not condition Then Throw New AssertFailedException("True여야 합니다")
    End Sub

    Public Sub Throws(action As Action)
        Try
            action()
        Catch
            Return    ' 예외 발생 = 성공
        End Try
        Throw New AssertFailedException("예외가 발생해야 합니다")
    End Sub
End Module

' --- 테스트 대상 코드 ---
Public Class Calculator
    Public Function Add(a As Integer, b As Integer) As Integer
        Return a + b
    End Function

    Public Function Divide(a As Integer, b As Integer) As Integer
        If b = 0 Then Throw New DivideByZeroException()
        Return a \ b
    End Function
End Class

Public Interface IEmailSender
    Sub Send(to As String, body As String)
End Interface

' 테스트용 Mock: 호출 기록을 남기는 가짜 구현
Public Class FakeEmailSender
    Implements IEmailSender

    Public Property SentCount As Integer
    Public Property LastTo As String

    Public Sub Send(to As String, body As String) Implements IEmailSender.Send
        SentCount += 1
        LastTo = to
        Console.WriteLine($"  [mock] {to}로 이메일 발송: {body}")
    End Sub
End Class

Public Class NotificationService
    Private ReadOnly _sender As IEmailSender

    Public Sub New(sender As IEmailSender)
        _sender = sender
    End Sub

    Public Sub Notify(to As String, message As String)
        _sender.Send(to, message)
    End Sub
End Class
