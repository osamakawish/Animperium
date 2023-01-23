using System;
using MathAnim.FileData;

namespace MathAnim.Settings;

public enum DoubleTolerance { Low = 2, Medium = 4, High = 6 }

/// <summary>
/// The standard values for variables and properties. These must all be readonly and cannot be modified.
/// </summary>
internal static class StandardSettings
{
    public static double AsDouble(this DoubleTolerance tolerance) => Math.Pow(10, -(int)tolerance);

    public static readonly FileSettings StandardFileSettings
        = new(DoubleTolerance.Medium, new AnimationTime(0, 1, 0, 0));
}