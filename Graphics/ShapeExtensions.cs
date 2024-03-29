﻿using System;
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
    public static Rect ZeroRect { get; } = new(0, 0, 0, 0);

    internal static AnimationCanvas GetAnimationCanvas(this Shape shape)
        => AnimationCanvas.ShapeToAssociatedCanvas[shape];

    public static void SetDecorativeShapeRegion(this Shape shape, Point point1, Point point2)
        => AnimationCanvas.RedrawFromPoints(shape, point1, point2);

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

    internal static Arc AddArc(this AnimationCanvas canvas, Point ellipseCenter, Double2D ellipseRadii,
        Double2D angleRange, bool isDecorative=false)
    {
        var arc = canvas.AddShape<Arc>((Double2D)ellipseCenter - ellipseRadii, ellipseRadii * 2, isDecorative:isDecorative);
        arc.StartAngle = angleRange.X; arc.EndAngle = angleRange.Y;

        return arc;
    }
}