using Animperium.FileData;

namespace Animperium.Settings;

// Add color themes into this in the future.
public class AppSettings
{
    public static AppSettings Default { get; internal set; } = StandardSettings.ForApp;

    public required RelativeMeasure2D RelativeMeasure2D { get; set; }

    public AnimationTime AnimationTime { get; set; } = new(5, 0, 0, 0);
}