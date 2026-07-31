Option Strict On

Imports System
Imports System.ComponentModel
Imports System.Globalization
Imports System.Runtime.CompilerServices
Imports System.Windows
Imports System.Windows.Data
Imports System.Windows.Media

Namespace Ch28

    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
            DataContext = New MainViewModel()
        End Sub
    End Class

    Public Class MainViewModel
        Implements INotifyPropertyChanged

        Private _active As Boolean = True
        Private _price As Decimal? = Nothing
        Private _score As Double = 75.0

        Public Property Active As Boolean
            Get
                Return _active
            End Get
            Set(value As Boolean)
                _active = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Price As Decimal?
            Get
                Return _price
            End Get
            Set(value As Decimal?)
                _price = value
                OnPropertyChanged()
            End Set
        End Property

        Public Property Score As Double
            Get
                Return _score
            End Get
            Set(value As Double)
                _score = value
                OnPropertyChanged()
            End Set
        End Property

        Public ReadOnly Property Min As Double
            Get
                Return 0
            End Get
        End Property

        Public ReadOnly Property Max As Double
            Get
                Return 100
            End Get
        End Property

        Public Event PropertyChanged As PropertyChangedEventHandler _
            Implements INotifyPropertyChanged.PropertyChanged

        Protected Sub OnPropertyChanged(<CallerMemberName> Optional name As String = Nothing)
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(name))
        End Sub
    End Class

    ' bool을 브러시로. ConverterParameter로 "참색,거짓색"을 외부에서 주입.
    Public Class BooleanToBrushConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object,
                                culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim parts = CStr(parameter).Split(","c)
            Dim brush = If(DirectCast(value, Boolean), parts(0), parts(1))
            Return New SolidColorBrush(CType(ColorConverter.ConvertFromString(brush), Color))
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object,
                                    culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotSupportedException()
        End Function
    End Class

    ' null 여부로 Visibility 결정. ConverterParameter="true"면 null일 때 표시.
    Public Class NullToVisibilityConverter
        Implements IValueConverter

        Public Function Convert(value As Object, targetType As Type, parameter As Object,
                                culture As CultureInfo) As Object Implements IValueConverter.Convert
            Dim showWhenNull = String.Equals(CStr(parameter), "true", StringComparison.OrdinalIgnoreCase)
            Dim visible = (value Is Nothing) = showWhenNull
            Return If(visible, Visibility.Visible, Visibility.Collapsed)
        End Function

        Public Function ConvertBack(value As Object, targetType As Type, parameter As Object,
                                    culture As CultureInfo) As Object Implements IValueConverter.ConvertBack
            Throw New NotSupportedException()
        End Function
    End Class

    ' 여러 값을 조합해 범위 이탈 경고 표시.
    Public Class RangeWarningConverter
        Implements IMultiValueConverter

        Public Function Convert(values() As Object, targetType As Type, parameter As Object,
                                culture As CultureInfo) As Object Implements IMultiValueConverter.Convert
            Dim v = Convert.ToDouble(values(0))
            Dim mn = Convert.ToDouble(values(1))
            Dim mx = Convert.ToDouble(values(2))
            Return If(v < mn OrElse v > mx, Visibility.Visible, Visibility.Collapsed)
        End Function

        Public Function ConvertBack(value As Object, targetTypes() As Type, parameter As Object,
                                    culture As CultureInfo) As Object() Implements IMultiValueConverter.ConvertBack
            Throw New NotSupportedException()
        End Function
    End Class

End Namespace
