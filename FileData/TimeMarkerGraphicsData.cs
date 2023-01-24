using System.Collections.Generic;
using System.Windows.Media;
using System.Windows.Shapes;
using MathAnim.ColorThemes;
using MathAnim.Controls;

namespace MathAnim.FileData;

public enum TimelineLocation : byte
{
    HasStart = 1,
    HasCurrentFrame = 1 << 1,
    HasEnd = 1 << 2,
    FullTimeline = HasStart | HasCurrentFrame | HasEnd
}

internal class TimeMarkerGraphicsData
{
    internal required AnimationCanvas Canvas { get; init; }
    private Dictionary<uint, Line> FrameMarkers { get; } = new();
    // Get time marker data from canvas.
    internal TimelineColorTheme TimelineColorTheme { get; set; } = new(
        new PenStrokeData(Colors.Gray, 1.6d),
        new PenStrokeData(new ArgbColor(0xA0, 0xA0, 0xA0), 1.4d),
        new PenStrokeData(new ArgbColor(0xC0, 0xC0, 0xC0), 1.2d),
        new PenStrokeData(new ArgbColor(0xE0, 0xE0, 0xE0), 1d)
    );
    private double FrameMarkerGap { get; set; }
}