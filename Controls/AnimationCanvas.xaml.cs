using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Animperium.Essentials;
using Animperium.FileData;
using Animperium.Graphics;

namespace Animperium.Controls;

/// <summary>
/// Interaction logic for AnimationCanvas.xaml
/// </summary>
public partial class AnimationCanvas
{
    internal static RelativeMeasure2D RelativeMeasure => BaseRelativeMeasureC.Standard;

    private readonly Dictionary<Shape, (Double2D position, Double2D size)> _shapes = new();
    internal GraphicsTool? GraphicsTool { get; set; } = null;

    public AnimationCanvas()
    {
        InitializeComponent();

        RelativeMeasure.ActualCanvasSize = (Canvas.ActualWidth, Canvas.ActualHeight);

        MouseDown += (_, e) => GraphicsTool?.OnDown(Canvas, e);
        MouseMove += (_, e) => GraphicsTool?.OnMove(Canvas, e);
        MouseUp   += (_, e) => GraphicsTool?.OnUp  (Canvas, e);

        // For Debugging only.
        AddShape<Ellipse>(strokeThickness: 1);
    }

    internal TShape AddShape<TShape>(
        Double2D? relativePosition = null,
        Double2D? relativeSize = null,
        double strokeThickness = 1,
        SolidColorBrush? strokeColor = null,
        SolidColorBrush? fillColor = null)
        where TShape : Shape, new()
    {
        // Initialize possible nulls.
        relativePosition ??= new Double2D(0, 0); relativeSize ??= new Double2D(1, 1);
        strokeColor ??= Brushes.Black; fillColor ??= Brushes.Transparent;

        // Implement parameters into shape.
        TShape shape = new() { StrokeThickness = strokeThickness, Stroke = strokeColor, Fill = fillColor };
        _shapes[shape] = (relativePosition, relativeSize);
        UpdateShapeRendering(shape);

        // Add object to canvas.
        Canvas.Children.Add(shape);

        return shape;
    }

    protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
    {
        base.OnRenderSizeChanged(sizeInfo);

        var (newWidth, newHeight) = (sizeInfo.NewSize.Width, sizeInfo.NewSize.Height);

        var (relativeWidth, relativeHeight) = RelativeMeasure.RelativeCanvasSize;
        var desiredWidthToHeight = relativeWidth / relativeHeight;

        // Maintain aspect ratio while fitting canvas within the grid.
        (Canvas.Width, Canvas.Height) = newWidth > desiredWidthToHeight * newHeight
            ? (newHeight * desiredWidthToHeight, newHeight)
            : (newWidth, newWidth / desiredWidthToHeight);

        RelativeMeasure.ActualCanvasSize = (Canvas.Width, Canvas.Height);
        foreach (var shape in _shapes.Keys) UpdateShapeRendering(shape);
    }

    /// <summary>
    /// Updates the rendered dimensions (position and size) of the shape.
    /// </summary>
    /// <param name="shape"></param>
    /// <remarks><b>Requires <see cref="_shapes"/> to be updated with the <see cref="shape"/>'s dimensions.</b></remarks>
    private void UpdateShapeRendering(Shape shape)
    {
        var (relativePosition, relativeSize) = _shapes[shape];
        var renderedPosition = RelativeMeasure.ToRenderedObjectPosition(relativePosition)
                               - (shape.StrokeThickness, shape.StrokeThickness);
        var renderedSize = RelativeMeasure.ToRenderedObjectSize(relativeSize);

        Canvas.SetLeft(shape, renderedPosition.X); Canvas.SetTop(shape, renderedPosition.Y);
        (shape.Width, shape.Height) = renderedSize;
    }
}