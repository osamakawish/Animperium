using System.Windows.Controls;
using System.Windows.Input;

namespace Animperium.Graphics;

public delegate void MouseReaction<in T>(Canvas canvas, T mouseButtonEventArgs)
    where T : MouseEventArgs;

/// <summary>
/// For defining various tools for animations.
/// </summary>
/// <param name="OnDown">Behaviour when mouse is initially pressed down.</param>
/// <param name="OnHold">Behaviour when mouse button is held.</param>
/// <param name="OnUp">Behaviour when the mouse button is released.</param>
/// <param name="IsAuditory">False (by default) for tools that apply to graphics.
/// True if it adds or manipulates audio.</param>
public record AnimationTool(
    MouseReaction<MouseButtonEventArgs> OnDown,
    MouseReaction<MouseEventArgs> OnHold,
    MouseReaction<MouseButtonEventArgs> OnUp,
    bool IsAuditory = false)
{
    private static bool StateIsPressed(MouseButtonState state) => state == MouseButtonState.Pressed;

    private static bool IsPressed(MouseEventArgs e) => StateIsPressed(e.LeftButton)
                                                       || StateIsPressed(e.MiddleButton)
                                                       || StateIsPressed(e.RightButton);
    public MouseReaction<MouseEventArgs> OnMove => (o, e) => { if (IsPressed(e)) OnHold(o, e); };
}

internal static class AnimationTools
{
    // Mouse tool
    internal static readonly AnimationTool ItemSelectTool = new(
        (canvas, args) => { },
        (canvas, args) => { },
        (canvas, args) => { }
    );

    // Circle Tool
    internal static readonly AnimationTool EllipseTool = new(
        (canvas, args) => { },
        (canvas, args) => { },
        (canvas, args) => { }
    );
}