using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using Animperium.Controls;
using Animperium.Essentials;

namespace Animperium.Graphics;

public class VisualMouseEventArgs<T> where T : MouseEventArgs
{
    public Point Start { get; internal init; }
    public Point End { get; internal init; }
    public ShapeCollection Shapes { get; internal init; } = null!;
    internal T MouseEventArgs { get; init; } = null!;

    public Rect Rect => new(Start, End);
    public HitTestResult? HitTestResult { get; internal init; }
}

/// <summary>
/// 
/// </summary>
/// <typeparam name="T"></typeparam>
public delegate void VisualMouseReaction<T>(VisualMouseEventArgs<T> mouseEventArgs)
    where T : MouseEventArgs;

public interface IVisualMouseReactor
{
    void OnDown(VisualMouseEventArgs<MouseButtonEventArgs> e);
    void OnHold(VisualMouseEventArgs<MouseEventArgs> e);
    void OnUp(VisualMouseEventArgs<MouseButtonEventArgs> e);
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
    public VisualMouseReaction<MouseEventArgs> OnMove => 
        (args) => { if (IsPressed(args.MouseEventArgs)) OnHold(args); };
}

internal static class AnimationTools
{
    // Mouse tool
    internal static readonly VisualAnimationTool ItemSelectTool = new(
        args => { },
        args => { },
        args => { }
    ) { Name = "Mouse Tool" };

    // Rect Tool: DEBUG
    private static Rectangle? _rect;
    internal static readonly VisualAnimationTool RectangleTool = new(
        args => _rect = ShapeExtensions.Create<Rectangle>(args.Start),
        args => _rect!.SetShapeRegion(args.Start, args.End),
        args => { _rect!.SetShapeRegion(args.Start, args.End); _rect = null; }
    )
    { Name = "Ellipse Tool" };

    // Ellipse Tool: DEBUG on down event
    private static Ellipse? _ellipse;
    internal static readonly VisualAnimationTool EllipseTool = new(
        args => _ellipse = ShapeExtensions.Create<Ellipse>(args.Start),
        args => _ellipse!.SetShapeRegion(args.Start, args.End),
        args => { _ellipse!.SetShapeRegion(args.Start, args.End); _ellipse = null; }
    ) { Name = "Ellipse Tool" };
}