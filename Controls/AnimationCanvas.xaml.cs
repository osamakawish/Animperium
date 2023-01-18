using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

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
        public uint CurrentFrame { get; internal set; }
        public uint FramesPerSecond { get; internal set; }
        public TimeSpan CurrentTime => TimeSpan.FromSeconds((double)CurrentFrame / FramesPerSecond);

        public AnimationCanvas()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Hides time dividers marked 0, and shows the ones marked 1.
        /// </summary>
        /// <param name="dividers"></param>
        internal void DrawTimeMarkers(TimeDividers dividers)
        {
            // TODO
        }

        internal void ClearKeyframes()
        {
            // TODO
        }

        internal void MoveTimeline(TimeSpan time)
        {
            // TODO
            // Remember to u
        }

        internal void MoveTimeline(uint frame)
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
