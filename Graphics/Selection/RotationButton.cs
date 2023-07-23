using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Animperium.Controls;

namespace Animperium.Graphics.Selection;

internal class RotationButton : Shape
{
    [Flags]
    public enum ButtonCorner
    {
        BottomLeft = 0b00,
        BottomRight = 0b10,
        TopLeft = 0b01,
        TopRight = 0b11
    }

    public static RotationButtons RotationButtons => new(
        TopLeft:     new(ButtonCorner.TopLeft),
        TopRight:    new(ButtonCorner.TopRight),
        BottomLeft:  new(ButtonCorner.BottomLeft),
        BottomRight: new(ButtonCorner.BottomRight)
    );

    private readonly ButtonCorner _corner;

    internal Point Center => new(IsRight * Width, IsTop * Height);

    internal double StartAngle => _corner switch {
        ButtonCorner.BottomLeft => -1.5 * Math.PI,
        ButtonCorner.BottomRight => -Math.PI,
        ButtonCorner.TopLeft => 0,
        ButtonCorner.TopRight => -0.5 * Math.PI,
        _ => throw new ArgumentOutOfRangeException()
    };

    internal double EndAngle => _corner switch {
        ButtonCorner.BottomLeft => 2 * Math.PI,
        ButtonCorner.BottomRight => 0.5 * Math.PI,
        ButtonCorner.TopLeft => 1.5 * Math.PI,
        ButtonCorner.TopRight => Math.PI,
        _ => throw new ArgumentOutOfRangeException()
    };

    internal Arc Arc => new(StartAngle, EndAngle);
    private int IsTop => (int)(_corner & (ButtonCorner)0b01);
    private int IsRight => (int)_corner >> 1;
    private Size Size => new(Width, Height);
    protected override Geometry DefiningGeometry => Arc.GetGeometry(
        center: new(IsRight * Width, IsTop * Height),
        ellipseRadii: Size,
        startAngle: StartAngle,
        endAngle: EndAngle);

    public RotationButton(ButtonCorner corner) => _corner = corner;
}

internal record RotationButtons(
    RotationButton TopLeft,
    RotationButton TopRight,
    RotationButton BottomLeft,
    RotationButton BottomRight)
{
    internal void ForEach(Action<RotationButton> action)
    {
        action(TopLeft);
        action(TopRight);
        action(BottomLeft);
        action(BottomRight);
    }
}