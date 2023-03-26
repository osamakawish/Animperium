using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Animperium.Graphics.Selection;

public class OriginButton : Shape
{
    protected override Geometry DefiningGeometry {
        get {
            LineGeometry HorizontalLine(double y, double x1, double x2) => new(new Point(x1, y), new Point(x2, y));
            LineGeometry VerticalLine(double x, double y1, double y2) => new(new Point(x, y1), new Point(x, y2));

            var center = new Point(Width / 2, Height / 2);
            var radius = 6;
            var plusLineRadius = 4;

            var circle = new EllipseGeometry(center, radius, radius);
            var plus = new GeometryGroup
            {
                Children = new GeometryCollection {
                    HorizontalLine(center.Y, center.X - plusLineRadius, center.X + plusLineRadius),
                    VerticalLine(center.X, center.Y - plusLineRadius, center.Y + plusLineRadius)
                }
            };

            return new GeometryGroup { Children = new GeometryCollection { circle, plus } };
        }
    }
}