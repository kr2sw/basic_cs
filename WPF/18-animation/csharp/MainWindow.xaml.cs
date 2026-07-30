using System.Windows;
using System.Windows.Media.Animation;

namespace Ch18;

public partial class MainWindow : Window
{
    public MainWindow() => InitializeComponent();

    private void FadeIn_Click(object sender, RoutedEventArgs e)
    {
        var storyboard = (Storyboard)Resources["FadeInStoryboard"];
        storyboard.Begin(animBox);
    }

    private void Scale_Click(object sender, RoutedEventArgs e)
    {
        var anim = new DoubleAnimation
        {
            From = 1.0,
            To = 2.0,
            Duration = new Duration(System.TimeSpan.FromSeconds(0.5)),
            AutoReverse = true
        };
        scaleTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleXProperty, anim);
        scaleTransform.BeginAnimation(System.Windows.Media.ScaleTransform.ScaleYProperty, anim);
    }

    private void Rotate_Click(object sender, RoutedEventArgs e)
    {
        var anim = new DoubleAnimation
        {
            From = 0,
            To = 360,
            Duration = new Duration(System.TimeSpan.FromSeconds(1)),
            RepeatBehavior = new RepeatBehavior(2)
        };
        rotateTransform.BeginAnimation(System.Windows.Media.RotateTransform.AngleProperty, anim);
    }

    private void Color_Click(object sender, RoutedEventArgs e)
    {
        var anim = new System.Windows.Media.Animation.ColorAnimation
        {
            From = System.Windows.Media.Colors.DodgerBlue,
            To = System.Windows.Media.Colors.Crimson,
            Duration = new Duration(System.TimeSpan.FromSeconds(1)),
            AutoReverse = true
        };
        animBox.Background?.BeginAnimation(System.Windows.Media.SolidColorBrush.ColorProperty, anim);
    }

    private void Reset_Click(object sender, RoutedEventArgs e)
    {
        animBox.Opacity = 1;
        scaleTransform.ScaleX = 1;
        scaleTransform.ScaleY = 1;
        rotateTransform.Angle = 0;
        animBox.Background = new System.Windows.Media.SolidColorBrush(System.Windows.Media.Colors.DodgerBlue);
    }
}
