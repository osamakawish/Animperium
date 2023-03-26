using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Animperium.Graphics.Selection;

// Make one for horizontal and another for vertical shear buttons, due to similarity in mouse behaviour.
// Geometry should only define one button.
public class ShearButton : Shape
{
    protected override Geometry DefiningGeometry {
        get {

            double MinLength(double edgeLength) => Math.Min(edgeLength / 6, 36);
            var length = Math.Min(MinLength(Width), MinLength(Height));

            var halfWidth = Width / 2; var halfHeight = Height / 2;
            var halfLineLength = length / 2; var thirdLineLength = length / 3;

            (LineGeometry firstLine, LineGeometry midLine, LineGeometry lastLine) CreateLines(
                Func<double, double, double, LineGeometry> createLine, double halfEdge, double edge, double fix)
                => (createLine(edge + fix, halfEdge - halfLineLength, halfEdge - thirdLineLength),
                    createLine(edge - fix, halfEdge - halfLineLength, halfEdge - thirdLineLength),
                    createLine(edge + fix, halfEdge - halfLineLength, halfEdge - thirdLineLength));

            LineGeometry HorizontalLine(double y, double x1, double x2) => new(new Point(x1, y), new Point(x2, y));
            LineGeometry VerticalLine(double x, double y1, double y2) => new(new Point(x, y1), new Point(x, y2));
    
            var topLines = CreateLines(HorizontalLine, halfWidth, 0, 3);
            var botLines = CreateLines(HorizontalLine, halfWidth, Height, -3);
            var leftLines = CreateLines(VerticalLine, halfHeight, 0, 3);
            var rightLines = CreateLines(VerticalLine, halfHeight, Width, -3);

            GeometryGroup GeometryGroup((LineGeometry firstLine, LineGeometry midLine, LineGeometry lastLine) lines)
                => new() { Children = new GeometryCollection { lines.firstLine, lines.midLine, lines.lastLine } };

            return new GeometryGroup
            {
                Children = new GeometryCollection {
                    GeometryGroup(topLines), GeometryGroup(botLines), GeometryGroup(leftLines), GeometryGroup(rightLines) }
            };
        }
    }
}