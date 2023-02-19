using System.Numerics;

namespace Animperium.FileData;

/// <summary>
/// A relative measure with center (C in <see cref="BaseRelativeMeasureC{T}"/>) of the canvas as its origin (0,0) point.
/// </summary>
/// <typeparam name="T"></typeparam>
public abstract record BaseRelativeMeasureC<T>
        where T : IUnaryNegationOperators<T, T>,
        IAdditionOperators<T,T,T>, IAdditionOperators<T, double, T>,
        ISubtractionOperators<T, T, T>, ISubtractionOperators<T, double, T>,
        IMultiplyOperators<T, T, T>, IMultiplyOperators<T, double, T>,
        IDivisionOperators<T, double, T>, IDivisionOperators<T, T, T>
{
    /// <summary>
    /// The current total length in the given direction of the screen to be relative to.
    /// </summary>
    public abstract required T ActualCanvasSize { get; set; }

    /// <summary>
    /// The scale factor that all lengths are scaled by. That is, the ratio of the actual canvas
    /// size to the relative canvas size multiplied by the factor.
    /// </summary>
    public T Scale => ActualCanvasSize / RelativeCanvasSize;

    /// <summary>
    /// The range of values to set to. Setting to the minimum value corresponds to applying Canvas.SetLeft(_, 0)
    /// or other similar x/y position setter. Max corresponds to the opposite end of the 0 on the canvas.
    /// </summary>
    public (T min, T max) RelativePositionRange => (-RelativeCanvasSize / 2, RelativeCanvasSize / 2);

    /// <summary>The relative size of the canvas, as multiples of the factor.</summary>
    public abstract T RelativeCanvasSize { get; init; }

    /// <summary>
    /// Provides the actual size to give to an object, provided its set relativeSize.
    /// </summary>
    /// <param name="relativeObjectSize">The relative size of a given object in units.</param>
    /// <returns>The actual size to be applied to the object.</returns>
    public T ToActualObjectSize(T relativeObjectSize) => Scale * relativeObjectSize;

    /// <summary>
    /// The actual object position, given its relative position, under this measure.
    /// </summary>
    /// <param name="relativeObjectPosition"></param>
    /// <returns></returns>
    public T ToActualObjectPosition(T relativeObjectPosition)
        => ActualCanvasSize * (relativeObjectPosition / RelativeCanvasSize + 0.5);

    /// <summary>
    /// The relative object position, given the actual position, under this measure.
    /// </summary>
    /// <param name="actualObjectPosition"></param>
    /// <returns></returns>
    public T ToRelativeObjectPosition(T actualObjectPosition)
    {
        return RelativeCanvasSize * (actualObjectPosition / ActualCanvasSize - 0.5);
    }

    /// <summary>
    /// The relative object size, given the actual size, under this measure.
    /// </summary>
    /// <param name="actualObjectSize"></param>
    /// <returns></returns>
    public T ToRelativeObjectSize(T actualObjectSize) => actualObjectSize / Scale;
}