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

    private bool TryGetCanvasBottom(double actualValue, out double canvasBottom)
    {
        var value =
            actualValue <= DoubleRange.X ? DoubleRange.X :
            actualValue >= DoubleRange.Y ? DoubleRange.Y :
            actualValue;

        // Q: Need to apply func so that it's about logarithmic, but also with 0 in center.
        double Func(double x)
        {
            var sign = Math.Sign(x);
            var log = Math.Log(Math.Abs(x) + 1);
            return sign * log;
        }
        value = Func(value);
        var range = DoubleRange.With(Func);

        var transform = Transform1D.FromMapping(
            new Mapping<double>(range.X, CanvasKeyframeMargin),
            new Mapping<double>(range.Y, ActualHeight - TimelineCanvas.ActualHeight - CanvasKeyframeMargin));
        canvasBottom = transform[value];
        
        return DoubleRange.BetweenXAndY(actualValue);
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

    internal void AddKeyframe(KeyframeType keyframeType, TimeSpan? time = null)
    {
        time ??= CurrentTime;

        var path = GetPath(keyframeType);
        KeyframeCanvas.Children.Add(path);
        
        TryGetCanvasBottom(0, out var bottom);
        Canvas.SetLeft(path, GetLeft(time.Value));
        Canvas.SetBottom(path, bottom);

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