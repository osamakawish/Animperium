using System;
using System.Windows;
using System.Windows.Controls;
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
    
    public Double2D EllipseRadii {
        get => (Width / 2, Height / 2);
        set => (Width, Height) = value * 2;
    }

    public Point Center {
        get => new(Canvas.GetLeft(this) + Width / 2, Canvas.GetTop(this) + Height / 2);
        set { Canvas.SetLeft(this, value.X - Width / 2); Canvas.SetTop(this, value.Y - Height / 2); }
    }

    public static Geometry GetGeometry(Point center, Size ellipseRadii, double startAngle, double endAngle)
    {
        Double2D EllipsePoint(double angle) => ellipseRadii * Double2D.ToCirclePoint(angle) + center;

        var arcSegment = new ArcSegment(
            point: EllipsePoint(endAngle),
            size: ellipseRadii, 0,
            isLargeArc: Math.Abs(startAngle - endAngle) >= Math.PI,
            sweepDirection: endAngle > startAngle
                ? SweepDirection.Counterclockwise : SweepDirection.Clockwise,
            isStroked: true);

        var pathFigure = new PathFigure
        {
            StartPoint = EllipsePoint(startAngle),
            Segments = new PathSegmentCollection { arcSegment }
        };

        return new PathGeometry(new[] { pathFigure });
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

    /// <summary>
    /// Creates a semicircle.
    /// </summary>
    public Arc()
        => (StartAngle, EndAngle) = (0, Math.PI);

    /// <summary>
    /// Creates an arc given the provided angle range. By default, this angle range is from 0 to pi, creating a semicircle.
    /// </summary>
    /// <param name="startAngle"></param>
    /// <param name="endAngle"></param>
    public Arc(double startAngle=0, double endAngle=Math.PI)
        => (StartAngle, EndAngle) = (startAngle, endAngle);
}