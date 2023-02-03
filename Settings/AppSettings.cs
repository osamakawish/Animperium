namespace MathAnim.Settings;

// Add color themes into this in the future.
public class AppSettings
{
    public static AppSettings Default { get; internal set; } = StandardSettings.ForApp;

    public required double CanvasLength = StandardSettings.ForApp.CanvasLength;
}