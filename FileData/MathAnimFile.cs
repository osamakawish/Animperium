using MathAnim.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAnim.FileData
{
    /// <summary>
    /// The animation frame stated in terms of relative and absolute position on the timeline.
    /// </summary>
    /// <param name="Relative">Initially, this is the percentage (between 0 and 1)
    /// of the total number of frames. May be modified over time as .</param>
    /// <param name="ExactFrameNo"></param>
    record AnimationFrame(decimal Relative, uint ExactFrameNo);

    class MathAnimFile
    {
        internal byte FramesPerSecond { get; set; } = 24;
        internal AnimationFrame Start { get; set; } = new(0, 0);
        internal AnimationFrame End { get; init; } = new(0, 0);

        internal GraphicsObjectTree GraphicsObjectTree { get; } = new();
    }
}
