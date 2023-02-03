using System;
using MathAnim.FileData;

namespace MathAnim.Settings;

public enum DoubleTolerance { Low = 2, Medium = 4, High = 6 }

/// <summary>
/// The standard values for variables and properties. These must all be readonly and cannot be modified.
/// This is to allow users to customize default values. If they wish to reset the default values, they reset
/// will be taken from <see cref="StandardSettings"/>.
/// </summary>
internal static class StandardSettings
{
    public static double AsDouble(this DoubleTolerance tolerance) => Math.Pow(10, -(int)tolerance);

    public static readonly FileSettings StandardFileSettings
        = new(DoubleTolerance.Medium, new AnimationTime(0, 1, 0, 0));

    public static readonly AppSettings StandardAppSettings
        = new() { CanvasLength = 24 };
}