using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Animperium.Controls;

namespace Animperium.Graphics.Selection;

internal class RotationButton : Shape
{
    [Flags]
    internal enum ButtonCorner
    {
        BottomLeft = 0b00,
        BottomRight = 0b10,
        TopLeft = 0b01,
        TopRight = 0b11
    }

    private readonly ButtonCorner _corner;

    internal Point Center => new(IsRight * Width, IsTop * Height);

    private int IsTop => (int)(_corner & (ButtonCorner)0b01);
    private int IsRight => (int)_corner >> 1;
    private Size Size => new(Width, Height);
    protected override Geometry DefiningGeometry => Arc.GetGeometry(
        new Point(IsRight * Width, IsTop * Height), Size,
        _corner switch {
            ButtonCorner.BottomLeft => -1.5 * Math.PI,
            ButtonCorner.BottomRight => -Math.PI,
            ButtonCorner.TopLeft => 0,
            ButtonCorner.TopRight => -0.5 * Math.PI,
            _ => throw new ArgumentOutOfRangeException()
        },
        _corner switch {
            ButtonCorner.BottomLeft => 2 * Math.PI,
            ButtonCorner.BottomRight => 0.5 * Math.PI,
            ButtonCorner.TopLeft => 1.5 * Math.PI,
            ButtonCorner.TopRight => Math.PI,
            _ => throw new ArgumentOutOfRangeException()
        });

    public RotationButton(ButtonCorner corner) => _corner = corner;
}