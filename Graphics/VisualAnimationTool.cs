using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Shapes;
using Animperium.Controls;

namespace Animperium.Graphics;

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="mousePathRect">The rectangle from the starting point of the mouse press to where it is now.</param>
/// <param name="shapes"></param>
/// <param name="mouseEventArgs"></param>
public delegate void VisualMouseReaction<in T>(Rect mousePathRect, ShapeCollection shapes, T mouseEventArgs)
    where T : MouseEventArgs;

public interface IVisualMouseReactor
{
    void OnDown(Rect mouseRect, ShapeCollection shapes, MouseButtonEventArgs e);
    void OnHold(Rect mouseRect, ShapeCollection shapes, MouseEventArgs e);
    void OnUp(Rect mouseRect, ShapeCollection shapes, MouseButtonEventArgs e);
}

//public delegate void AuditoryMouseReaction

/// <summary>
/// For defining various tools for animations, particularly graphics-related tools involving the animation canvas.<br/>
/// This class is named in contrast to auditory tools, which affect audio.
/// </summary>
/// <param name="OnDown">Behaviour when mouse is initially pressed down.</param>
/// <param name="OnHold">Behaviour when mouse button is held.</param>
/// <param name="OnUp">Behaviour when the mouse button is released.</param>
/// <example></example>
public record VisualAnimationTool(
    VisualMouseReaction<MouseButtonEventArgs> OnDown,
    VisualMouseReaction<MouseEventArgs> OnHold,
    VisualMouseReaction<MouseButtonEventArgs> OnUp)
{
    public string Name { get; set; } = "";
    public override string ToString() => (string.IsNullOrEmpty(Name) ? base.ToString() : Name)!;

    public static VisualAnimationTool FromReactor(IVisualMouseReactor reactor)
        => new(reactor.OnDown, reactor.OnHold, reactor.OnUp);

    private static bool StateIsPressed(MouseButtonState state) => state == MouseButtonState.Pressed;

    private static bool IsPressed(MouseEventArgs e) => StateIsPressed(e.LeftButton)
                                                       || StateIsPressed(e.MiddleButton)
                                                       || StateIsPressed(e.RightButton);
    public VisualMouseReaction<MouseEventArgs> OnMove => (r, o, e) => { if (IsPressed(e)) OnHold(r, o, e); };
}

internal static class AnimationTools
{
    // Mouse tool
    internal static readonly VisualAnimationTool ItemSelectTool = new(
        (rect, shapes, args) => { },
        (rect, shapes, args) => { },
        (rect, shapes, args) => { }
    ) { Name = "Mouse Tool" };

    // Rect Tool: DEBUG
    private static Shape? _rect;
    internal static readonly VisualAnimationTool RectangleTool = new(
            (rect, _, _) => _rect = ShapeExtensions.Create<Rectangle>(rect.Location),
            (rect, _, _) => _rect!.SetShapeRegion(rect),
            (rect, _, _) => { _rect!.SetShapeRegion(rect); _rect = null; }
        )
        { Name = "Ellipse Tool" };

    // Ellipse Tool: DEBUG on down event
    private static Shape? _ellipse;
    internal static readonly VisualAnimationTool EllipseTool = new(
        (rect, _, _) => _ellipse = ShapeExtensions.Create<Ellipse>(rect.Location),
        (rect, _, _) => _ellipse!.SetShapeRegion(rect),
        (rect, _, _) => { _ellipse!.SetShapeRegion(rect); _ellipse = null; }
    ) { Name = "Ellipse Tool" };
}