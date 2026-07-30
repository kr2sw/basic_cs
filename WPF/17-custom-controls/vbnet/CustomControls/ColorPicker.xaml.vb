Option Strict On

Imports System.Windows
Imports System.Windows.Controls
Imports System.Windows.Media

Namespace Ch17
    Public Partial Class ColorPicker
        Inherits UserControl

        Public Shared ReadOnly ColorChangedEvent As RoutedEvent =
            EventManager.RegisterRoutedEvent("ColorChanged", RoutingStrategy.Bubble,
                GetType(RoutedEventHandler), GetType(ColorPicker))

        Public Custom Event ColorChanged As RoutedEventHandler
            AddHandler(value As RoutedEventHandler)
                Me.AddHandler(ColorChangedEvent, value)
            End AddHandler
            RemoveHandler(value As RoutedEventHandler)
                Me.RemoveHandler(ColorChangedEvent, value)
            End RemoveHandler
            RaiseEvent(sender As Object, e As RoutedEventArgs)
                Me.OnColorChanged(e)
            End RaiseEvent
        End Event

        Public Shared ReadOnly SelectedColorProperty As DependencyProperty =
            DependencyProperty.Register("SelectedColor", GetType(Color), GetType(ColorPicker),
                New PropertyMetadata(Colors.Black))

        Public Property SelectedColor As Color
            Get
                Return DirectCast(GetValue(SelectedColorProperty), Color)
            End Get
            Set(value As Color)
                SetValue(SelectedColorProperty, value)
            End Set
        End Property

        Public Sub New()
            InitializeComponent()

            AddHandler redSlider.ValueChanged, AddressOf Slider_ValueChanged
            AddHandler greenSlider.ValueChanged, AddressOf Slider_ValueChanged
            AddHandler blueSlider.ValueChanged, AddressOf Slider_ValueChanged
        End Sub

        Private Sub Slider_ValueChanged(sender As Object, e As RoutedPropertyChangedEventArgs(Of Double))
            Dim color As Color = Color.FromRgb(CByte(redSlider.Value), CByte(greenSlider.Value), CByte(blueSlider.Value))
            SelectedColor = color
            preview.Background = New SolidColorBrush(color)
            redValue.Text = CByte(redSlider.Value).ToString()
            greenValue.Text = CByte(greenSlider.Value).ToString()
            blueValue.Text = CByte(blueSlider.Value).ToString()
            MyBase.RaiseEvent(New RoutedEventArgs(ColorChangedEvent))
        End Sub

        Protected Overridable Sub OnColorChanged(e As RoutedEventArgs)
        End Sub
    End Class
End Namespace
