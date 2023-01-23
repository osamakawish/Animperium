using MathAnim.Graphics;
using MathAnim.Settings;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathAnim.FileData
{
    internal class MathAnimFile
    {
        public const uint SecondsPerMinute = 60;
        public const uint SecondsPerHour = 60 * 60;
        //public static MathAnimFile FromSettings(FileSettings fileSettings)

        internal EventHandler<AnimationTime>? TotalTimeChanged;
        internal EventHandler<byte>? FramesPerSecondChanged;

        private byte _framesPerSecond = FileSettings.Default.AnimationTime.FramesPerSecond;
        private AnimationTime _totalTime = FileSettings.Default.AnimationTime;

        internal byte FramesPerSecond
        {
            get => _framesPerSecond;
            set
            {
                if (_framesPerSecond == value) return;
                _framesPerSecond = value;
                FramesPerSecondChanged?.Invoke(this, value);
            }
        }

        internal AnimationTime TotalTime
        {
            get => _totalTime;
            set
            {
                if (!value.HasLegalInputs) return;
                if (_totalTime == value) return;
                _totalTime = value;
                TotalTimeChanged?.Invoke(this, value);
            }
        }

        internal IGraphicsObject? SelectedObject { get; set; }
        internal GraphicsObjectTree GraphicsObjectTree { get; } = new();

        public MathAnimFile()
        {
                
        }

        public MathAnimFile(byte framesPerSecond, AnimationTime animationTime)
            { _framesPerSecond = framesPerSecond; _totalTime = animationTime; }
    }
}
