using Microsoft.Win32;
using System.Windows;

namespace Ch14;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    private void MessageBox_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("계속하시겠습니까?", "확인",
            MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
        resultBox.Text = $"MessageBox 결과: {result}";
    }

    private void OpenFile_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new OpenFileDialog
        {
            Title = "파일 열기",
            Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일 (*.*)|*.*",
            Multiselect = true
        };

        if (dialog.ShowDialog() == true)
        {
            resultBox.Text = $"선택한 파일:\n{string.Join("\n", dialog.FileNames)}";
        }
        else
        {
            resultBox.Text = "파일 선택이 취소되었습니다.";
        }
    }

    private void SaveFile_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new SaveFileDialog
        {
            Title = "파일 저장",
            Filter = "텍스트 파일 (*.txt)|*.txt|모든 파일 (*.*)|*.*",
            FileName = "document.txt"
        };

        if (dialog.ShowDialog() == true)
        {
            resultBox.Text = $"저장 경로: {dialog.FileName}";
        }
        else
        {
            resultBox.Text = "저장이 취소되었습니다.";
        }
    }

    private void CustomDialog_Click(object sender, RoutedEventArgs e)
    {
        var dialog = new InputDialog("사용자 입력", "이름을 입력하세요:");
        if (dialog.ShowDialog() == true)
        {
            resultBox.Text = $"입력한 이름: {dialog.InputText}";
        }
        else
        {
            resultBox.Text = "대화상자가 취소되었습니다.";
        }
    }
}

public class InputDialog : Window
{
    private readonly System.Windows.Controls.TextBox _textBox;

    public string InputText => _textBox.Text;

    public InputDialog(string title, string prompt)
    {
        Title = title;
        Width = 350;
        Height = 180;
        WindowStartupLocation = WindowStartupLocation.CenterOwner;
        Owner = Application.Current.MainWindow;

        var grid = new System.Windows.Controls.Grid { Margin = new Thickness(15) };
        grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = System.Windows.GridLength.Auto });
        grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = System.Windows.GridLength.Auto });
        grid.RowDefinitions.Add(new System.Windows.Controls.RowDefinition { Height = System.Windows.GridLength.Auto });

        var label = new System.Windows.Controls.Label { Content = prompt, Margin = new Thickness(0, 0, 0, 5) };
        System.Windows.Controls.Grid.SetRow(label, 0);
        grid.Children.Add(label);

        _textBox = new System.Windows.Controls.TextBox { Margin = new Thickness(0, 0, 0, 10), Padding = new Thickness(5) };
        System.Windows.Controls.Grid.SetRow(_textBox, 1);
        grid.Children.Add(_textBox);

        var btnPanel = new System.Windows.Controls.StackPanel
        {
            Orientation = System.Windows.Controls.Orientation.Horizontal,
            HorizontalAlignment = System.Windows.HorizontalAlignment.Right
        };

        var okBtn = new System.Windows.Controls.Button
        {
            Content = "확인",
            Padding = new Thickness(15),
            Margin = new Thickness(5),
            IsDefault = true
        };
        okBtn.Click += (s, args) => { DialogResult = true; Close(); };

        var cancelBtn = new System.Windows.Controls.Button
        {
            Content = "취소",
            Padding = new Thickness(15),
            IsCancel = true
        };
        cancelBtn.Click += (s, args) => { DialogResult = false; Close(); };

        btnPanel.Children.Add(okBtn);
        btnPanel.Children.Add(cancelBtn);
        System.Windows.Controls.Grid.SetRow(btnPanel, 2);
        grid.Children.Add(btnPanel);

        Content = grid;
    }
}
