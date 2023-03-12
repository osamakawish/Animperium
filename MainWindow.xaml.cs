using System;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
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

        Loaded += (_, _) => TestAnimate();
    }

    /// <summary>
    /// Do NOT delete this method. It covers a basic example regarding how to implement animations.
    /// </summary>
    private void TestAnimate()
    {
        // Create rectangle on canvas.
        var rect = new Rectangle
        {
            //Name = "Rect", // Line not required
            Width = 200,
            Height = 80,
            Fill = Brushes.Brown
        };
        //RegisterName(rect.Name, rect); // Line not required.
        AnimationCanvas.AddShape(rect, (0, 0), (4, 4));

        // Add animation to the storyboard
        var doubleAnim = new DoubleAnimation(43, 266d, new Duration(TimeSpan.FromSeconds(5)));
        _storyboard.Children.Add(doubleAnim);
        Storyboard.SetTarget(doubleAnim, rect);
        Storyboard.SetTargetProperty(doubleAnim, new PropertyPath(Canvas.TopProperty));
        rect.Loaded += delegate
        {
            _storyboard.Begin(this, true);
            //_storyboard.Pause(this);

            // Jump to 2 seconds in.
            //_storyboard.SeekAlignedToLastTick(this, TimeSpan.FromSeconds(2), TimeSeekOrigin.BeginTime);
        };
    }
}