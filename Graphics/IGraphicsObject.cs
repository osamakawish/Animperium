using MathAnim.Animation;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MathAnim.Graphics;

// Origin in center by default.
internal interface IGraphicsObject
{
    AnimatableProperty<SizeF> Size { get; set; }
    AnimatableProperty<PointF> Position { get; set; }
    AnimatableProperty<Transform> TransformTransform { get; set; }
    AnimatableProperty<bool> Visibility { get; set; }
}