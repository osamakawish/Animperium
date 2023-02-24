using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Animperium.Controls;
using Animperium.Essentials;

namespace Animperium.Graphics;

/// <summary>
/// Extension methods provided for making animations on shapes using the relative coordinate systems easier.
/// </summary>
public static class ShapeExtensions
{
    internal static AnimationCanvas GetAnimationCanvas(this Shape shape)
        => AnimationCanvas.ShapeToAssociatedCanvas[
            AnimationCanvas.ShapeToAssociatedCanvas.Keys.First(set => set.Contains(shape))];

    public static void SetShapeRegion(this Shape shape, Point point1, Point point2)
        => shape.GetAnimationCanvas().SetShapeRegion(shape, new Rect(point1, point2));

    public static TShape Create<TShape>(
        Double2D? relativePosition = null,
        double strokeThickness = 1,
        SolidColorBrush? stroke = null,
        SolidColorBrush? fill = null)
            where TShape : Shape, new()
        => AnimationCanvas.Current!.AddShape<TShape>(
                relativePosition ?? (0, 0),
                (0, 0),
                strokeThickness,
                stroke ?? Brushes.Black,
                fill ?? Brushes.Transparent);

    /// <summary>
    /// Actual top left of the shape's position, ignoring stroke thickness.
    /// </summary>
    /// <param name="shape"></param>
    /// <returns>The *actual* top left value position of the shape's position during initialization.</returns>
    internal static Point GetTopLeft(this Shape shape)
        => new(Canvas.GetLeft(shape), Canvas.GetTop(shape));

    public static Rect GetRelativeBounds(this Shape shape)
    {
        var bounds = shape.RenderedGeometry.GetRenderBounds(new Pen(shape.Stroke, shape.StrokeThickness));
        var measure = AnimationCanvas.RelativeMeasure;

        var location = measure.ToRelativeObjectPosition(shape.GetTopLeft());
        Size size = measure.ToRelativeObjectSize(bounds.Size);

        return new Rect(location, size.IsEmpty ? new Size(0, 0) : size);
    }

    internal static Path AddArc(this AnimationCanvas canvas, Rect rect, Double2D angleRange)
        { var radii = (Double2D)rect.Size / 2; return AddArc(canvas, rect.Location + radii, radii, angleRange); }

    // TODO: Debug
    internal static Path AddArc(this AnimationCanvas canvas, Point ellipseCenter, Double2D ellipseRadii,
        Double2D angleRange)
    {
        var arc = canvas.AddShape<Path>((Double2D)ellipseCenter - ellipseRadii, ellipseRadii * 2);
        
        ellipseRadii = AnimationCanvas.RelativeMeasure.ToActualObjectSize(ellipseRadii);
        Double2D EllipsePoint(double angle)
            => ellipseRadii * (1 + Double2D.ToCirclePoint(angle));

        var arcSegment = new ArcSegment(
            point: EllipsePoint(angleRange.Y),
            size: ellipseRadii, 0,
            isLargeArc: Math.Abs(angleRange.X - angleRange.Y) >= Math.PI,
            sweepDirection: angleRange.Y > angleRange.X ? SweepDirection.Counterclockwise : SweepDirection.Clockwise,
            isStroked: true);

        var pathFigure = new PathFigure
        {
            StartPoint = EllipsePoint(angleRange.X),
            Segments = new PathSegmentCollection { arcSegment }
        };

        arc.Data = new PathGeometry(new[] { pathFigure });

        return arc;
    }
}