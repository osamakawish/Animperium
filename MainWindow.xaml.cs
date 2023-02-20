using System.Windows;
using System.Windows.Media.Animation;
using Animperium.Graphics;

namespace Animperium;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly Storyboard _storyboard = new();

    // Audio tools and raster/vector/video tools go to different ui elements.
    internal VisualAnimationTool VisualAnimationTool => ToolView.VisualAnimationTool;

    public MainWindow()
    {
        InitializeComponent();

        WindowState = WindowState.Maximized;
        ToolView.ToolChanged += (_, tool) => AnimationCanvas.VisualAnimationTool = tool;

        //TestAnimate();
    }

    //private void TestAnimate()
    //{
    //    // Create rectangle on canvas.
    //    var rect = new Rectangle
    //    {
    //        Name = "Rect",
    //        Width = 200,
    //        Height = 80,
    //        Fill = Brushes.Brown
    //    };
    //    RegisterName(rect.Name, rect);
    //    MainCanvas.Children.Add(rect);
    //    Canvas.SetLeft(rect, 110); Canvas.SetTop(rect, 43);

    //    // Add animation to the storyboard
    //    var doubleAnim = new DoubleAnimation(43, 166d, new Duration(TimeSpan.FromSeconds(5)));
    //    _storyboard.Children.Add(doubleAnim);
    //    Storyboard.SetTarget(doubleAnim, rect);
    //    Storyboard.SetTargetProperty(doubleAnim, new PropertyPath(Canvas.TopProperty));
    //    rect.Loaded += (_, _) => _storyboard.Begin(this);
    //}
}