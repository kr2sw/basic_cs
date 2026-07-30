using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows;

namespace Ch20;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        DataContext = this;
    }

    public string AppName => Assembly.GetExecutingAssembly().GetName().Name ?? "WPF App";
    public string Version => Assembly.GetExecutingAssembly().GetName().Version?.ToString() ?? "1.0.0.0";
    public string Runtime => RuntimeEnvironment.GetSystemVersion();
    public string OsInfo => Environment.OSVersion.ToString();
}
