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
    internal FileLogger FileLogger = new($"{nameof(AnimationCanvas)}.Transformations");

    private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
    {
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
        FileLogger.LogLine($"{nameof(ApplyTransform)}: transform = {transform}{(isSet ? " (set)" : "")}");
        if (transform.IsInvalid) return;

        if (TimelinePosition == TimelineLocation.FullTimeline
            && transform.Scale < 1 + Tolerance)
            return;

        // DEBUG: ApplyTransform doesn't appear to scale properly. That, or transform is invalid too frequently.
        TimelineTransform =
            (isSet ? transform : TimelineTransform.ApplyTransform(transform))
            .Fix(TimelineCanvas.ActualWidth, out var hasEnds);

        // kv.Key = frame, kv.Value = Line drawn for given frame.
        // Note: This is efficient code, as it calculates isSet once for the entire collection, instead of once
        // per line. "kv => isSet ? ..." would be slower as it would compute isSet for every individual line.
        Func<KeyValuePair<uint, Line>, double> getLeft
            = isSet ? kv => FrameMarkerGap * kv.Key : kv => transform[Canvas.GetLeft(kv.Value)];
        foreach (var frameMarker in FrameMarkers) Canvas.SetLeft(frameMarker.Value, getLeft(frameMarker));

        if (hasEnds.hasZero) TimelinePosition = TimelineLocation.HasStart;
        if (hasEnds.hasMax) TimelinePosition |= TimelineLocation.HasEnd;
    }
}