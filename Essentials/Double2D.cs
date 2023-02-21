using System.Numerics;
using System.Windows;

namespace Animperium.Essentials;

/// <summary>
/// A 2D double with element-wise addition/subtraction/multiplication/division.
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
/// <remarks><b>Note.</b> It is much cleaner to introduce construct these objects via a
/// (<see cref="double"/>, <see cref="double"/>) tuple.<br/>
/// </remarks>
public record Double2D(double X, double Y) : IAdditiveIdentity<Double2D, Double2D>,
    IMultiplicativeIdentity<Double2D, Double2D>, IUnaryNegationOperators<Double2D, Double2D>,
    IAdditionOperators<Double2D, Double2D, Double2D>, IAdditionOperators<Double2D, double, Double2D>, 
    ISubtractionOperators<Double2D, Double2D, Double2D>, ISubtractionOperators<Double2D, double, Double2D>,
    IMultiplyOperators<Double2D, Double2D, Double2D>, IMultiplyOperators<Double2D, double, Double2D>,
    IDivisionOperators<Double2D, double, Double2D>, IDivisionOperators<Double2D, Double2D, Double2D>,
    IComparisonOperators<Double2D, Double2D, (bool x, bool y)>
{
    public static Double2D Zero { get; } = new(0, 0);
    public static Double2D AdditiveIdentity => Zero;

    public static Double2D Unit { get; } = new(1, 1);

    public static Double2D MultiplicativeIdentity => Unit;

    public static Double2D operator -(Double2D value) => new(-value.X, -value.Y);

    public static Double2D operator +(Double2D left, Double2D right) => new(left.X + right.X, left.Y + right.Y);
    public static Double2D operator +(Double2D left, double right) => (left.X + right, left.Y + right);
    public static Double2D operator -(Double2D left, Double2D right) => new(left.X - right.X, left.Y - right.Y);
    public static Double2D operator -(Double2D left, double right) => new(left.X - right, left.Y - right);

    public static Double2D operator *(Double2D left, double value) => new(left.X / value, left.Y / value);
    public static Double2D operator *(Double2D left, Double2D right) => new(left.X * right.X, left.Y * right.Y);

    public static Double2D operator /(Double2D left, double value) => new(left.X / value, left.Y / value);
    public static Double2D operator /(Double2D left, Double2D right) => new(left.X / right.X, left.Y / right.Y);

    public static implicit operator Double2D((double x, double y) pair) => new(pair.x, pair.y);
    public static implicit operator Double2D(Size size) => new(size.Width, size.Height);
    public static implicit operator Size(Double2D double2D)
        => double2D.X < 0 || double2D.Y < 0 ? new Size(0, 0) : new Size(double2D.X, double2D.Y);
    public static implicit operator Double2D(Point point) => new(point.X, point.Y);
    public static implicit operator Point(Double2D double2D) => new(double2D.X, double2D.Y);

    public static (bool x, bool y) operator >(Double2D left, Double2D right) => (left.X > right.X, left.Y > right.Y);
    public static (bool x, bool y) operator >=(Double2D left, Double2D right) => (left.X >= right.X, left.Y >= right.Y);
    public static (bool x, bool y) operator <(Double2D left, Double2D right) => (left.X < right.X, left.Y < right.Y);
    public static (bool x, bool y) operator <=(Double2D left, Double2D right) => (left.X <= right.X, left.Y <= right.Y);

    static (bool x, bool y) IEqualityOperators<Double2D, Double2D, (bool x, bool y)>.operator ==(Double2D? left,
        Double2D? right)
    {
        if (left is null && right is null) return (true, true);
        if (left is null || right is null) return (false, false);
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        return (left.X == right.X,
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            left.Y == right.Y);
    }

    static (bool x, bool y) IEqualityOperators<Double2D, Double2D, (bool x, bool y)>.operator !=(Double2D? left, Double2D? right)
    {
        if (left is null && right is null) return (false, false);
        if (left is null || right is null) return (true, true);
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        return (left.X != right.X,
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            left.Y != right.Y);
    }

    public static (Double2D Min, Double2D Max) Bounds(Double2D left, Double2D right)
    {
        var (lessThanX, lessThanY) = left < right;

        var (minX, maxX) = lessThanX ? (left.X, right.X) : (right.X, left.X);
        var (minY, maxY) = lessThanY ? (left.Y, right.Y) : (right.Y, left.Y);

        return ((minX, minY), (maxX, maxY));
    }

    public static (Double2D Min, Double2D Max) BoundsFromMultiple(Double2D first, params Double2D[] pairs)
    {
        var (minX, minY, maxX, maxY) = (first.X, first.Y, first.X, first.Y);

        foreach (var (x, y) in pairs) {
            if (x < minX) minX = x; else if (x > maxX) maxX = x;
            if (y < minY) minY = y; else if (y > maxY) maxY = y;
        }

        return ((minX, minY), (maxX, maxY));
    }
}