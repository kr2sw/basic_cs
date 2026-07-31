using System.Windows;

namespace Ch40;

public partial class App : Application
{
    // 테마 전환 (31장의 패턴 재사용)
    public static void ApplyTheme(string path)
    {
        Resources.MergedDictionaries.Clear();
        Resources.MergedDictionaries.Add(new ResourceDictionary
        {
            Source = new Uri(path, UriKind.Relative)
        });
    }
}
