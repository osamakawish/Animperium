using MathAnim.FileData;

namespace MathAnim.Settings;

/// <summary>
/// A set of default values for the application. Can be modified by users.
/// </summary>
public class FileSettings
{
    public static FileSettings Default { get; internal set; } = StandardSettings.ForFile;

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