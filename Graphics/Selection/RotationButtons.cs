using System;
using System.Windows;

namespace Animperium.Graphics.Selection;

internal record RotationButtons(
    RotationButton TopLeft,
    RotationButton TopRight,
    RotationButton BottomLeft,
    RotationButton BottomRight)
{
    private double _additionalRotation;

    internal void ForEach(Action<RotationButton> action)
    {
        action(TopLeft);
        action(TopRight);
        action(BottomLeft);
        action(BottomRight);
    }

    // TODO: Again, handle the angle ranges here as well.
    internal void SetRect(Rect rect)
        => (TopLeft.Center, TopRight.Center, BottomLeft.Center, BottomRight.Center)
            = (rect.TopLeft, rect.TopRight, rect.BottomLeft, rect.BottomRight);

    // TODO: Need to handle angles based on the points.
    internal void SetPoints(Point topLeft, Point topRight, Point bottomLeft, Point bottomRight)
        => (TopLeft.Center, TopRight.Center, BottomLeft.Center, BottomRight.Center)
            = (topLeft, topRight, bottomLeft, bottomRight);

    internal double AdditionalRotation
    {
        get => _additionalRotation;
        set
        {
            ForEach(x => x.AdditionalAngle = value);
            _additionalRotation = value;
        }
    }
}