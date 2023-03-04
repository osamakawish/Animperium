using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Animperium.Controls;

namespace Animperium.Graphics;

// TODO: Implement details such as origin/rotation/scale/shear later.
public class SelectionRectangle : Shape
{
    internal SelectionRectColorTheme ColorTheme { get; set; }
    internal RotationButtons RotationButtons { get; set; }
    internal Point StartPoint { get; set; }
    internal Point EndPoint { get; set; }

    public SelectionRectVisibleButtons Visible { get; internal set; }

    protected override Geometry DefiningGeometry {
        get {
            return new PathGeometry();
        }
    }

    private Path Origin { get; init; }
    private Path Scale { get; init; }
    private Path Rotation { get; init; }
    private Path Shear { get; init; }
}

public enum SelectionRectVisibleButtons : byte
{
    None     = 0b0000,
    Origin   = 0b0001,
    Scale    = 0b0010,
    Rotation = 0b0100,
    Shear    = 0b1000
}

internal record RotationArcButton(
    AnimationCanvas Canvas,
    Point Center,
    double Radius,
    double BaseAngle,
    double AngleSpan = RotationArcButton.ThreePiOverFour)
{
    internal const double ThreePiOverFour = 3 * Math.PI / 4;

    internal double AngleSpan { get; set; } = AngleSpan;
    internal Point Center { get => Arc.Center; set => Arc.Center = value; }

    internal double AdditionalAngle {
        get => Arc.StartAngle - BaseAngle;
        set {
            Arc.StartAngle = BaseAngle + value;
            Arc.EndAngle = Arc.StartAngle + AngleSpan;
        }
    }

    internal Arc Arc { get; } = Canvas.AddArc(Center, (Radius, Radius), (BaseAngle, BaseAngle + Math.PI), true);
}

public record SelectionRectColorTheme(
    SolidColorBrush Brush,
    DoubleCollection StrokeDashArray,
    double Thickness = 1.5,
    double Offset = 0);

internal record RotationButtons(
    RotationArcButton TopLeft,
    RotationArcButton TopRight,
    RotationArcButton BottomLeft,
    RotationArcButton BottomRight)
{
    private double _additionalRotation;

    internal void ForEach(Action<RotationArcButton> action)
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

    internal double AdditionalRotation {
        get => _additionalRotation;
        set {
            ForEach(x => x.AdditionalAngle = value);
            _additionalRotation = value;
        }
    }
}