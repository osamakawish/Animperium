﻿using System;
using System.Linq;
using System.Windows;
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

    public static Shape Create<TShape>() where TShape : Shape
    {
        throw new NotImplementedException();
    }
}