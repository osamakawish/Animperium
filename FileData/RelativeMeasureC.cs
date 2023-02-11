using Animperium.Settings;

namespace Animperium.FileData;

/// <summary>
/// Computes relative lengths on a canvas with the centre (C for centre in <see cref="BaseRelativeMeasureC"/>)
/// of objects as their origin. <br/>
/// This class uses has the range of -<see cref="RelativeCanvasSize"/>/2 to +<see cref="RelativeCanvasSize"/>/2
/// for its positions.<br/>
/// <b>This class is fundamentally 1-dimensional.</b>
/// </summary>
/// <remarks>The purpose of this is to use units and size that's independent of the size
/// of the canvas.</remarks>
/// <param name="RelativeCanvasSize">The relative size of the canvas, as multiples of the factor.</param>
public record BaseRelativeMeasureC(double RelativeCanvasSize) : BaseRelativeMeasureC<double>
{
    public static implicit operator BaseRelativeMeasureC(double canvasLength)
        => new(canvasLength) { ActualCanvasSize = double.NaN };

    public static readonly RelativeMeasure2D Standard = StandardSettings.ForApp.RelativeMeasure2D;

    /// <summary>
    /// The current total length in the given direction of the screen to be relative to.
    /// </summary>
    public override required double ActualCanvasSize { get; set; }
}