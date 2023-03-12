using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Shapes;
using Animperium.Controls.TimelinePropertyCanvas;

namespace Animperium.Controls;

/// <summary>
/// Interaction logic for AnimationPanel.xaml
/// </summary>
public partial class AnimationPanel
{
    private KeyframeType _currentKeyframeType = KeyframeType.Constant;
    internal KeyframeType CurrentKeyframeType {
        get => _currentKeyframeType;
        private set {
            foreach (var (toggleButton, keyframeType) in ToggleButtonKeyframes)
                toggleButton.IsChecked = keyframeType == value;

            _currentKeyframeType = value;
        }
    }

    private Dictionary<KeyframeType, Geometry> KeyframeIconInsides { get; } = new();
    private Dictionary<KeyframeType, Geometry> KeyframeIconOutsides { get; } = new();

    private Dictionary<ToggleButton, KeyframeType> ToggleButtonKeyframes { get; }

    public AnimationPanel()
    {
        InitializeComponent();

        ToggleButtonKeyframes = new Dictionary<ToggleButton, KeyframeType> {
            { ConstantToggleButton, KeyframeType.Constant },
            { LinearToggleButton, KeyframeType.Linear },
            { QuadraticToggleButton , KeyframeType.Quadratic },
            { CubicToggleButton, KeyframeType.Cubic },
            { CustomToggleButton , KeyframeType.Custom }
        };

        CreateKeyframeButton.Click += delegate { TimelinePropertyCanvas.AddKeyframe(CurrentKeyframeType); };
        var toggleButtons = new[] {
            ConstantToggleButton, LinearToggleButton, QuadraticToggleButton, CubicToggleButton, CustomToggleButton
        };
        
        foreach (var button in toggleButtons) {
            var keyframeType = ToggleButtonKeyframes[button];

            // Handle click events.
            button.Click += delegate { CurrentKeyframeType = keyframeType; };

            // Draw button icons
            var inner = new Path {
                Name = "Inner",
                Stroke = Brushes.OrangeRed,
                StrokeThickness = 1,
                Data = GetButtonIconInner(keyframeType)
            };
            var outer = new Path
            {
                Name = "Inner",
                Stroke = Brushes.Black,
                StrokeThickness = 1,
                Data = GetButtonIconOuter(keyframeType)
            };
            var canvas = new Canvas {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            canvas.Children.Add(inner); canvas.Children.Add(outer);
            Panel.SetZIndex(outer, 2);

            button.Content = canvas;
        }
    }

    private static Geometry GetButtonIconInner(KeyframeType keyframeType)
        => keyframeType switch {
            KeyframeType.Constant  => new LineGeometry(new Point(-10, 0), new Point(10, 0)),
            KeyframeType.Linear    => new LineGeometry(new Point(-10, 10), new Point(10, -10)),
            KeyframeType.Quadratic => Geometry.Parse("M -8,-10 Q 0,16 8,-10"),
            KeyframeType.Cubic     => Geometry.Parse("M -9,10 C -3,-24 1,24 9,-10"),
            KeyframeType.Custom    => Geometry.Parse("M -9,-6 L 0,3 9,-6"),
            _ => throw new ArgumentOutOfRangeException(nameof(keyframeType), keyframeType, null)
        };

    private static Geometry GetButtonIconOuter(KeyframeType keyframeType)
        => new RectangleGeometry(new Rect(new Point(-10, -10), new Point(10, 10)));
}