using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
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
    // For quickly accessing shapes' canvases.
    internal static readonly Dictionary<ShapeCollection, AnimationCanvas> ShapeToAssociatedCanvas = new();

    internal static RelativeMeasure2D RelativeMeasure => BaseRelativeMeasureC.Standard;

    private readonly Dictionary<Shape, (Double2D position, Double2D size)> _shapes = new();

    internal VisualAnimationTool VisualAnimationTool { get; set; } = AnimationTools.ItemSelectTool;
    private ShapeCollection ShapeCollection { get; }
    private Rect? _mouseRect;

    public AnimationCanvas()
    {
        InitializeComponent();

        RelativeMeasure.ActualCanvasSize = (Canvas.ActualWidth, Canvas.ActualHeight);

        ShapeCollection = new ShapeCollection();

        // Handle the mouse events, given the visual animation tool.
        void MouseEventBehaviour<TMouseEventArgs>(
            TMouseEventArgs eventArgs,
            VisualMouseReaction<TMouseEventArgs> mouseEventReaction,
            bool isMouseUp = false)
                where TMouseEventArgs : MouseEventArgs
        {
            // Currently handling left mouse button events only. Will deal
            // with other buttons later.
            if (eventArgs.LeftButton.HasFlag(MouseButtonState.Released)) return;

            var point2 = eventArgs.GetPosition(this);
            var point1 = _mouseRect?.Location ?? point2;

            var rect = new Rect(point1, point2);
            mouseEventReaction(rect, ShapeCollection, eventArgs);
            _mouseRect = isMouseUp ? null : rect;
        }
        MouseDown += (_, e) => MouseEventBehaviour(e, VisualAnimationTool.OnDown);
        MouseMove += (_, e) => MouseEventBehaviour(e, VisualAnimationTool.OnMove);
        MouseUp   += (_, e) => MouseEventBehaviour(e, VisualAnimationTool.OnUp, isMouseUp: true);

        ShapeToAssociatedCanvas.Add(ShapeCollection, this);

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

        // Implement parameters and fields into shape.
        TShape shape = new() { StrokeThickness = strokeThickness, Stroke = strokeColor, Fill = fillColor };
        _shapes[shape] = (relativePosition, relativeSize); ShapeCollection.Add(shape);
        UpdateShapeRendering(shape);

        // Add object to canvas.
        Canvas.Children.Add(shape);

        return shape;
    }

    /// <summary>
    /// Moves and rescales the shape to the given rect according to relative coordinates.
    /// </summary>
    /// <param name="shape"></param>
    /// <param name="rect"></param>
    internal void SetShapeRegion(Shape shape, Rect rect)
    {
        _shapes[shape] = ((rect.X, rect.Y), (rect.Width, rect.Height));
        UpdateShapeRendering(shape);
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