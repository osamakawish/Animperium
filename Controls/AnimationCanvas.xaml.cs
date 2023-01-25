using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MathAnim.FileData;
using MathAnim.Settings;

namespace MathAnim.Controls;


public record struct TimeMarker(TimeDividers Divider, Line Line);

/// <summary>
/// Interaction logic for AnimationCanvas.xaml
/// </summary>
public partial class AnimationCanvas
{
    public required TimeMarkerData MarkerData { get; init; }
    public required TimeMarkerGraphicsData MarkerGraphicsData { get; init; }

    public enum TimelineLocation : byte
    {
        HasStart = 1,
        HasCurrentFrame = 1 << 1,
        HasEnd = 1 << 2,
        FullTimeline = HasStart | HasCurrentFrame | HasEnd
    }

    public TimelineLocation TimelinePosition { get; private set; } = TimelineLocation.FullTimeline;
    internal MathAnimFile AssociatedFile { get; set; }
    public uint CurrentFrame { get; internal set; }

    private byte _framesPerSecond = FileSettings.Default.AnimationTime.FramesPerSecond;
    public byte FramesPerSecond
    {
        get => _framesPerSecond;
        set
        {
            if (_framesPerSecond == value) return;

            var timeMarkers = MarkerGraphicsData.FrameMarkers;

            void RemoveMarkers(IEnumerable<Line> lines) => lines.ToList().ForEach(TimelineCanvas.Children.Remove);
            RemoveMarkers(timeMarkers.Select(x => x.Value));

            MarkerData.Clear(); MarkerGraphicsData.Clear();

            MarkerData.TotalTime = MarkerData.TotalTime with { FramesPerSecond = value };
            _framesPerSecond = value;

            MarkerGraphicsData.DrawTimeMarkers();
        }
    }

    private double MinimumFrameMarkerGap => ActualWidth / MarkerData.TotalTime.TotalFrames;

    private double _frameMarkerGap;
    private double FrameMarkerGap
    {
        get => _frameMarkerGap;
        set
        {
            if (value < MinimumFrameMarkerGap) return;
            if (Math.Abs(value - _frameMarkerGap) < 0.01) return;

            // TODO: Update Frame Marker Locations

            _frameMarkerGap = value;
        }
    }

    public TimeSpan CurrentTime => TimeSpan.FromSeconds((double)CurrentFrame / FramesPerSecond);
    

    internal static readonly double TimelineHeight = 32;

    public AnimationCanvas()
    {
        InitializeComponent();
        SizeChanged += OnSizeChanged;
        CurrentValues.CurrentFile ??= new MathAnimFile();

        MarkerData = new TimeMarkerData { TotalTime = FileSettings.Default.AnimationTime };
        MarkerGraphicsData = new TimeMarkerGraphicsData { AnimationCanvas = this };

        AssociatedFile = CurrentValues.CurrentFile;
        AssociatedFile.FramesPerSecondChanged += (_, b) => FramesPerSecond = b;
        AssociatedFile.TotalTimeChanged += (_, t) => MarkerData.TotalTime = t;

        _frameMarkerGap = MinimumFrameMarkerGap;
        Loaded += (_, _) => MarkerGraphicsData.DrawTimeMarkers();
    }

    private void UpdateTimelineLocation()
    {
        // TODO
    }

    // Better to have a keyframe as input instead.
}

public enum TimeDividers : byte
{
    None = 0,
    Frames = 1,
    Seconds = 1 << 1,
    Minutes = 1 << 2,
    Hours = 1 << 3,
    All = 0b1111
}