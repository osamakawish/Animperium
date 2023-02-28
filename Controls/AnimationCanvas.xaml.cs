using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
    /// <summary>
    /// The current animation canvas. Should be modified as the current animation canvas is changed
    /// (ie. when a file changes).
    /// </summary>
    internal static AnimationCanvas? Current { get; set; }

    // For quickly accessing shapes' canvases.
    internal static readonly Dictionary<ShapeCollection, AnimationCanvas> ShapeToAssociatedCanvas = new();

    internal static RelativeMeasure2D RelativeMeasure => BaseRelativeMeasureC.Standard;

    private readonly Dictionary<Shape, (Double2D position, Double2D size)> _shapes = new();

    internal VisualAnimationTool VisualAnimationTool { get; set; } = AnimationTools.ItemSelectTool;
    private ShapeCollection ShapeCollection { get; }
    private Point? _start;
    private HitTestResult? _hitTestResult;

    public AnimationCanvas()
    {
        InitializeComponent();
        Current = this;

        RelativeMeasure.ActualCanvasSize = (Canvas.ActualWidth, Canvas.ActualHeight);

        ShapeCollection = new ShapeCollection(this);

        // Handle the mouse events, given the visual animation tool.
        void MouseEventBehaviour<TMouseEventArgs>(
            TMouseEventArgs eventArgs,
            VisualMouseReaction<TMouseEventArgs> mouseEventReaction,
            bool isMouseUp = false)
                where TMouseEventArgs : MouseEventArgs
        {
            // Currently handling left mouse button events only. Will deal with other buttons later.
            if (!eventArgs.LeftButton.HasFlag(MouseButtonState.Pressed) && !isMouseUp) return;

            // Get the points relative to canvas.
            var point2 = eventArgs.GetPosition(Canvas);
            var point1 = _start ??= point2;
            // Get relative positions of the points.
            var point = RelativeMeasure.ToRelativeObjectPosition(point1);
            point2 = RelativeMeasure.ToRelativeObjectPosition(point2);
            
            mouseEventReaction(new VisualMouseEventArgs<TMouseEventArgs> {
                Start = point,
                End = point2,
                Shapes = ShapeCollection,
                MouseEventArgs = eventArgs,
                HitTestResult = _hitTestResult ??= VisualTreeHelper.HitTest(Canvas, eventArgs.GetPosition(Canvas))
            });
            
            if (!isMouseUp) return;
            _start = null;
            _hitTestResult = null;
        }
        MouseDown += (_, e) => MouseEventBehaviour(e, VisualAnimationTool.OnDown);
        MouseMove += (_, e) => MouseEventBehaviour(e, VisualAnimationTool.OnMove);
        MouseUp   += (_, e) => MouseEventBehaviour(e, VisualAnimationTool.OnUp, isMouseUp: true);

        ShapeToAssociatedCanvas.Add(ShapeCollection, this);

        Canvas.SetLeft(Path2, 0);
        Canvas.SetTop(Path2, 0);

        Loaded += (_, _) =>
        {
            // For Debugging only
            //var arc = this.AddArc(new Rect(new Point(3, 3), new Size(8, 8)), (Math.PI / 6, - 2 * Math.PI / 3));
            var arc = this.AddArc(new Point(4, 4), (4, 4), (0, Math.PI));
            arc.Stroke = Brushes.BlueViolet;

            var rect = AddShape<Rectangle>((0, 0), (8, 8), strokeColor: Brushes.DarkRed);

            var arc2 = AddShape<Arc>((-4, -4), (4, 4), strokeColor: Brushes.Crimson);
            var rect2 = AddShape<Rectangle>((-4, -4), (4, 4));
        };
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TShape"></typeparam>
    /// <param name="relativePosition"></param>
    /// <param name="relativeSize"></param>
    /// <param name="strokeThickness"></param>
    /// <param name="strokeColor"></param>
    /// <param name="fillColor"></param>
    /// <param name="isDecorative">For elements not included in the shape collection.<br/>
    /// A reference must be stored somewhere as it cannot be found in the ShapeCollection.<br/>
    /// Decorative shapes are hidden by default.
    /// </param>
    /// <returns></returns>
    internal TShape AddShape<TShape>(
        Double2D? relativePosition = null, // Use a rect here for cleaner code -> Hand rect.Empty and negative/zero Rect cases.
        Double2D? relativeSize = null,
        double strokeThickness = 1, // Use PenStrokeData instead here, to clean up code.
        SolidColorBrush? strokeColor = null,
        SolidColorBrush? fillColor = null,
        bool isDecorative = false)
        where TShape : Shape, new()
    {
        // Initialize possible nulls.
        relativePosition ??= new Double2D(0, 0); relativeSize ??= new Double2D(1, 1);
        strokeColor ??= Brushes.Black; fillColor ??= Brushes.Transparent;

        // Implement parameters and fields into shape.
        TShape shape = new() { StrokeThickness = strokeThickness, Stroke = strokeColor, Fill = fillColor };

        if (!isDecorative) {
            _shapes[shape] = (relativePosition, relativeSize);
            ShapeCollection.Add(shape);
            UpdateShapeRendering(shape);
        }
        else {
            shape.Visibility = Visibility.Collapsed;
            Redraw(shape, relativePosition, relativeSize);
        }

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
        Redraw(shape, relativePosition, relativeSize);
    }

    internal static void Redraw(Shape shape, Double2D relativePosition, Double2D relativeSize)
    {
        var renderedPosition = RelativeMeasure.ToActualObjectPosition(relativePosition)
                               - (shape.StrokeThickness, shape.StrokeThickness);
        var renderedSize = RelativeMeasure.ToActualObjectSize(relativeSize);

        Canvas.SetLeft(shape, renderedPosition.X + shape.StrokeThickness);
        Canvas.SetTop(shape, renderedPosition.Y + shape.StrokeThickness);
        (shape.Width, shape.Height) = renderedSize;
    }

    internal void HideSelectionRect()
    {
        ShapeCollection.SelectionRectangle.Visibility = Visibility.Collapsed;
    }
}