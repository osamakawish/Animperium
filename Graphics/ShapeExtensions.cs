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
    /// <param name="withThickness"></param>
    /// <returns>The *actual* top left value position of the shape's position during initialization.</returns>
    internal static Point GetTopLeft(this Shape shape, double withThickness)
        => new(Canvas.GetLeft(shape) + withThickness, Canvas.GetTop(shape) + withThickness);

    public static Rect GetRelativeBounds(this Shape shape, double withThickness = double.NaN)
    {
        withThickness = double.IsNaN(withThickness) || double.IsInfinity(withThickness) ? shape.StrokeThickness : withThickness;

        var bounds = shape.RenderedGeometry.GetRenderBounds(new Pen(shape.Stroke, shape.StrokeThickness));
        var measure = AnimationCanvas.RelativeMeasure;

        var location = measure.ToRelativeObjectPosition(shape.GetTopLeft(withThickness));
        var size = measure.ToRelativeObjectSize(bounds.Size);
        
        return new Rect(location, new Size(size.X, size.Y));
    }
}