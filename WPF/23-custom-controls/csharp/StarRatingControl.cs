using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

// ThemeInfo: 테마 전용 리소스 딕셔너리는 없고(None),
// 일반 리소스 딕셔너리(Themes/Generic.xaml)는 현재 어셈블리에 있음(SourceAssembly)을 선언합니다.
[assembly: ThemeInfo(ResourceDictionaryLocation.None, ResourceDictionaryLocation.SourceAssembly)]

namespace Ch23;

// Control을 상속해 템플릿 기반 커스텀 컨트롤을 만듭니다.
// [TemplatePart]는 템플릿에서 반드시 제공해야 하는 요소(PART)의 계약입니다.
[TemplatePart(Name = "PART_Stars", Type = typeof(StackPanel))]
[TemplatePart(Name = "PART_Text", Type = typeof(TextBlock))]
public class StarRatingControl : Control
{
    static StarRatingControl()
    {
        // 기본 스타일 키를 자신의 타입으로 지정해야 Generic.xaml의 Style이 적용됩니다.
        DefaultStyleKeyProperty.OverrideMetadata(typeof(StarRatingControl),
            new FrameworkPropertyMetadata(typeof(StarRatingControl)));
    }

    public static readonly DependencyProperty RatingProperty = DependencyProperty.Register(
        nameof(Rating), typeof(int), typeof(StarRatingControl),
        new PropertyMetadata(0, OnRatingChanged));

    public static readonly DependencyProperty MaxRatingProperty = DependencyProperty.Register(
        nameof(MaxRating), typeof(int), typeof(StarRatingControl),
        new PropertyMetadata(5, OnRatingChanged));

    public int Rating
    {
        get => (int)GetValue(RatingProperty);
        set => SetValue(RatingProperty, value);
    }

    public int MaxRating
    {
        get => (int)GetValue(MaxRatingProperty);
        set => SetValue(MaxRatingProperty, value);
    }

    private TextBlock? _text;

    private static void OnRatingChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is StarRatingControl control)
        {
            control.UpdateText();
        }
    }

    public override void OnApplyTemplate()
    {
        base.OnApplyTemplate();

        // TemplatePart로 선언한 PART 요소를 찾아 별 버튼을 구성합니다.
        var stars = GetTemplateChild("PART_Stars") as StackPanel;
        if (stars is null) return;

        stars.Children.Clear();
        for (int i = 1; i <= MaxRating; i++)
        {
            int index = i; // 루프 변수 캡처 문제 방지
            var star = new Button
            {
                Content = "★",
                FontSize = 18,
                Tag = index,
                Margin = new Thickness(2),
                Background = Brushes.Transparent,
                BorderThickness = new Thickness(0),
                Cursor = Cursors.Hand,
            };
            star.Click += (_, _) => Rating = index;
            stars.Children.Add(star);
        }

        _text = GetTemplateChild("PART_Text") as TextBlock;
        UpdateText();
    }

    private void UpdateText()
    {
        if (_text is not null)
        {
            _text.Text = $"{Rating} / {MaxRating}";
        }
    }
}
