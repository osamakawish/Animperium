using System;
using System.Windows;

namespace MathAnim.Controls;

public partial class AnimationCanvas
{
    private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
    {
        // TODO: Update the timeline
    }

    /// <summary>
    /// Removes the keyframes from the canvas.
    /// </summary>
    internal void ClearKeyframes()
    {
        // TODO
    }

    internal void MoveTimeline(TimeSpan time, bool isForward = true)
    {
        if (TimelinePosition == TimelineLocation.FullTimeline) return;

        // TODO
    }

    internal void MoveTimeline(int frames)
    {
        if (TimelinePosition == TimelineLocation.FullTimeline) return;

        // TODO
        var dist = frames * FrameMarkerGap;
    }

    internal void ScaleTimeline(double scale)
    {
        // TODO
        // F : x -> a*(x-p) + p : rescales timeline centered at p by scale factor of a. Apply F to every line.
        // Make sure that neither start nor end of timeline are anywhere but at the edges or outside.
        // In fact, test that before applying F to every line, then update F accordingly.

        // TODO: Make sure to update TimelinePosition
    }
}