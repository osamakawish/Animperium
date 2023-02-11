using System.Windows.Controls;
using System.Windows.Input;

namespace Animperium.Graphics;

public delegate void MouseReaction<in T>(Canvas canvas, T mouseButtonEventArgs)
    where T : MouseEventArgs;

public record GraphicsTool(
    MouseReaction<MouseButtonEventArgs> OnDown,
    MouseReaction<MouseEventArgs> OnHold,
    MouseReaction<MouseButtonEventArgs> OnUp)
{
    private static bool StateIsPressed(MouseButtonState state) => state == MouseButtonState.Pressed;

    private static bool IsPressed(MouseEventArgs e) => StateIsPressed(e.LeftButton)
                                                       || StateIsPressed(e.MiddleButton)
                                                       || StateIsPressed(e.RightButton);
    public MouseReaction<MouseEventArgs> OnMove => (o, e) => { if (IsPressed(e)) OnHold(o, e); };
}

internal static class GraphicsTools
{
    // Mouse tool
    internal static readonly GraphicsTool ItemSelectTool = new(
        (canvas, args) => { },
        (canvas, args) => { },
        (canvas, args) => { }
    );

    // Circle Tool
    internal static readonly GraphicsTool EllipseTool = new(
        (canvas, args) => { },
        (canvas, args) => { },
        (canvas, args) => { }
    );
}