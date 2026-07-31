using System.Globalization;
using System.Windows;

namespace Ch36;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        Localization.SetCulture("ko-KR");
    }
}

// 문화권 변경: CurrentCulture/CurrentUICulture 설정 + 리소스 사전 교체
public static class Localization
{
    public static void SetCulture(string name)
    {
        var culture = CultureInfo.GetCultureInfo(name);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        Application.Current.Resources.MergedDictionaries.Clear();
        Application.Current.Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri($"Resources/{name}.xaml", UriKind.Relative)
        });
    }
}
