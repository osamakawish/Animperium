using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace MathAnim.Graphics;

public class ShapeCollection : ICollection<Shape>
{
    private readonly HashSet<Shape> _shapes = new();
    private readonly HashSet<Shape> _selected = new();

    /// <summary>
    /// Needs testing: Intended to be the bounds of the selected items.
    /// </summary>
    public Rect SelectionBounds {
        get {
            var rect = _selected.First().RenderedGeometry.Bounds;

            foreach (var shape in _selected.Skip(1))
                rect.Union(shape.RenderedGeometry.Bounds);
            
            return rect;
        }
    }

    public IEnumerator<Shape> GetEnumerator() => _shapes.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(Shape item) { _shapes.Add(item); }
    public void Clear() { _shapes.Clear(); }
    public bool Contains(Shape item) => _shapes.Contains(item);

    public void CopyTo(Shape[] array, int arrayIndex) => _shapes.CopyTo(array, arrayIndex);
    public bool Remove(Shape item) => _shapes.Remove(item);

    public int Count => _shapes.Count;
    public bool IsReadOnly => false;

    public bool Select(Shape shape) => _selected.Add(shape);
    public void Select(IEnumerable<Shape> shapes, bool append = false)
    { if (!append) _selected.Clear(); foreach (var shape in shapes) Select(shape); }
    public void SelectAll() { foreach (var shape in _shapes) Select(shape); }

    public void DeselectAll() => _selected.Clear();
    public void Deselect(IEnumerable<Shape> shapes)
    { foreach (var shape in shapes) Deselect(shape); }
    public bool Deselect(Shape shape) => _selected.Remove(shape);
}