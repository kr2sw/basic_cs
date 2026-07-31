Option Strict On

Imports System
Imports System.Windows
Imports System.Windows.Controls

Namespace Ch37

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub
    End Class

    ' WrapPanel과 유사: 가로 공간이 부족하면 다음 줄로 이동
    Public Class WrapFlowPanel
        Inherits Panel

        Protected Overrides Function MeasureOverride(availableSize As Size) As Size
            Dim width As Double = 0
            Dim height As Double = 0
            Dim lineWidth As Double = 0
            Dim lineHeight As Double = 0

            For Each child As UIElement In InternalChildren
                child.Measure(availableSize)
                Dim w = child.DesiredSize.Width
                Dim h = child.DesiredSize.Height

                If lineWidth + w > availableSize.Width AndAlso lineWidth > 0 Then
                    width = Math.Max(width, lineWidth)
                    height += lineHeight
                    lineWidth = w
                    lineHeight = h
                Else
                    lineWidth += w
                    lineHeight = Math.Max(lineHeight, h)
                End If
            Next

            width = Math.Max(width, lineWidth)
            height += lineHeight

            If Double.IsPositiveInfinity(availableSize.Width) Then
                Return New Size(width, height)
            End If
            Return New Size(availableSize.Width, height)
        End Function

        Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
            Dim x As Double = 0
            Dim y As Double = 0
            Dim lineHeight As Double = 0

            For Each child As UIElement In InternalChildren
                If x + child.DesiredSize.Width > finalSize.Width AndAlso x > 0 Then
                    x = 0
                    y += lineHeight
                    lineHeight = 0
                End If

                child.Arrange(New Rect(x, y, child.DesiredSize.Width, child.DesiredSize.Height))
                x += child.DesiredSize.Width
                lineHeight = Math.Max(lineHeight, child.DesiredSize.Height)
            Next

            Return finalSize
        End Function
    End Class

    ' 자식을 원형으로 배치. Radius는 디펜던시 속성이라 슬라이더로 조절 가능.
    Public Class RadialPanel
        Inherits Panel

        Public Shared ReadOnly RadiusProperty As DependencyProperty =
            DependencyProperty.Register(
                NameOf(Radius), GetType(Double), GetType(RadialPanel),
                New FrameworkPropertyMetadata(110.0,
                    FrameworkPropertyMetadataOptions.AffectsMeasure Or
                    FrameworkPropertyMetadataOptions.AffectsArrange))

        Public Property Radius As Double
            Get
                Return CDbl(GetValue(RadiusProperty))
            End Get
            Set(value As Double)
                SetValue(RadiusProperty, value)
            End Set
        End Property

        Protected Overrides Function MeasureOverride(availableSize As Size) As Size
            Dim max As Double = 0
            For Each child As UIElement In InternalChildren
                child.Measure(availableSize)
                max = Math.Max(max, Math.Max(child.DesiredSize.Width, child.DesiredSize.Height))
            Next
            Dim d = Radius * 2 + max * 2
            Return New Size(d, d)
        End Function

        Protected Overrides Function ArrangeOverride(finalSize As Size) As Size
            Dim count = InternalChildren.Count
            If count = 0 Then Return finalSize

            Dim center As New Point(finalSize.Width / 2, finalSize.Height / 2)
            Dim step = 2 * Math.PI / count

            For i As Integer = 0 To count - 1
                Dim child = InternalChildren(i)
                Dim angle = i * step - Math.PI / 2   ' 12시 방향부터 시계 방향

                Dim p As New Point(
                    center.X + Radius * Math.Cos(angle) - child.DesiredSize.Width / 2,
                    center.Y + Radius * Math.Sin(angle) - child.DesiredSize.Height / 2)

                child.Arrange(New Rect(p, child.DesiredSize))
            Next

            Return finalSize
        End Function
    End Class

End Namespace
