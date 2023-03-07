using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Animperium.FileData;
using Animperium.Settings;

namespace Animperium.Controls.TimelinePropertyCanvas;

public record struct TimeMarker(TimeDividers Divider, Line Line);

/// <summary>
/// Interaction logic for AnimationCanvas.xaml
/// </summary>
public partial class TimelinePropertyCanvas
{
    // Handles purely computational component of the time markers.
    public required TimeMarkerData MarkerData { get; init; }

    internal AppFile AssociatedFile { get; set; }
    internal Storyboard CurrentStoryboard => AssociatedFile.CurrentStoryboard;

    private Path CurrentFrameLine { get; } = new() {
        StrokeThickness = 1.5,
        StrokeDashArray = new DoubleCollection(new double[] { 2, 3 }),
        Stroke = Brushes.Black,
        Data = new LineGeometry(new Point(0, 0), new Point(0, 800))
    };
    private uint _currentFrame;
    public uint CurrentFrame {
        get => _currentFrame;
        internal set {
            // Move current frame line.
            Canvas.SetLeft(CurrentFrameLine, GetLeft(value));

            // Move storyboard to given frame.
            //CurrentStoryboard.Begin();

            _currentFrame = value;
        }
    }

    private byte _framesPerSecond = FileSettings.Default.AnimationTime.FramesPerSecond;

    public byte FramesPerSecond {
        get => _framesPerSecond;
        set {
            if (_framesPerSecond == value) return;

            void RemoveMarkers(IEnumerable<Line> lines)
                => lines.ToList().ForEach(TimelineCanvas.Children.Remove);
            RemoveMarkers(FrameMarkers.Select(x => x.Value));

            MarkerData.Clear();
            Clear();

            MarkerData.TotalTime = MarkerData.TotalTime with { FramesPerSecond = value };
            _framesPerSecond = value;

            DrawTimeMarkers();
        }
    }

    public TimeSpan CurrentTime => TimeSpan.FromSeconds((double)CurrentFrame / FramesPerSecond);
    public TimelineLocation TimelinePosition { get; private set; } = TimelineLocation.FullTimeline;


    internal static readonly double TimelineHeight = 32;

    public TimelinePropertyCanvas()
    {
        InitializeComponent();
        SizeChanged += OnSizeChanged;
        CurrentValues.CurrentFile ??= new AppFile();

        MarkerData = new TimeMarkerData { TotalTime = FileSettings.Default.AnimationTime };

        AssociatedFile = CurrentValues.CurrentFile;
        AssociatedFile.FramesPerSecondChanged += (_, b) => FramesPerSecond = b;
        AssociatedFile.TotalTimeChanged += (_, t) => MarkerData.TotalTime = t;

        TimelineCanvas.Children.Add(CurrentFrameLine);
        Panel.SetZIndex(CurrentFrameLine, int.MaxValue - 1);

        Loaded += delegate
        {
            DrawTimeMarkers();
            AddKeyframe(KeyframeType.Constant, TimeSpan.FromSeconds(30));
            CurrentFrame = ToFrame(TimeSpan.FromSeconds(20));
        };
    }

    private uint ToFrame(TimeSpan timeSpan) => (uint)(timeSpan.TotalSeconds * FramesPerSecond);

    private void UpdateTimelineLocation()
    {
        // TODO
    }

    // Better to have a keyframe as input instead.
}