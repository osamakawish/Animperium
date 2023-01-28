using MathAnim.FileData;

namespace MathAnim.Settings;

/// <summary>
/// A set of default values for the application. Can be modified by users.
/// </summary>
internal class FileSettings
{
    internal static FileSettings Default = StandardSettings.StandardFileSettings;

    /// <summary>
    /// A set of default values for the application. Can be modified by users.
    /// </summary>
    public FileSettings(DoubleTolerance tolerance, AnimationTime animationTime)
    {
        Tolerance = tolerance;
        AnimationTime = animationTime;
    }

    public double ToleranceAsDouble => Tolerance.AsDouble();
    public DoubleTolerance Tolerance { get; init; }
    public AnimationTime AnimationTime { get; init; }
}