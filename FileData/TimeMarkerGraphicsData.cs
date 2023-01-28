using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using MathAnim.ColorThemes;
using MathAnim.Controls;
using MathAnim.Settings;

namespace MathAnim.FileData;

[Flags]
public enum TimelineLocation : byte
{
    HasStart = 1,
    HasCurrentFrame = 1 << 1,
    HasEnd = 1 << 2,
    FullTimeline = HasStart | HasCurrentFrame | HasEnd
}

public class TimeMarkerGraphicsData
{
    public TimelineLocation TimelinePosition { get; internal set; } = TimelineLocation.FullTimeline;

    private static readonly double TimelineHeight = AnimationCanvas.TimelineHeight;

    public required AnimationCanvas AnimationCanvas { get; init; }
    internal Transform1D TimelineTransform { get; set; } = Transform1D.Identity;
    public TimeMarkerData MarkerData => AnimationCanvas.MarkerData;
    private AnimationTime TotalTime => MarkerData.TotalTime;
    private byte FramesPerSecond => AnimationCanvas.FramesPerSecond;
    internal Dictionary<uint, Line> FrameMarkers { get; } = new();
    internal TimelineColorTheme TimelineColorTheme { get; set; } = new(
        (Colors.Gray, 1.6d),
        (new ArgbColor(0xA0, 0xA0, 0xA0), 1.4d),
        (new ArgbColor(0xC0, 0xC0, 0xC0), 1.2d),
        (new ArgbColor(0xE0, 0xE0, 0xE0), 1d)
    );
    internal double MinimumFrameMarkerGap => AnimationCanvas.ActualWidth / TotalTime.TotalFrames;

    internal double Tolerance => AnimationCanvas.AssociatedFile.DoubleTolerance.AsDouble();


    private double _frameMarkerGap;
    internal double FrameMarkerGap
    {
        get
        { if (_frameMarkerGap == 0) _frameMarkerGap = MinimumFrameMarkerGap; return _frameMarkerGap; }
        set
        {
            if (value < MinimumFrameMarkerGap) return;
            if (Math.Abs(value - _frameMarkerGap) < 1 + Tolerance) return;

            // TODO: Update Frame Marker Locations

            _frameMarkerGap = value;
        }
    }

    /// <summary>
    /// Renders all the time markers on the animation timeline. 
    /// </summary>
    internal void DrawTimeMarkers()
    {
        var framesPerMinute = FramesPerSecond * 60;
        var framesPerHour = framesPerMinute * 60;

        var framesUntilSecond = FramesPerSecond;
        var framesUntilMinute = framesPerMinute;
        var framesUntilHour = framesPerHour;

        var children = AnimationCanvas.TimelineCanvas.Children;

        for (uint i = 0; i < TotalTime.TotalFrames; i++)
        {
            --framesUntilSecond;
            --framesUntilMinute;
            --framesUntilHour;

            if (i == 0) continue;

            var x = FrameMarkerGap * i;

            if (framesUntilHour == 0)
            {
                DrawMarker(TimeDividers.Hours, x, i, children);
                framesUntilHour = framesPerHour;
            }
            else if (framesUntilMinute == 0)
            {
                DrawMarker(TimeDividers.Minutes, x, i, children);
                framesUntilMinute = framesPerMinute;
            }
            else if (framesUntilSecond == 0)
            {
                DrawMarker(TimeDividers.Seconds, x, i, children);
                framesUntilSecond = FramesPerSecond;
            }
            else DrawMarker(TimeDividers.Frames, x, i, children);
        }
    }

    internal void Add(uint frame, TimeDividers divider, Line line, UIElementCollection childCollection)
    {
        MarkerData.Add(frame, divider);
        FrameMarkers.Add(frame, line);
        childCollection.Add(line);
    }
    internal void Clear() => FrameMarkers.Clear();

    private void DrawMarker(TimeDividers divider, double x, uint frame, UIElementCollection children)
    {
        var line = new Line { X1 = 0, Y1 = 0, X2 = 0, Y2 = TimelineHeight };
        Canvas.SetLeft(line, x); Panel.SetZIndex(line, (int)divider);
        (line.Stroke, line.StrokeThickness) = TimelineColorTheme[divider];
        Add(frame, divider, line, children);
    }

    /// <summary>
    /// Hides time dividers marked 0, and shows the ones marked 1.
    /// </summary>
    public void ShowTimeMarkers(TimeDividers timeDividers)
    {
        bool HasFlag(TimeDividers divider) => timeDividers.HasFlag(divider);
        
        void UpdateVisibility(TimeDividers d)
        {
            void SetVisibility(Visibility visibility) => MarkerData.DividerFrames[d]
                .ForEach(f => FrameMarkers[f].Visibility = visibility);

            SetVisibility(HasFlag(d) ? Visibility.Visible : Visibility.Hidden);
        }

        void UpdateVisibilities(params TimeDividers[] dividers) => dividers.ToList().ForEach(UpdateVisibility);

        UpdateVisibilities(TimeDividers.Frames, TimeDividers.Seconds, TimeDividers.Minutes, TimeDividers.Hours);
    }
}