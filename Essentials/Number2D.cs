using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;
using System.Windows;

namespace Animperium.Essentials;


/// <summary>
/// A 2D TNumber with element-wise addition/subtraction/multiplication/division.
/// </summary>
/// <param name="X"></param>
/// <param name="Y"></param>
/// <remarks><b>Note.</b> It is much cleaner to introduce construct these objects via a
/// (<see cref="TNumber"/>, <see cref="TNumber"/>) tuple.<br/>
/// </remarks>
public record Number2D<TNumber>(TNumber X, TNumber Y) :
    IUnaryNegationOperators<Number2D<TNumber>, Number2D<TNumber>>,
    IAdditionOperators<Number2D<TNumber>, Number2D<TNumber>, Number2D<TNumber>>, IAdditionOperators<Number2D<TNumber>, TNumber, Number2D<TNumber>>,
    ISubtractionOperators<Number2D<TNumber>, Number2D<TNumber>, Number2D<TNumber>>, ISubtractionOperators<Number2D<TNumber>, TNumber, Number2D<TNumber>>,
    IMultiplyOperators<Number2D<TNumber>, Number2D<TNumber>, Number2D<TNumber>>, IMultiplyOperators<Number2D<TNumber>, TNumber, Number2D<TNumber>>,
    IDivisionOperators<Number2D<TNumber>, TNumber, Number2D<TNumber>>, IDivisionOperators<Number2D<TNumber>, Number2D<TNumber>, Number2D<TNumber>>,
    IComparisonOperators<Number2D<TNumber>, Number2D<TNumber>, (bool x, bool y)>
        where TNumber : IUnaryNegationOperators<TNumber, TNumber>,
                IAdditionOperators<TNumber, TNumber, TNumber>, ISubtractionOperators<TNumber, TNumber, TNumber>,
                IMultiplyOperators<TNumber, TNumber, TNumber>, IDivisionOperators<TNumber, TNumber, TNumber>,
                IComparisonOperators<TNumber, TNumber, bool>
{
    public static Number2D<TNumber> operator -(Number2D<TNumber> value) => new(-value.X, -value.Y);

    public static Number2D<TNumber> operator +(Number2D<TNumber> left, Number2D<TNumber> right) => new(left.X + right.X, left.Y + right.Y);
    public static Number2D<TNumber> operator +(Number2D<TNumber> left, TNumber right) => (left.X + right, left.Y + right);
    public static Number2D<TNumber> operator -(Number2D<TNumber> left, Number2D<TNumber> right) => new(left.X - right.X, left.Y - right.Y);
    public static Number2D<TNumber> operator -(Number2D<TNumber> left, TNumber right) => new(left.X - right, left.Y - right);

    public static Number2D<TNumber> operator *(Number2D<TNumber> left, TNumber value) => new(left.X / value, left.Y / value);
    public static Number2D<TNumber> operator *(Number2D<TNumber> left, Number2D<TNumber> right) => new(left.X * right.X, left.Y * right.Y);

    public static Number2D<TNumber> operator /(Number2D<TNumber> left, TNumber value) => new(left.X / value, left.Y / value);
    public static Number2D<TNumber> operator /(Number2D<TNumber> left, Number2D<TNumber> right) => new(left.X / right.X, left.Y / right.Y);

    public static implicit operator Number2D<TNumber>((TNumber x, TNumber y) pair) => new(pair.x, pair.y);

    public static (bool x, bool y) operator >(Number2D<TNumber> left, Number2D<TNumber> right) => (left.X > right.X, left.Y > right.Y);
    public static (bool x, bool y) operator >=(Number2D<TNumber> left, Number2D<TNumber> right) => (left.X >= right.X, left.Y >= right.Y);
    public static (bool x, bool y) operator <(Number2D<TNumber> left, Number2D<TNumber> right) => (left.X < right.X, left.Y < right.Y);
    public static (bool x, bool y) operator <=(Number2D<TNumber> left, Number2D<TNumber> right) => (left.X <= right.X, left.Y <= right.Y);

    static (bool x, bool y) IEqualityOperators<Number2D<TNumber>, Number2D<TNumber>, (bool x, bool y)>.operator ==(Number2D<TNumber>? left,
        Number2D<TNumber>? right)
    {
        if (left is null && right is null) return (true, true);
        if (left is null || right is null) return (false, false);
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        return (left.X == right.X,
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            left.Y == right.Y);
    }

    static (bool x, bool y) IEqualityOperators<Number2D<TNumber>, Number2D<TNumber>, (bool x, bool y)>.operator !=(Number2D<TNumber>? left, Number2D<TNumber>? right)
    {
        if (left is null && right is null) return (false, false);
        if (left is null || right is null) return (true, true);
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        return (left.X != right.X,
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            left.Y != right.Y);
    }

    public static (Number2D<TNumber> Min, Number2D<TNumber> Max) Bounds(Number2D<TNumber> left, Number2D<TNumber> right)
    {
        var (lessThanX, lessThanY) = left < right;

        var (minX, maxX) = lessThanX ? (left.X, right.X) : (right.X, left.X);
        var (minY, maxY) = lessThanY ? (left.Y, right.Y) : (right.Y, left.Y);

        return ((minX, minY), (maxX, maxY));
    }

    public static (Number2D<TNumber> Min, Number2D<TNumber> Max) BoundsFromMultiple(Number2D<TNumber> first, params Number2D<TNumber>[] pairs)
    {
        var (minX, minY, maxX, maxY) = (first.X, first.Y, first.X, first.Y);

        foreach (var (x, y) in pairs)
        {
            if (x < minX) minX = x; else if (x > maxX) maxX = x;
            if (y < minY) minY = y; else if (y > maxY) maxY = y;
        }

        return ((minX, minY), (maxX, maxY));
    }
}