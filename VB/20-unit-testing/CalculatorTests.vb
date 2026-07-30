Imports Microsoft.VisualStudio.TestTools.UnitTesting

<TestClass>
Public Class CalculatorTests
    Private _calc As Calculator

    <TestInitialize>
    Public Sub Setup()
        _calc = New Calculator()
    End Sub

    <TestMethod>
    Public Sub Add_TwoNumbers_ReturnsSum()
        Dim result = _calc.Add(3, 5)
        Assert.AreEqual(8, result)
    End Sub

    <TestMethod>
    Public Sub Subtract_TwoNumbers_ReturnsDifference()
        Dim result = _calc.Subtract(10, 3)
        Assert.AreEqual(7, result)
    End Sub

    <TestMethod>
    Public Sub Multiply_TwoNumbers_ReturnsProduct()
        Dim result = _calc.Multiply(4, 5)
        Assert.AreEqual(20, result)
    End Sub

    <TestMethod>
    Public Sub Divide_ByNonZero_ReturnsQuotient()
        Dim result = _calc.Divide(10, 3)
        Assert.AreEqual(3, result)
    End Sub

    <TestMethod>
    <ExpectedException(GetType(DivideByZeroException))>
    Public Sub Divide_ByZero_ThrowsException()
        _calc.Divide(10, 0)
    End Sub

    <DataRow(1, False)>
    <DataRow(2, True)>
    <DataRow(3, True)>
    <DataRow(4, False)>
    <DataRow(17, True)>
    <DataRow(20, False)>
    <DataTestMethod>
    Public Sub IsPrime_ReturnsExpected(n As Integer, expected As Boolean)
        Dim result = _calc.IsPrime(n)
        Assert.AreEqual(expected, result)
    End Sub

    <TestCleanup>
    Public Sub Cleanup()
        _calc = Nothing
    End Sub
End Class
