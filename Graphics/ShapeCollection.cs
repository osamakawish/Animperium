using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Animperium.Controls;
using Animperium.Essentials;

namespace Animperium.Graphics;

public class ShapeCollection : ICollection<Shape>
{
    internal ShapeCollection(AnimationCanvas animationCanvas)
    {
        SelectionRectangle = animationCanvas.AddShape<Rectangle>(relativeSize: (2, 2), isDecorative: true);
        SelectionColorTheme = new SelectionRectColorTheme(Brushes.Black, new DoubleCollection(new double[] { 2, 3, 2, 3 }));
        Panel.SetZIndex(SelectionRectangle, int.MaxValue);
    }

    private readonly HashSet<Shape> _shapes = new();
    private readonly HashSet<Shape> _selected = new();

    private SelectionRectColorTheme _selectionRectColorTheme
        = new(Brushes.Black, new DoubleCollection(new double[] { 2, 3, 2, 3 }));
    internal SelectionRectColorTheme SelectionColorTheme {
        get => _selectionRectColorTheme;
        set {
            SelectionRectangle.StrokeThickness = value.Thickness;
            SelectionRectangle.Stroke = value.Brush;
            SelectionRectangle.StrokeDashArray = value.StrokeDashArray;

            UpdateSelectionRectangle();
            
            _selectionRectColorTheme = value;
        }
    }

    internal readonly Rectangle SelectionRectangle;

    /// <summary>
    /// Needs testing: Intended to be the bounds of the selected items.
    /// </summary>
    public Rect GetSelectionBounds()
    {
        if (SelectionIsEmpty()) return new Rect(Double2D.Zero, (Size)Double2D.Zero);

        var rect = _selected.First().GetRelativeBounds();

        foreach (var shape in _selected.Skip(1)) rect.Union(shape.GetRelativeBounds());

        return rect;
    }

    public IEnumerator<Shape> GetEnumerator() => _shapes.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(Shape shape) { _shapes.Add(shape); }
    public void Clear() { _shapes.Clear(); }
    public bool Contains(Shape item) => _shapes.Contains(item);

    public void CopyTo(Shape[] array, int arrayIndex) => _shapes.CopyTo(array, arrayIndex);
    public bool Remove(Shape item) => _shapes.Remove(item);

    public int Count => _shapes.Count;
    public bool IsReadOnly => false;

    private void UpdateSelectionRectangle()
    {
        var bounds = GetSelectionBounds();
        var offset = SelectionColorTheme.Offset *
                     AnimationCanvas.RelativeMeasure.XMeasure.ToRelativeObjectSize(_selectionRectColorTheme.Thickness);
        bounds.Inflate(offset, offset);
        AnimationCanvas.Redraw(SelectionRectangle, bounds.TopLeft, bounds.BottomRight);
    }

    public bool SelectionIsEmpty() => _selected.Count == 0;
    public bool Select(Shape shape) => _selected.Add(shape);
    public void Select(IEnumerable<Shape> shapes, bool append = false)
    { if (!append) _selected.Clear(); foreach (var shape in shapes) Select(shape); }
    public void SelectAll() { foreach (var shape in _shapes) Select(shape); }

    public void DeselectAll() => _selected.Clear();
    public void Deselect(IEnumerable<Shape> shapes) { foreach (var shape in shapes) Deselect(shape); }
    public bool Deselect(Shape shape) => _selected.Remove(shape);

    internal bool ShowSelectionRect()
    {
        UpdateSelectionRectangle();
        if (SelectionIsEmpty())
        {
            SelectionRectangle.Visibility = Visibility.Collapsed;
            return false;
        }
        SelectionRectangle.Visibility = Visibility.Visible;
        return true;
    }

    internal void ShowRotationButtons()
    {
        // Note: All rotation buttons behave effectively the same way:
        // rotate by angle between point clicked current cursor position.
    }
}

public record SelectionRectColorTheme(
    SolidColorBrush Brush,
    DoubleCollection StrokeDashArray,
    double Thickness = 1.5,
    double Offset = 0);

internal record RotationArcButton
{
    public RotationArcButton(AnimationCanvas canvas, Point center, double radius, double baseAngle)
    {
        Canvas = canvas;
        Radius = radius;
        BaseAngle = baseAngle;
        Center = center;
        Angle = baseAngle;
        var arcData = canvas.AddArc(center, (radius, radius), (baseAngle, baseAngle + Math.PI), true);
        Arc = arcData.Path;
        Figure = arcData.Figure;
        ArcSegment = arcData.ArcSegment;
    }
    private Path Arc { get; }
    private PathFigure Figure { get; }
    private ArcSegment ArcSegment { get; }

    internal required Point Center { get; set; } // Modify Arc upon change.
    internal required double Angle { get; set; } // Modify arc upon change.

    public AnimationCanvas Canvas { get; init; }
    public double Radius { get; init; }
    public double BaseAngle { get; init; }

    public void Deconstruct(out AnimationCanvas canvas, out Point center, out double radius, out double baseAngle)
    {
        canvas = Canvas;
        center = Center;
        radius = Radius;
        baseAngle = BaseAngle;
    }
}

internal record RotationButtons(
    RotationArcButton TopLeft,
    RotationArcButton TopRight,
    RotationArcButton BottomLeft,
    RotationArcButton BottomRight)
{
    private double _additionalRotation;

    private void ApplyToEach(Action<RotationArcButton> action)
        { action(TopLeft); action(TopRight); action(BottomLeft); action(BottomRight); }

    internal void SetPoints(Point topLeft, Point topRight, Point bottomLeft, Point bottomRight)
        => (TopLeft.Center, TopRight.Center, BottomLeft.Center, BottomRight.Center)
            = (topLeft, topRight, bottomLeft, bottomRight);

    internal double AdditionalRotation {
        get => _additionalRotation;
        set {
            ApplyToEach(x => x.Angle = x.BaseAngle + value);
            _additionalRotation = value;
        }
    }
}