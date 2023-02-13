using System;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Animperium.Controls;

namespace Animperium.Graphics;

/// <summary>
/// Extension methods provided for making animations on shapes using the relative coordinate systems easier.
/// </summary>
public static class ShapeExtensions
{
    internal static AnimationCanvas GetAnimationCanvas(this Shape shape)
        => AnimationCanvas.ShapeToAssociatedCanvas[
            AnimationCanvas.ShapeToAssociatedCanvas.Keys.First(set => set.Contains(shape))];

    public static void SetShapeRegion(this Shape shape, Rect region)
        => shape.GetAnimationCanvas().SetShapeRegion(shape, region);

    public static Shape Create<TShape>(double strokeThickness = 1, SolidColorBrush? stroke = null, SolidColorBrush? fill = null)
        where TShape : Shape, new()
        => AnimationCanvas.Current!.AddShape<TShape>(
            relativeSize: (0, 0),
            strokeThickness: strokeThickness,
            strokeColor: stroke ?? Brushes.Black,
            fillColor: fill ?? Brushes.Black);
}