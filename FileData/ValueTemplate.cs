using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAnim.FileData
{
    /// <summary>
    /// A set of default values for the application. Can be modified by users.
    /// </summary>
    internal record ValueTemplate(byte FramesPerSecond, AnimationTime TotalTime)
    {
        internal static ValueTemplate Default =
            new(StandardValues.FramesPerSecond,
                StandardValues.TotalTime);
    }

    /// <summary>
    /// The standard values for variables and properties. These must all be readonly and cannot be modified.
    /// </summary>
    internal static class StandardValues
    {
        public static readonly byte FramesPerSecond = 24;
        public static readonly AnimationTime TotalTime = new(0, 1, 0, 0);
    }
}
