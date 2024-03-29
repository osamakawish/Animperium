﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using Animperium.ColorThemes;
using Animperium.FileData;
using Animperium.Settings;

namespace Animperium.Controls.TimelinePropertyCanvas;

[Flags]
public enum TimelineLocation : byte
{
    HasStart = 1,
    HasCurrentFrame = 1 << 1,
    HasEnd = 1 << 2,
    FullTimeline = HasStart | HasCurrentFrame | HasEnd
}

public partial class TimelinePropertyCanvas
{
    private Transform1D TimelineTransform { get; set; } = Transform1D.Identity;
    private AnimationTime TotalTime => MarkerData.TotalTime;
    /// <summary>
    /// Stores the drawn time marker line at the given frame.
    /// </summary>
    internal Dictionary<uint, Line> FrameMarkers { get; } = new();

    internal TimelineColorTheme TimelineColorTheme { get; set; } = new(
        (Colors.Gray, 1.6d),
        (new ArgbColor(0xA0, 0xA0, 0xA0), 1.4d),
        (new ArgbColor(0xC0, 0xC0, 0xC0), 1.2d),
        (new ArgbColor(0xE0, 0xE0, 0xE0), 1d)
    );

    /// <summary>
    /// The gap between frame markers when the entire timeline is fitted to the
    /// width of the animation canvas.
    /// </summary>
    internal double BaseMarkerGap => ActualWidth / TotalTime.TotalFrames;

    internal double Tolerance => AssociatedFile.DoubleTolerance.AsDouble();


    private double _frameMarkerGap;

    internal double FrameMarkerGap {
        get {
            if (_frameMarkerGap == 0) _frameMarkerGap = BaseMarkerGap;
            return _frameMarkerGap;
        }
        set {
            if (value < BaseMarkerGap) return;
            if (Math.Abs(value - _frameMarkerGap) < 1 + Tolerance) return;

            // May be SLOW.
            Clear(); 
            DrawTimeMarkers();

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

        for (uint i = 0; i < TotalTime.TotalFrames; i++) {
            --framesUntilSecond;
            --framesUntilMinute;
            --framesUntilHour;

            if (i == 0) continue;

            var x = TimelineTransform[BaseMarkerGap * i];

            void DrawMarker(TimeDividers divider, uint frame)
            {
                var line = new Line { X1 = 0, Y1 = 0, X2 = 0, Y2 = TimelineHeight };
                Canvas.SetLeft(line, x); Panel.SetZIndex(line, (int)divider);
                (line.Stroke, line.StrokeThickness) = TimelineColorTheme[divider];
                Add(frame, divider, line);
            }

            if (framesUntilHour == 0) {
                DrawMarker(TimeDividers.Hours, i);
                framesUntilHour = framesPerHour;
            }
            else if (framesUntilMinute == 0) {
                DrawMarker(TimeDividers.Minutes, i);
                framesUntilMinute = framesPerMinute;
            }
            else if (framesUntilSecond == 0) {
                DrawMarker(TimeDividers.Seconds, i);
                framesUntilSecond = FramesPerSecond;
            }
            else
                DrawMarker(TimeDividers.Frames, i);
        }
    }

    internal void Add(uint frame, TimeDividers divider, Line line)
    {
        MarkerData.Add(frame, divider);
        FrameMarkers.Add(frame, line);
        TimelineCanvas.Children.Add(line);
    }

    internal void Clear()
    {
        FrameMarkers.Values.ToList()
            .ForEach(TimelineCanvas.Children.Remove);
        FrameMarkers.Clear();
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

    private uint GetFrame(TimeSpan timespan) => (uint)(timespan.TotalSeconds * FramesPerSecond);
}