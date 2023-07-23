using System.CodeDom;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Animperium.Graphics.Selection;

// Need separate geometry for distinct buttons.
public class ScaleButton : Shape
{
    protected override Geometry DefiningGeometry {
        get {
            var rect = new Rect(point1: new(0, 0), point2: new(Width, Height));
            rect.Inflate(-6, -6);
            var length = 8;

            PolyLineSegment CornerScaleButton(Point point, bool isLeft, bool isTop) => new()
            {
                Points = new PointCollection {
                    point + new Vector(isLeft ? length : -length, 0),
                    point,
                    point + new Vector(0, isTop ? length : -length)
                }
            };

            var nw = CornerScaleButton(rect.TopLeft,     true, true);
            var ne = CornerScaleButton(rect.TopRight,    false, true);
            var sw = CornerScaleButton(rect.BottomLeft,  true, false);
            var se = CornerScaleButton(rect.BottomRight, false, false);

            return null;
        }
    }
}