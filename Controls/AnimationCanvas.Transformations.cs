using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using MathAnim._Debug;
using MathAnim.FileData;

namespace MathAnim.Controls;

public partial class AnimationCanvas
{
    //internal FileLogger FileLogger = new($"{nameof(AnimationCanvas)}.Transformations");

    private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs) => UpdateMarkers();

    /// <summary>
    /// Removes the keyframes from the timeline canvas. Useful for resetting image.
    /// </summary>
    internal void ClearKeyframes() {
        foreach (var line in FrameMarkers.Values)
            TimelineCanvas.Children.Remove(line);
        Clear(); MarkerData.Clear(); TimelinePosition = 0;
    }

    internal void MoveTimeline(TimeSpan time, bool isForward = true)
        => MoveTimeline((isForward ? 1 : -1) * (int)(time.TotalSeconds * FramesPerSecond));

    internal void MoveTimeline(int frames) => ApplyTransform(new Transform1D(Shift: frames * BaseMarkerGap));

    internal void ScaleTimeline(double scale) => ApplyTransform(new Transform1D(scale));

    internal void SetTransform(Transform1D transform) => ApplyTransform(transform);

    /// <summary>
    /// Applies given transform onto the existing transform.
    /// </summary>
    /// <param name="transform"></param>
    internal void ApplyTransform(Transform1D transform) {
        if (transform.IsInvalid) return;
        if (TimelinePosition == TimelineLocation.FullTimeline && transform.Scale < 1 + Tolerance) return;

        TimelineTransform =
            TimelineTransform.ApplyTransform(transform).Fix(TimelineCanvas.ActualWidth, out var hasEnds);
        UpdateMarkers();
        
        if (hasEnds.hasZero) TimelinePosition = TimelineLocation.HasStart;
        if (hasEnds.hasMax) TimelinePosition |= TimelineLocation.HasEnd;
    }

    internal void UpdateMarkers() {
        foreach (var (frame, markerLine) in FrameMarkers)
            Canvas.SetLeft(markerLine, TimelineTransform[frame * BaseMarkerGap]);
    }
}