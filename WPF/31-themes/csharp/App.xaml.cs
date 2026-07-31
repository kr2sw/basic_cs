using System.Windows;

namespace Ch31;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        ApplyTheme("Themes/Light.xaml");
    }

    // 실행 중 리소스 사전을 통째로 교체해 테마를 전환한다
    public static void ApplyTheme(string path)
    {
        Resources.MergedDictionaries.Clear();
        Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri(path, UriKind.Relative)
        });
    }
}
