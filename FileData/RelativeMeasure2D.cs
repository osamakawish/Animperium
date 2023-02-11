using MathAnim.Essentials;

namespace MathAnim.FileData;

public record RelativeMeasure2D(BaseRelativeMeasureC XMeasure, BaseRelativeMeasureC YMeasure)
    : BaseRelativeMeasureC<Double2D>
{
    public static implicit operator RelativeMeasure2D((double width, double height) canvasSize)
        => new(canvasSize.width, canvasSize.height) { ActualCanvasSize = (double.NaN, double.NaN) };

    public override required Double2D ActualCanvasSize { get; set; } = (double.NaN, double.NaN);
    public override Double2D RelativeCanvasSize { get; init; }
        = (XMeasure.RelativeCanvasSize, YMeasure.RelativeCanvasSize);
}