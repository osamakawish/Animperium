using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Animperium.Controls.TimelinePropertyCanvas;

public partial class TimelinePropertyCanvas
{
    private double GetLeft(uint frame) => Canvas.GetLeft(FrameMarkers[frame]);

    private double GetLeft(TimeSpan timespan) => GetLeft(GetFrame(timespan));

    private static Path ConstantKeyframe => new() {
        StrokeThickness = 1,
        Stroke = Brushes.DarkRed,
        Fill = Brushes.DarkOrange
    };

    internal void AddKeyframe(KeyframeType keyframeType, TimeSpan time)
    {
        var constantKeyframe = ConstantKeyframe;
        KeyframeCanvas.Children.Add(constantKeyframe);
        Canvas.SetLeft(constantKeyframe, GetLeft(time));
        Canvas.SetTop(constantKeyframe, 64); // Modify top value as needed.
        constantKeyframe.Data = new EllipseGeometry(new Point(0, 0), 6, 6);
        Panel.SetZIndex(constantKeyframe, 20);
        
    }

    internal void RemoveKeyframe(TimeSpan time)
    {
        // TODO
    }

    internal void AddKeyframe(uint frame)
    {
        // TODO
    }

    internal void RemoveKeyframe(uint frame)
    {
        // TODO
    }
}

public enum KeyframeType : byte { Constant = 0, Linear = 1, Quadratic = 2, Cubic = 3, Custom = 255 }