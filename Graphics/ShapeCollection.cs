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
using Animperium.Graphics.Selection;

namespace Animperium.Graphics;

public class ShapeCollection : ICollection<Shape>
{
    internal ShapeCollection(AnimationCanvas animationCanvas)
    {
        SelectionRectangle = animationCanvas.AddShape<Rectangle>(relativeSize: (2, 2), isDecorative: true);
        SelectionColorTheme = new SelectionRectColorTheme(Brushes.Black, new DoubleCollection(new double[] { 2, 3, 2, 3 }));
        Panel.SetZIndex(SelectionRectangle, int.MaxValue);

        const double halfPi = 0.5 * Math.PI;
        RotationButtons = new RotationButtons(
            new RotationButtonData(animationCanvas, new Point(0, 0), .5, 0),
            new RotationButtonData(animationCanvas, new Point(0, 0), .5, -halfPi),
            new RotationButtonData(animationCanvas, new Point(0, 0), .5, halfPi),
            new RotationButtonData(animationCanvas, new Point(0, 0), .5, Math.PI)
        );

        _animationCanvas = animationCanvas;
    }

    private readonly HashSet<Shape> _shapes = new();
    private readonly HashSet<Shape> _selected = new();

    private readonly AnimationCanvas _animationCanvas;

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

    private RotationButtons RotationButtons { get; }

    // TODO: Use SelectionRectangle class for this later.
    internal Rectangle SelectionRectangle { get; }

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

    internal void ShowRotationButtons() =>
        // Note: All rotation buttons behave effectively the same way:
        // rotate by angle between point clicked current cursor position.
        RotationButtons.ForEach(button => button.Arc.Visibility = Visibility.Visible);

    internal void HideRotationButtons() => RotationButtons.ForEach(button => button.Arc.Visibility = Visibility.Collapsed);
}