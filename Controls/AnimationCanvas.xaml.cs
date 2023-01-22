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

namespace MathAnim.Controls
{
    using TimeMarkersDictionary = Dictionary<TimeDividers, List<TimeMarker>>;

    internal record struct TimeMarker(uint Frame, Line Line);

    /// <summary>
    /// Interaction logic for AnimationCanvas.xaml
    /// </summary>
    public partial class AnimationCanvas
    {
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

        private byte _framesPerSecond = FileSettings.Default.FramesPerSecond;
        public byte FramesPerSecond
        {
            get => _framesPerSecond;
            set
            {
                if (_framesPerSecond == value) return;

                TimeMarkers[TimeDividers.Frames].Clear();
                // TODO: Modify the lines.
                //_timeMarkers[TimeDividers.Frames].Add();

                _framesPerSecond = value;
            }
        }

        private double MinimumFrameMarkerGap => ActualWidth / TotalFrames;

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

        private AnimationTime _totalTime = FileSettings.Default.TotalTime;
        public AnimationTime TotalTime
        {
            get => _totalTime;
            set
            {
                if (_totalTime == value) return;
                var totalFrames = value.TotalFrames(FramesPerSecond);

                if (value < _totalTime)
                {
                    // SLOW : All frames are tested by this code.
                    void RemoveSurplusMarkers(params TimeDividers[] dividers)
                        => dividers.ToList().ForEach(d => TimeMarkers[d].RemoveAll(x => x.Frame > totalFrames));

                    RemoveSurplusMarkers(
                        TimeDividers.Frames,
                        TimeDividers.Seconds,
                        TimeDividers.Minutes,
                        TimeDividers.Hours);
                }
                else if (value > _totalTime)
                {
                    // TODO
                }

                _totalTime = value;
            }
        }

        public uint TotalFrames => TotalTime.TotalFrames(FramesPerSecond);
        public uint TotalSeconds => TotalTime.TotalSeconds;

        public TimeSpan CurrentTime => TimeSpan.FromSeconds((double)CurrentFrame / FramesPerSecond);

        private TimeMarkersDictionary TimeMarkers { get; } = new();

        private static SolidColorBrush Brush(byte r, byte g, byte b, byte a = 255) => new(Color.FromArgb(a, r, g, b));
        internal Dictionary<TimeDividers, (Brush brush, double thickness)> TimeMarkerData { get; }
            = new(new Dictionary<TimeDividers, (Brush brush, double thickness)>()
        {
            { TimeDividers.Frames, (Brush(0xE8, 0xE8, 0xE8), 1d) },
            { TimeDividers.Seconds, (Brush(0xC0, 0xC0, 0xC0), 1.2d) },
            { TimeDividers.Minutes, (Brush(0xA0, 0xA0, 0xA0), 1.4d) },
            { TimeDividers.Hours, (Brushes.Gray, 1.6d) }
        });

        private static readonly double TimelineHeight = 32;

        public AnimationCanvas()
        {
            InitializeComponent();
            SizeChanged += OnSizeChanged;
            CurrentValues.CurrentFile ??= new MathAnimFile();

            AssociatedFile = CurrentValues.CurrentFile;
            AssociatedFile.FramesPerSecondChanged += (_, b) => FramesPerSecond = b;
            AssociatedFile.TotalTimeChanged += (_, t) => TotalTime = t;

            _frameMarkerGap = MinimumFrameMarkerGap;
            Loaded += (_, _) => DrawTimeMarkers();
        }

        /// <summary>
        /// Renders all the time markers on the animation timeline. 
        /// </summary>
        private void DrawTimeMarkers()
        {
            var framesPerMinute = FramesPerSecond * 60;
            var framesPerHour = framesPerMinute * 60;

            var framesUntilSecond = FramesPerSecond;
            var framesUntilMinute = framesPerMinute;
            var framesUntilHour = framesPerHour;

            for (uint i = 0; i < TotalFrames; i++)
            {
                --framesUntilSecond;
                --framesUntilMinute;
                --framesUntilHour;

                if (i == 0) continue;

                var x = FrameMarkerGap * i;

                if (framesUntilHour == 0)
                {
                    DrawMarker(TimeDividers.Hours, x, i);
                    framesUntilHour = framesPerHour;
                }
                else if (framesUntilMinute == 0)
                {
                    DrawMarker(TimeDividers.Minutes, x, i); 
                    framesUntilMinute = framesPerMinute;
                }
                else if (framesUntilSecond == 0)
                {
                    DrawMarker(TimeDividers.Seconds, x, i);
                    framesUntilSecond = FramesPerSecond;
                }
                else DrawMarker(TimeDividers.Frames, x, i);
            }
        }

        private void DrawMarker(TimeDividers divider, double x, uint i)
        {
            var line = new Line { X1 = 0, Y1 = 0, X2 = 0, Y2 = TimelineHeight };
            Canvas.SetLeft(line, x);
            (line.Stroke, line.StrokeThickness) = TimeMarkerData[divider];
            Panel.SetZIndex(line, (int)divider);

            if (TimeMarkers.TryGetValue(divider, out var lines))
                lines.Add(new TimeMarker(i, line));
            else TimeMarkers.Add(divider, new List<TimeMarker>());

            TimelineCanvas.Children.Add(line);
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            // TODO: Update the timeline
        }

        private void UpdateTimelineLocation()
        {
            // TODO
        }

        /// <summary>
        /// Hides time dividers marked 0, and shows the ones marked 1.
        /// </summary>
        public void ShowTimeMarkers(TimeDividers timeDividers)
        {
            bool HasFlag(TimeDividers divider) => timeDividers.HasFlag(divider);

            void UpdateVisibility(bool hasFlag, TimeDividers divider) =>
                TimeMarkers[divider]
                    .ForEach(l => l.Line.Visibility = hasFlag ? Visibility.Visible : Visibility.Hidden);

            void UpdateVisibilities(params TimeDividers[] dividers)
                => dividers.ToList().ForEach(d => UpdateVisibility(HasFlag(d), d));

            UpdateVisibilities(TimeDividers.Frames, TimeDividers.Seconds, TimeDividers.Minutes, TimeDividers.Hours);
        }

        /// <summary>
        /// Removes the keyframes from the canvas.
        /// </summary>
        internal void ClearKeyframes()
        {
            // TODO
        }

        internal void MoveTimeline(TimeSpan time, bool isForward=true)
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

        internal void AddKeyframe(TimeSpan time)
        {
            // TODO
        }

        // Better to have a keyframe as input instead.
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

        internal void GoToPreviousFrame()
        {
            // TODO
        }

        internal void GoToNextFrame()
        {
            // TODO
        }

        internal void Play()
        {
            // TODO
        }

        internal void Pause()
        {
            // TODO
        }

        internal void Stop()
        {
            // TODO
        }
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
}
