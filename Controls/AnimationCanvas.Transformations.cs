using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using MathAnim.FileData;

namespace MathAnim.Controls;

public partial class AnimationCanvas
{
    private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
    {
        // TODO: Update the timeline
        var transform = TimelineTransform;
        transform = transform.FromZeroFixedMapping
            (new Mapping<double>(sizeChangedEventArgs.PreviousSize.Width, sizeChangedEventArgs.NewSize.Width));
        SetTransform(transform);
    }

    /// <summary>
    /// Removes the keyframes from the timeline canvas. Useful for resetting image.
    /// </summary>
    internal void ClearKeyframes()
    {
        foreach (var line in FrameMarkers.Values) TimelineCanvas.Children.Remove(line);
        Clear(); MarkerData.Clear();
        TimelinePosition = 0;
    }

    internal void MoveTimeline(TimeSpan time, bool isForward = true)
        => MoveTimeline((isForward ? 1 : -1) * (int)(time.TotalSeconds * FramesPerSecond));

    internal void MoveTimeline(int frames) 
        => ApplyTransform(new Transform1D(Shift: frames * FrameMarkerGap));

    internal void ScaleTimeline(double scale) => ApplyTransform(new Transform1D(scale));

    internal void SetTransform(Transform1D transform) => ApplyTransform(transform, true);

    internal void ApplyTransform(Transform1D transform, bool isSet=false)
    {
        if (TimelinePosition == TimelineLocation.FullTimeline
            && transform.Scale < 1 + Tolerance)
            return;

        TimelineTransform =
            (isSet ? transform : TimelineTransform.ApplyTransform(transform))
            .Fix(TimelineCanvas.ActualWidth, out var hasEnds);
        
        // DEBUG isSet=true case. Current code here applies to isSet=false.
        foreach (var line in FrameMarkers.Values)
            Canvas.SetLeft(line, transform[Canvas.GetLeft(line)]);
        
        if (hasEnds.hasZero) TimelinePosition = TimelineLocation.HasStart;
        if (hasEnds.hasMax) TimelinePosition |= TimelineLocation.HasEnd;
    }
}