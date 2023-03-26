using System;
using System.Windows;
using Animperium.Controls;

namespace Animperium.Graphics.Selection;

// TODO: Better to make this inherit from Shape., and then rename it to RotationButton.
internal record RotationButtonData(
    AnimationCanvas Canvas,
    Point Center,
    double Radius,
    double BaseAngle,
    double AngleSpan = RotationButtonData.ThreePiOverFour)
{
    internal const double ThreePiOverFour = 3 * Math.PI / 4;

    internal double AngleSpan { get; set; } = AngleSpan;
    internal Point Center { get => Arc.Center; set => Arc.Center = value; }

    internal double AdditionalAngle
    {
        get => Arc.StartAngle - BaseAngle;
        set
        {
            Arc.StartAngle = BaseAngle + value;
            Arc.EndAngle = Arc.StartAngle + AngleSpan;
        }
    }

    // TODO: Arc needs to handle rotation on click.
    internal Arc Arc { get; } = Canvas.AddArc(Center, (Radius, Radius), (BaseAngle, BaseAngle + Math.PI), true);
}