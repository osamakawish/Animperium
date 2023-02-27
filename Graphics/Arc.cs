using System;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;
using Animperium.Essentials;

namespace Animperium.Graphics;

public class Arc : Shape
{
    public static readonly DependencyProperty StartAngleProperty
        = DependencyProperty.Register($"{nameof(StartAngle)}", typeof(double), typeof(Arc));

    public static readonly DependencyProperty EndAngleProperty
        = DependencyProperty.Register($"{nameof(EndAngle)}", typeof(double), typeof(Arc));

    public double StartAngle {
        get => (double)GetValue(StartAngleProperty);
        set => SetValue(StartAngleProperty, value);
    }

    public double EndAngle {
        get => (double)GetValue(EndAngleProperty);
        set => SetValue(EndAngleProperty, value);
    }

    protected override Geometry DefiningGeometry {
        get {
            var ellipseRadii = new Double2D(Width / 2, Height / 2);
            Double2D EllipsePoint(double angle) => ellipseRadii * (1 + Double2D.ToCirclePoint(angle));

            var arcSegment = new ArcSegment(
                point: EllipsePoint(EndAngle),
                size: ellipseRadii, 0,
                isLargeArc: Math.Abs(StartAngle - EndAngle) >= Math.PI,
                sweepDirection: EndAngle > StartAngle
                    ? SweepDirection.Counterclockwise : SweepDirection.Clockwise,
                isStroked: true);
            
            var pathFigure = new PathFigure
            {
                StartPoint = EllipsePoint(StartAngle),
                Segments = new PathSegmentCollection { arcSegment }
            };

            return new PathGeometry(new[] { pathFigure });
        }
    }

    public Arc()
        => (StartAngle, EndAngle) = (0, Math.PI);

    public Arc(double startAngle=0, double endAngle=Math.PI)
        => (StartAngle, EndAngle) = (startAngle, endAngle);
}