using System.Windows.Media;
using Animperium.Animation;

namespace Animperium.Graphics;

internal interface IShape : IGraphicsObject
{
    AnimatableProperty<double> StrokeThickness { get; set; }
    AnimatableProperty<Brush> Stroke { get; set; }
    AnimatableProperty<Brush> Fill { get; set; }
}