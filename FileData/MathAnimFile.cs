using MathAnim.Graphics;
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
        internal EventHandler<AnimationTime>? TotalTimeChanged;
        internal EventHandler<byte>? FramesPerSecondChanged;

        private byte _framesPerSecond = ValueTemplate.Default.FramesPerSecond;
        private AnimationTime _totalTime = ValueTemplate.Default.TotalTime;

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
                if (!value.HasLegalInputs(FramesPerSecond)) return;
                if (_totalTime == value) return;
                _totalTime = value;
                TotalTimeChanged?.Invoke(this, value);
            }
        }

        internal IGraphicsObject? SelectedObject { get; set; }
        internal GraphicsObjectTree GraphicsObjectTree { get; } = new();
    }
}
