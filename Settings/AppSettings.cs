using MathAnim.FileData;

namespace MathAnim.Settings;

// Add color themes into this in the future.
public class AppSettings
{
    public static AppSettings Default { get; internal set; } = StandardSettings.ForApp;

    public required RelativeMeasure2D RelativeMeasure2D = StandardSettings.ForApp.RelativeMeasure2D;
}