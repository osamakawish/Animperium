using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using MathAnim.Controls;

namespace MathAnim.Graphics;

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