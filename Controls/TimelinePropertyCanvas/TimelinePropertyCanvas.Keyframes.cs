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
        Fill = Brushes.DarkOrange, 
        Data = new EllipseGeometry(new Point(0, 0), 6, 6)
    };

    private static Path LinearKeyframe => new() {
        StrokeThickness = 1,
        Stroke = Brushes.DarkRed,
        Fill = Brushes.DarkOrange,
        Data = new EllipseGeometry(new Point(0, 0), 6, 6)
    };

    private static Path QuadraticKeyframe => new() {
        StrokeThickness = 1,
        Stroke = Brushes.DarkRed,
        Fill = Brushes.DarkOrange,
        Data = new EllipseGeometry(new Point(0, 0), 6, 6)
    };

    private static Path CubicKeyframe => new() {
        StrokeThickness = 1,
        Stroke = Brushes.DarkRed,
        Fill = Brushes.DarkOrange,
        Data = new EllipseGeometry(new Point(0, 0), 6, 6)
    };

    private static Path CustomKeyframe => new() {
        StrokeThickness = 1,
        Stroke = Brushes.DarkRed,
        Fill = Brushes.DarkOrange,
        Data = new EllipseGeometry(new Point(0, 0), 6, 6)
    };

    private static Path GetPath(KeyframeType keyframeType) => keyframeType switch {
        KeyframeType.Constant => ConstantKeyframe,
        KeyframeType.Linear => LinearKeyframe,
        KeyframeType.Quadratic => QuadraticKeyframe,
        KeyframeType.Cubic => CubicKeyframe,
        KeyframeType.Custom => CustomKeyframe,
        _ => throw new ArgumentOutOfRangeException(nameof(keyframeType), keyframeType, null)
    };

    internal void AddKeyframe(KeyframeType keyframeType, TimeSpan? time = null)
    {
        time ??= CurrentTime;

        var path = GetPath(keyframeType);
        KeyframeCanvas.Children.Add(path);
        Canvas.SetLeft(path, GetLeft(time.Value));
        Canvas.SetTop(path, 64); // Modify top value as needed.
        Panel.SetZIndex(path, 20);
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