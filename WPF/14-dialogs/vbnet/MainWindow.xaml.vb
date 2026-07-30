Option Strict On

Imports Microsoft.Win32
Imports System.Windows

Namespace Ch14
    Public Partial Class MainWindow
        Inherits Window

        Public Sub New()
            InitializeComponent()
        End Sub

        Private Sub MessageBox_Click(sender As Object, e As RoutedEventArgs)
            Dim result = MessageBox.Show("계속하시겠습니까?", "확인",
                MessageBoxButton.YesNoCancel, MessageBoxImage.Question)
            resultBox.Text = $"MessageBox 결과: {result}"
        End Sub

        Private Sub OpenFile_Click(sender As Object, e As RoutedEventArgs)
            Dim dialog As New OpenFileDialog With {
                .Title = "파일 열기",
                .Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일 (*.*)|*.*",
                .Multiselect = True
            }

            If dialog.ShowDialog() = True Then
                resultBox.Text = $"선택한 파일:{vbCrLf}{String.Join(vbCrLf, dialog.FileNames)}"
            Else
                resultBox.Text = "파일 선택이 취소되었습니다."
            End If
        End Sub

        Private Sub SaveFile_Click(sender As Object, e As RoutedEventArgs)
            Dim dialog As New SaveFileDialog With {
                .Title = "파일 저장",
                .Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일 (*.*)|*.*",
                .FileName = "document.txt"
            }

            If dialog.ShowDialog() = True Then
                resultBox.Text = $"저장 경로: {dialog.FileName}"
            Else
                resultBox.Text = "저장이 취소되었습니다."
            End If
        End Sub

        Private Sub CustomDialog_Click(sender As Object, e As RoutedEventArgs)
            Dim dialog As New InputDialog("사용자 입력", "이름을 입력하세요:")
            If dialog.ShowDialog() = True Then
                resultBox.Text = $"입력한 이름: {dialog.InputText}"
            Else
                resultBox.Text = "대화상자가 취소되었습니다."
            End If
        End Sub
    End Class

    Public Class InputDialog
        Inherits Window

        Private ReadOnly _textBox As New Controls.TextBox()

        Public ReadOnly Property InputText As String
            Get
                Return _textBox.Text
            End Get
        End Property

        Public Sub New(title As String, prompt As String)
            Me.Title = title
            Me.Width = 350
            Me.Height = 180
            Me.WindowStartupLocation = WindowStartupLocation.CenterOwner
            Me.Owner = System.Windows.Application.Current.MainWindow

            Dim grid As New Controls.Grid()
            grid.Margin = New Thickness(15)
            grid.RowDefinitions.Add(New Controls.RowDefinition With {.Height = GridLength.Auto})
            grid.RowDefinitions.Add(New Controls.RowDefinition With {.Height = GridLength.Auto})
            grid.RowDefinitions.Add(New Controls.RowDefinition With {.Height = GridLength.Auto})

            Dim label As New Controls.Label With {
                .Content = prompt,
                .Margin = New Thickness(0, 0, 0, 5)
            }
            Controls.Grid.SetRow(label, 0)
            grid.Children.Add(label)

            _textBox.Margin = New Thickness(0, 0, 0, 10)
            _textBox.Padding = New Thickness(5)
            Controls.Grid.SetRow(_textBox, 1)
            grid.Children.Add(_textBox)

            Dim btnPanel As New Controls.StackPanel With {
                .Orientation = Controls.Orientation.Horizontal,
                .HorizontalAlignment = HorizontalAlignment.Right
            }

            Dim okBtn As New Controls.Button With {
                .Content = "확인",
                .Padding = New Thickness(15),
                .Margin = New Thickness(5),
                .IsDefault = True
            }
            AddHandler okBtn.Click, Sub(s, args) 
                DialogResult = True
                Close()
            End Sub

            Dim cancelBtn As New Controls.Button With {
                .Content = "취소",
                .Padding = New Thickness(15),
                .IsCancel = True
            }
            AddHandler cancelBtn.Click, Sub(s, args)
                DialogResult = False
                Close()
            End Sub

            btnPanel.Children.Add(okBtn)
            btnPanel.Children.Add(cancelBtn)
            Controls.Grid.SetRow(btnPanel, 2)
            grid.Children.Add(btnPanel)

            Content = grid
        End Sub
    End Class
End Namespace
