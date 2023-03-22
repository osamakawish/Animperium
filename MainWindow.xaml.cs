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

        // Add animation to the storyboard -> Switch to this approach in StoryboardAnimation.cs
        var doubleAnimationUsingKeyFrames = new DoubleAnimationUsingKeyFrames {
            KeyFrames = new DoubleKeyFrameCollection {
                new LinearDoubleKeyFrame(43, KeyTime.FromTimeSpan(TimeSpan.Zero)),
                new LinearDoubleKeyFrame(266, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(5))),
                new SplineDoubleKeyFrame(400, KeyTime.FromTimeSpan(TimeSpan.FromSeconds(10)), new KeySpline(0, 0.3, 0.5, 0.7))
            }
        };
        _storyboard.Children.Add(doubleAnimationUsingKeyFrames);
        Storyboard.SetTarget(doubleAnimationUsingKeyFrames, rect);
        Storyboard.SetTargetProperty(doubleAnimationUsingKeyFrames, new PropertyPath(Canvas.TopProperty));

        rect.Loaded += delegate
        {
            _storyboard.Begin(this, true);
            //_storyboard.Pause(this);

            // Jump to 2 seconds in.
            //_storyboard.SeekAlignedToLastTick(this, TimeSpan.FromSeconds(2), TimeSeekOrigin.BeginTime);
        };
    }
}