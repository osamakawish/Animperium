using System.Windows.Shapes;
using System.Windows;

namespace Animperium.Controls;

public partial class AnimationCanvas
{
    public static readonly DependencyProperty XProperty
        = DependencyProperty.RegisterAttached(
            name: "X",
            propertyType: typeof(double),
            ownerType: typeof(AnimationCanvas),
            new FrameworkPropertyMetadata(0d,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnXChanged));

    private static void OnXChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        var shape = (Shape)obj;

        var canvas = ShapeToAssociatedCanvas[shape];
        var (position, size) = canvas._shapes[shape];
        canvas._shapes[shape] = (((double)args.NewValue, position.Y), size);
        canvas.UpdateShapeRendering(shape);
    }

    public static void SetX(Shape shape, double x) => shape.SetValue(XProperty, x);
    public static double GetX(Shape shape) => ShapeToAssociatedCanvas[shape]._shapes[shape].position.X;

    public static readonly DependencyProperty YProperty
        = DependencyProperty.RegisterAttached(
            name: "Y",
            propertyType: typeof(double),
            ownerType: typeof(AnimationCanvas),
            new FrameworkPropertyMetadata(0d,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnYChanged));

    private static void OnYChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        var shape = (Shape)obj;

        var canvas = ShapeToAssociatedCanvas[shape];
        var (position, size) = canvas._shapes[shape];
        canvas._shapes[shape] = ((position.X, (double)args.NewValue), size);
        canvas.UpdateShapeRendering(shape);
    }

    public static void SetY(Shape shape, double y) => shape.SetValue(YProperty, y);
    public static double GetY(Shape shape) => ShapeToAssociatedCanvas[shape]._shapes[shape].position.Y;

    public static readonly DependencyProperty ShapeWidthProperty
        = DependencyProperty.RegisterAttached(
            name: "ShapeWidth",
            propertyType: typeof(double),
            ownerType: typeof(AnimationCanvas),
            new FrameworkPropertyMetadata(1d,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnShapeWidthChanged));

    private static void OnShapeWidthChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        var shape = (Shape)obj;

        var canvas = ShapeToAssociatedCanvas[shape];
        var (position, size) = canvas._shapes[shape];
        canvas._shapes[shape] = (position, ((double)args.NewValue, size.Y));
        canvas.UpdateShapeRendering(shape);
    }

    public static void SetShapeWidth(Shape shape, double width) => shape.SetValue(ShapeWidthProperty, width);
    public static double GetShapeWidth(Shape shape) => ShapeToAssociatedCanvas[shape]._shapes[shape].size.X;

    public static readonly DependencyProperty ShapeHeightProperty
        = DependencyProperty.RegisterAttached(
            name: "ShapeHeight",
            propertyType: typeof(double),
            ownerType: typeof(AnimationCanvas),
            new FrameworkPropertyMetadata(1d,
                FrameworkPropertyMetadataOptions.AffectsRender,
                OnShapeHeightChanged));

    private static void OnShapeHeightChanged(DependencyObject obj, DependencyPropertyChangedEventArgs args)
    {
        var shape = (Shape)obj;

        var canvas = ShapeToAssociatedCanvas[shape];
        var (position, size) = canvas._shapes[shape];
        canvas._shapes[shape] = (position, (size.X, (double)args.NewValue));
        canvas.UpdateShapeRendering(shape);
    }

    public static void SetShapeHeight(Shape shape, double height) => shape.SetValue(ShapeHeightProperty, height);
    public static double GetShapeHeight(Shape shape) => (double)shape.GetValue(ShapeHeightProperty);
}