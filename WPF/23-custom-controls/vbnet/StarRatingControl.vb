Option Strict On

Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Input
Imports System.Windows.Media

' ThemeInfo: 테마 전용 리소스 딕셔너리는 없고(None),
' 일반 리소스 딕셔너리(Themes/Generic.xaml)는 현재 어셈블리에 있음(SourceAssembly)을 선언합니다.
<Assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)>

Namespace Ch23

    ' Control을 상속해 템플릿 기반 커스텀 컨트롤을 만듭니다.
    ' <TemplatePart>는 템플릿에서 반드시 제공해야 하는 요소(PART)의 계약입니다.
    <TemplatePart(Name:="PART_Stars", Type:=GetType(StackPanel))>
    <TemplatePart(Name:="PART_Text", Type:=GetType(TextBlock))>
    Public Class StarRatingControl
        Inherits Control

        Shared Sub New()
            ' 기본 스타일 키를 자신의 타입으로 지정해야 Generic.xaml의 Style이 적용됩니다.
            DefaultStyleKeyProperty.OverrideMetadata(GetType(StarRatingControl),
                New FrameworkPropertyMetadata(GetType(StarRatingControl)))
        End Sub

        Public Shared ReadOnly RatingProperty As DependencyProperty = DependencyProperty.Register(
            "Rating", GetType(Integer), GetType(StarRatingControl),
            New PropertyMetadata(0, AddressOf OnRatingChanged))

        Public Shared ReadOnly MaxRatingProperty As DependencyProperty = DependencyProperty.Register(
            "MaxRating", GetType(Integer), GetType(StarRatingControl),
            New PropertyMetadata(5, AddressOf OnRatingChanged))

        Public Property Rating As Integer
            Get
                Return CInt(GetValue(RatingProperty))
            End Get
            Set(value As Integer)
                SetValue(RatingProperty, value)
            End Set
        End Property

        Public Property MaxRating As Integer
            Get
                Return CInt(GetValue(MaxRatingProperty))
            End Get
            Set(value As Integer)
                SetValue(MaxRatingProperty, value)
            End Set
        End Property

        Private _text As TextBlock

        Private Shared Sub OnRatingChanged(d As DependencyObject, e As DependencyPropertyChangedEventArgs)
            Dim control = TryCast(d, StarRatingControl)
            If control IsNot Nothing Then control.UpdateText()
        End Sub

        Public Overrides Sub OnApplyTemplate()
            MyBase.OnApplyTemplate()

            ' TemplatePart로 선언한 PART 요소를 찾아 별 버튼을 구성합니다.
            Dim stars = TryCast(GetTemplateChild("PART_Stars"), StackPanel)
            If stars Is Nothing Then Return

            stars.Children.Clear()
            For i As Integer = 1 To MaxRating
                Dim index = i ' 루프 변수 캡처 문제 방지
                Dim star = New Button() With {
                    .Content = "★",
                    .FontSize = 18,
                    .Tag = index,
                    .Margin = New Thickness(2),
                    .Background = Brushes.Transparent,
                    .BorderThickness = New Thickness(0),
                    .Cursor = Cursors.Hand
                }
                AddHandler star.Click, Sub(s, e) Rating = index
                stars.Children.Add(star)
            Next

            _text = TryCast(GetTemplateChild("PART_Text"), TextBlock)
            UpdateText()
        End Sub

        Private Sub UpdateText()
            If _text IsNot Nothing Then
                _text.Text = $"{Rating} / {MaxRating}"
            End If
        End Sub
    End Class

End Namespace
