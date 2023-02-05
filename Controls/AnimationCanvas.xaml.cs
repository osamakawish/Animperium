using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MathAnim.FileData;

namespace MathAnim.Controls
{
    /// <summary>
    /// Interaction logic for AnimationCanvas.xaml
    /// </summary>
    public partial class AnimationCanvas
    {
        internal static RelativeMeasure2D RelativeMeasure => BaseRelativeMeasureC.Standard;

        private readonly Dictionary<Shape, (Double2D position, Double2D size)> _shapes = new();

        public AnimationCanvas()
        {
            InitializeComponent();

            RelativeMeasure.ActualCanvasSize = (Canvas.ActualWidth, Canvas.ActualHeight);

            AddShape<Ellipse>();
        }

        internal void AddShape<TShape>(
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
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            // DEBUG
            base.OnRenderSizeChanged(sizeInfo);

            // TODO: Make sure the size ratio aligns with RelativeMeasure2D's size ratio.

            RelativeMeasure.ActualCanvasSize = sizeInfo.NewSize;
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
            var renderedPosition = RelativeMeasure.ToRenderedObjectPosition(relativePosition);
            var renderedSize = RelativeMeasure.ToRenderedObjectSize(relativeSize);

            Canvas.SetLeft(shape, renderedPosition.X); Canvas.SetTop(shape, renderedPosition.Y);
            (shape.Width, shape.Height) = renderedSize;
        }
    }
}
