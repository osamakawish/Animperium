namespace MathAnim.FileData;

/// <summary>
/// Computes relative lengths on a canvas with the centre (C) of objects as their origin.
/// This class uses a range of -<see cref="RelativeCanvasLength"/>/2 to +<see cref="RelativeCanvasLength"/>/2.
/// </summary>
/// <remarks>The purpose of this is to use units and size that's independent of the size
/// of the canvas.</remarks>
/// <param name="Factor">The factor to multiple relative lengths by.</param>
/// <param name="RelativeCanvasLength">The relative size of the canvas, as multiples of the factor.</param>
public record RelativeLengthC(double Factor, double RelativeCanvasLength)
{
    //public static readonly 

    /// <summary>
    /// The current total length in the given direction of the screen to be relative to.
    /// </summary>
    public required double ActualCanvasLength { get; set; }

    /// <summary>
    /// The scale factor that all lengths are scaled by. That is, the ratio of the actual canvas
    /// size to the relative canvas size multiplied by the factor.
    /// </summary>
    public double Scale => ActualCanvasLength / (Factor * RelativeCanvasLength);

    public double CanvasLengthRatio => ActualCanvasLength / RelativeCanvasLength;

    /// <summary>
    /// Provides the actual size to give to an object, provided its set relativeSize.
    /// </summary>
    /// <param name="relativeObjectSize">The relative size of a given object in units.</param>
    /// <returns>The actual size to be applied to the object.</returns>
    public double ActualObjectSize(double relativeObjectSize) => Scale * relativeObjectSize;

    /// <summary>
    /// The range of values to set to. Setting to the minimum value corresponds to applying Canvas.SetLeft(_, 0)
    /// or other similar x/y position setter. Max corresponds to the opposite end of the 0 on the canvas.
    /// </summary>
    public (double min, double max) PositionRange => (-RelativeCanvasLength / 2, RelativeCanvasLength / 2);

    /// <summary>
    /// The actual object position, given its relative position.
    /// </summary>
    /// <param name="relativeObjectPosition"></param>
    /// <returns></returns>
    public double this[double relativeObjectPosition]
        => (CanvasLengthRatio * relativeObjectPosition + ActualCanvasLength) / 2;
}