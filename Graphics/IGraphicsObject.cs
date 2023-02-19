using System.Drawing;
using System.Windows.Media;
using Animperium.Animation;

namespace Animperium.Graphics;

// Origin in center by default.
internal interface IGraphicsObject
{
    AnimatableProperty<SizeF> Size { get; set; }
    AnimatableProperty<Point> Position { get; set; }
    AnimatableProperty<Transform> TransformTransform { get; set; }
    AnimatableProperty<bool> Visibility { get; set; }
}