using System.Numerics;
using System.Windows;

namespace MathAnim.FileData;

/// <summary>
/// A 2D double with element-wise addition/subtraction/multiplication/division.
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
/// <remarks><b>Note.</b> It is much cleaner to introduce construct these objects via a
/// (<see cref="double"/>, <see cref="double"/>) tuple.<br/>
/// <b>See:</b> <see cref="op_Implicit"/>.
/// </remarks>
public record Double2D(double X, double Y) :
    IUnaryNegationOperators<Double2D, Double2D>,
    IAdditionOperators<Double2D, Double2D, Double2D>, ISubtractionOperators<Double2D, Double2D, Double2D>,
    IMultiplyOperators<Double2D, Double2D, Double2D>, IMultiplyOperators<Double2D, double, Double2D>,
    IDivisionOperators<Double2D, double, Double2D>, IDivisionOperators<Double2D, Double2D, Double2D>
{
    public static Double2D operator -(Double2D value) => new(-value.X, -value.Y);

    public static Double2D operator +(Double2D left, Double2D right) => new(left.X + right.X, left.Y + right.Y);
    public static Double2D operator -(Double2D left, Double2D right) => new(left.X - right.X, left.Y - right.Y);
    
    public static Double2D operator *(Double2D left, double value) => new(left.X / value, left.Y / value);
    public static Double2D operator *(Double2D left, Double2D right) => new(left.X * right.X, left.Y * right.Y);

    public static Double2D operator /(Double2D left, double value) => new(left.X / value, left.Y / value);
    public static Double2D operator /(Double2D left, Double2D right) => new(left.X / right.X, left.Y / right.Y);

    public static implicit operator Double2D((double x, double y) pair) => new(pair.x, pair.y);
    public static implicit operator Double2D(Size size) => new(size.Width, size.Height);
}