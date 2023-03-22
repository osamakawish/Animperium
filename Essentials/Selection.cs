using System.Windows;
using System.Windows.Shapes;

namespace Animperium.Essentials;

internal class Selection
{
    internal Shape? Item { get; set; }
    internal DependencyProperty? Property { get; set; }
    // Should be able to 
    internal object? Keyframe { get; set; }
}