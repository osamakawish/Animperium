using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Animperium.Essentials;
using Animperium.FileData;

namespace Animperium.Controls.TimelinePropertyCanvas;

public partial class TimelinePropertyCanvas
{
    private double CanvasKeyframeMargin => (ActualHeight - TimelineCanvas.ActualHeight) / YAxis.KeyframeDivisionConstant;

    private double GetLeft(uint frame) => Canvas.GetLeft(FrameMarkers[frame]);

    private double GetLeft(TimeSpan timespan) => GetLeft(GetFrame(timespan));

    internal Double2D DoubleRange => AssociatedFile.DoubleRange;

    /// <param name="actualValue"></param>
    /// <returns>The <see cref="Canvas.BottomProperty"/> for a keyframe given its relative value to the two extremes.</returns>
    private double GetCanvasBottom(double actualValue)
    {
        var value =
            actualValue <= DoubleRange.X ? DoubleRange.X :
            actualValue >= DoubleRange.Y ? DoubleRange.Y :
            actualValue;
        
        double Func(double x) => Math.Sign(x) * Math.Log(x * x + 1);
        value = Func(value);
        var range = DoubleRange.With(Func);

        var transform = Transform1D.FromMapping(
            new Mapping<double>(range.X, CanvasKeyframeMargin),
            new Mapping<double>(range.Y, ActualHeight - TimelineCanvas.ActualHeight - CanvasKeyframeMargin));
        return transform[value];
    }

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
        KeyframeType.Constant  => ConstantKeyframe,
        KeyframeType.Linear    => LinearKeyframe,
        KeyframeType.Quadratic => QuadraticKeyframe,
        KeyframeType.Cubic     => CubicKeyframe,
        KeyframeType.Custom    => CustomKeyframe,
        _ => throw new ArgumentOutOfRangeException(nameof(keyframeType), keyframeType, null)
    };

    internal void AddKeyframe(KeyframeType keyframeType, TimeSpan? time = null, double value=0)
    {
        time ??= CurrentTime;

        var path = GetPath(keyframeType);
        var button = new Button { Content = path };
        button.Click += (_, e) =>
        {
            // Set selected keyframe to this keyframe.
            
        };
        KeyframeCanvas.Children.Add(button);

        Canvas.SetLeft(path, GetLeft(time.Value));
        Canvas.SetBottom(path, GetCanvasBottom(value));

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

file static class YAxis
{
    internal static readonly double KeyframeDivisionConstant = 8;
}
