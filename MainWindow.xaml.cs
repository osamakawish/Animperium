using System;
using System.ComponentModel;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace MathAnim;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow
{
    private readonly Storyboard _storyboard = new();

    public MainWindow()
    {
        InitializeComponent();
        
        TestAnimate();
    }

    private void TestAnimate()
    {
        // Create rectangle on canvas.
        var rect = new Rectangle
        {
            Name = "Rect",
            Width = 200,
            Height = 80,
            Fill = Brushes.Brown
        };
        RegisterName(rect.Name, rect);
        MainCanvas.Children.Add(rect);
        Canvas.SetLeft(rect, 110); Canvas.SetTop(rect, 43);

        // Add animation to the storyboard
        var doubleAnim = new DoubleAnimation(43, 166d, new Duration(TimeSpan.FromSeconds(5)));
        _storyboard.Children.Add(doubleAnim);
        Storyboard.SetTarget(doubleAnim, rect);
        Storyboard.SetTargetProperty(doubleAnim, new PropertyPath(Canvas.TopProperty));
        rect.Loaded += (_, _) => _storyboard.Begin(this);
    }
}