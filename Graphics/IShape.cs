using MathAnim.Animation;
using System.Windows.Media;

namespace MathAnim.Graphics;

internal interface IShape : IGraphicsObject
{
    AnimatableProperty<double> StrokeThickness { get; set; }
    AnimatableProperty<Brush> Stroke { get; set; }
    AnimatableProperty<Brush> Fill { get; set; }
}