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

namespace MathAnim.Controls
{

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

        private byte _framesPerSecond = 24;
        public byte FramesPerSecond
        {
            get => _framesPerSecond;
            set
            {
                if (_framesPerSecond == value) return;

                _timeMarkers[TimeDividers.Frames].Clear();
                // TODO: Modify the lines.
                //_timeMarkers[TimeDividers.Frames].Add();

                _framesPerSecond = value;
            }
        }

        private double MinimumFrameMarkerGap => ActualWidth / FramesPerSecond;

        private AnimationTime _totalTime;

        public AnimationTime TotalTime
        {
            get => _totalTime;
            set
            {
                if (value < _totalTime) { }

                _totalTime = value;
            }
        }


        public uint TotalFrames => FramesPerSecond * TotalSeconds;

        private uint _totalSeconds;

        public uint TotalSeconds
        {
            get => _totalSeconds;
            set
            {
                if (_totalSeconds == value) return;

                // TODO: Draw additional markers if more, remove existing ones if less.

                _totalSeconds = value;
            }
        }


        public TimeSpan CurrentTime => TimeSpan.FromSeconds((double)CurrentFrame / FramesPerSecond);

        private Dictionary<TimeDividers, List<Line>> _timeMarkers = new();

        internal Dictionary<TimeDividers, (Brush brush, double thickness)> TimeMarkerData { get; }
            = new(new Dictionary<TimeDividers, (Brush brush, double thickness)>()
        {
            { TimeDividers.Frames, (Brushes.GhostWhite, 0.2d) },
            { TimeDividers.Seconds, (Brushes.LightGray, 0.5d) },
            { TimeDividers.Minutes, (Brushes.DarkGray, 0.8d) },
            { TimeDividers.Hours, (Brushes.Gray, 1d) }
        });

        private static readonly double TimelineHeight = 32;

        public AnimationCanvas()
        {
            InitializeComponent();
            SizeChanged += OnSizeChanged;
            CurrentValues.CurrentFile ??= new MathAnimFile();

            AssociatedFile = CurrentValues.CurrentFile;
            AssociatedFile.FramesPerSecondChanged += (_, b) => FramesPerSecond = b;
            AssociatedFile.TotalSecondsChanged += (_, t) => TotalSeconds = t;

            DrawTimeMarkers();;
        }

        private void DrawTimeMarkers()
        {
            // DEBUG: Needs testing.
            var framesPerMinute = FramesPerSecond * 60;
            var framesPerHour = framesPerMinute * 60;

            var framesUntilSecond = FramesPerSecond;
            var framesUntilMinute = FramesPerSecond * 60;
            var framesUntilHour = FramesPerSecond * 3600;

            // TODO: Add the markers.
            for (var i = 0; i < TotalFrames; i++)
            {
                --framesUntilSecond;
                --framesUntilMinute;
                --framesUntilHour;

                if (i == 0) continue;

                var x = MinimumFrameMarkerGap * i;
                
                if (framesUntilHour == 0)
                {
                    DrawMarker(x, TimeDividers.Hours);
                    framesUntilHour = framesPerHour;
                }
                else if (framesUntilMinute == 0)
                {
                    DrawMarker(x, TimeDividers.Minutes); 
                    framesUntilMinute = framesPerMinute;
                }
                else if (framesUntilSecond == 0)
                {
                    DrawMarker(x, TimeDividers.Seconds);
                    framesUntilSecond = FramesPerSecond;
                }
                else DrawMarker(x, TimeDividers.Frames);
            }
        }

        private void DrawMarker(double x, TimeDividers divider)
        {
            // Initiate line and add to canvas, keeping private variables updated.
            var line = new Line { X1 = 0, Y1 = 0, X2 = 0, Y2 = TimelineHeight };
            Canvas.SetLeft(line, x);
            (line.Stroke, line.StrokeThickness) = TimeMarkerData[divider];
            _timeMarkers[divider].Add(line);
            TimelineCanvas.Children.Add(line);
        }

        protected void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs)
        {
            // TODO: Update the timeline
        }

        /// <summary>
        /// Hides time dividers marked 0, and shows the ones marked 1.
        /// </summary>
        public void ShowTimeMarkers(TimeDividers timeDividers)
        {
            bool HasFlag(TimeDividers divider) => timeDividers.HasFlag(divider);

            void UpdateVisibility(bool hasFlag, TimeDividers divider) =>
                _timeMarkers[divider]
                    .ForEach(l => l.Visibility = hasFlag ? Visibility.Visible : Visibility.Hidden);

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

        internal void MoveTimeline(TimeSpan time)
        {
            // TODO
            // Remember to u
        }

        internal void MoveTimeline(int frames)
        {
            // TODO
        }

        internal void ScaleTimeline(double scale)
        {
            // TODO
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
