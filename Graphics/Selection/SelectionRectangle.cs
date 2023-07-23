using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Animperium.Controls;

namespace Animperium.Graphics.Selection;

public class SelectionRectangle
{
    internal AnimationCanvas AnimationCanvas { get; init; } = null!;
    internal SelectionRectColorTheme ColorTheme { get; set; }
    internal RotationButtons RotationButtons { get; set; }
    internal Point StartPoint { get; set; }
    internal Point EndPoint { get; set; }

    internal double Width => StartPoint.X - EndPoint.X;
    internal double Height => StartPoint.Y - EndPoint.Y;

    public SelectionRectVisibleButtons VisibleButtons { get; internal set; }
    
    private Path ScaleButtonsShape()
    {
        return null;
    }

    private Shape RotationButtonsShape()
    {
        var radius = 3;
        Path ArcShape(Point center, double startAngle, double endAngle)
        {
            var path = new Path();

            var figure = new PathFigure() { StartPoint = new Point(-radius, 0) };
            figure.Segments.Add(new ArcSegment(new Point(0, radius), new Size(6, 6), endAngle - startAngle, true, SweepDirection.Counterclockwise, true));

            return path;
        }


        return null;
    }

    private Path Origin { get; init; }
    private Path Scale { get; init; }
    private Path Rotation { get; init; }
    private Path Shear { get; init; }
}

public enum SelectionRectVisibleButtons : byte
{
    None = 0b0000,
    Origin = 0b0001,
    Scale = 0b0010,
    Rotation = 0b0100,
    Shear = 0b1000
}

public record SelectionRectColorTheme(
    SolidColorBrush Brush,
    DoubleCollection StrokeDashArray,
    double Thickness = 1.5,
    double Offset = 0);