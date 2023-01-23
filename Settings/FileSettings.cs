using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathAnim.FileData;

namespace MathAnim.Settings
{
    /// <summary>
    /// A set of default values for the application. Can be modified by users.
    /// </summary>
    internal record FileSettings(DoubleTolerance Tolerance, AnimationTime AnimationTime)
    {
        internal static FileSettings Default = StandardSettings.StandardFileSettings;

        public double ToleranceAsDouble => Tolerance.AsDouble();
    }
}
