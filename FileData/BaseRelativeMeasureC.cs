using System.Numerics;

namespace MathAnim.FileData;

/// <summary>
/// A relative measure with center (C in <see cref="BaseRelativeMeasureC{T}"/>) of the canvas as its origin (0,0) point.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract record BaseRelativeMeasureC<T> where T : IUnaryNegationOperators<T, T>, IAdditionOperators<T,T,T>,
    IMultiplyOperators<T, T, T>, IDivisionOperators<T, double, T>, IDivisionOperators<T, T, T>
{
    /// <summary>
    /// The current total length in the given direction of the screen to be relative to.
    /// </summary>
    public abstract T ActualCanvasSize { get; set; }

    /// <summary>
    /// The scale factor that all lengths are scaled by. That is, the ratio of the actual canvas
    /// size to the relative canvas size multiplied by the factor.
    /// </summary>
    public T Scale => ActualCanvasSize / RelativeCanvasSize;

    /// <summary>
    /// The range of values to set to. Setting to the minimum value corresponds to applying Canvas.SetLeft(_, 0)
    /// or other similar x/y position setter. Max corresponds to the opposite end of the 0 on the canvas.
    /// </summary>
    public (T min, T max) PositionRange => (-RelativeCanvasSize/2, RelativeCanvasSize/2);

    /// <summary>The relative size of the canvas, as multiples of the factor.</summary>
    public abstract T RelativeCanvasSize { get; init; }

    /// <summary>
    /// Provides the actual size to give to an object, provided its set relativeSize.
    /// </summary>
    /// <param name="relativeObjectSize">The relative size of a given object in units.</param>
    /// <returns>The actual size to be applied to the object.</returns>
    public T ActualObjectSize(T relativeObjectSize) => Scale * relativeObjectSize;

    /// <summary>
    /// The actual object position, given its relative position, under this measure.
    /// </summary>
    /// <param name="relativeObjectPosition"></param>
    /// <returns></returns>
    public T this[T relativeObjectPosition] => (Scale * relativeObjectPosition + ActualCanvasSize) / 2;
}