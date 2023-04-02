using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Animperium.Graphics.Selection;

public class ShearButton : Shape
{
    [Flags]
    internal enum ButtonEdge
    { Left = 0b00, Right = 0b10, Bottom = 0b01, Top = 0b11 }
    
    private readonly ButtonEdge _edge;

    internal bool IsHorizontal { get; }

    /// <summary>
    /// For creating horizontal/vertical lines.
    /// </summary>
    /// <param name="a">The fixed value. The x coordinate for vertical lines, and y coordinate in horizontal lines.</param>
    /// <param name="b1">The initial value of varying coordinate.</param>
    /// <param name="b2">The final value of the varying coordinate.</param>
    /// <returns></returns>
    private delegate LineGeometry FlatLine(double a, double b1, double b2);
    protected override Geometry DefiningGeometry {
        get {

            double MinLength(double edgeLength) => Math.Min(edgeLength / 6, 36);
            var length = Math.Min(MinLength(Width), MinLength(Height));

            var centreWidth = Width / 2; var centreHeight = Height / 2;
            var halfLineLength = length / 2; var thirdLineLength = length / 3;

            (LineGeometry firstLine, LineGeometry midLine, LineGeometry lastLine) CreateLines(
                FlatLine createLine, double centerOfEdge, double edge, double fix)
                => (createLine(a: edge + fix, b1: centerOfEdge - halfLineLength,  b2: centerOfEdge - thirdLineLength),
                    createLine(a: edge - fix, b1: centerOfEdge - thirdLineLength, b2: centerOfEdge + thirdLineLength),
                    createLine(a: edge + fix, b1: centerOfEdge + thirdLineLength, b2: centerOfEdge + halfLineLength));

            var lines = _edge switch {
                ButtonEdge.Left => CreateLines(VerticalLine, centreHeight, 0, 3),
                ButtonEdge.Right => CreateLines(VerticalLine, centreHeight, 0, -3),
                ButtonEdge.Bottom => CreateLines(HorizontalLine, centreWidth, Height, -3),
                ButtonEdge.Top => CreateLines(VerticalLine, centreHeight, 0, 3),
                _ => throw new ArgumentOutOfRangeException()
            };

            return new GeometryGroup { Children = new GeometryCollection { lines.firstLine, lines.midLine, lines.lastLine } };
        }
    }

    private static LineGeometry VerticalLine(double x, double y1, double y2) => new(new Point(x, y1), new Point(x, y2));

    private static LineGeometry HorizontalLine(double y, double x1, double x2) => new(new Point(x1, y), new Point(x2, y));

    internal ShearButton(ButtonEdge buttonEdge)
    { _edge = buttonEdge; IsHorizontal = _edge < ButtonEdge.Right; }
}