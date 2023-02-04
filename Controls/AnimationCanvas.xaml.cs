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

        private Dictionary<Shape, Double2D> _shapeSizes = new();

        public AnimationCanvas()
        {
            InitializeComponent();

            RelativeMeasure.ActualCanvasSize = (Canvas.Width, Canvas.Height);

            Shape shape = new Ellipse();
            
        }

        internal void AddShape<TShape>(double relativeWidth = 1, double relativeHeight = 1, double strokeThickness = 1,
            SolidColorBrush? strokeColor = null, SolidColorBrush? fillColor = null) where TShape : Shape, new()
        {
            strokeColor ??= Brushes.Black;
            fillColor ??= Brushes.Transparent;

            var actualDimensions = RelativeMeasure[(relativeWidth, relativeHeight)];

            TShape shape = new() {
                Width = actualDimensions.X,
                Height = actualDimensions.Y,
                StrokeThickness = strokeThickness,
                Stroke = strokeColor,
                Fill = fillColor
            };
            _shapeSizes[shape] = (relativeWidth, relativeHeight);

            Canvas.Children.Add(shape);
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            // DEBUG
            base.OnRenderSizeChanged(sizeInfo);

            RelativeMeasure.ActualCanvasSize = sizeInfo.NewSize;
            foreach (var (shape, relativeSize) in _shapeSizes)
                (shape.Width, shape.Height) = RelativeMeasure[relativeSize];
        }
    }
}
