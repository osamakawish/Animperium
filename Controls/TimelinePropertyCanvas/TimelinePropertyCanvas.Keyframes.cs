using System;
using System.Windows.Controls;

namespace Animperium.Controls.TimelinePropertyCanvas;

public partial class TimelinePropertyCanvas
{
    private double GetLeft(uint frame) => Canvas.GetLeft(FrameMarkers[frame]);

    private double GetLeft(TimeSpan timespan) => GetLeft(GetFrame(timespan));

    internal void AddKeyframe(TimeSpan time)
    {
        // TODO
    }

    internal void RemoveKeyframe(TimeSpan time)
    {
        // TODO
    }

    internal void AddKeyframe(uint frame)
    {
        // TODO
    }

    internal void RemoveKeyframe(uint frame)
    {
        // TODO
    }
}