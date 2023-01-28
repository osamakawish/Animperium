using System;

namespace MathAnim.FileData;

public record struct AnimationTime
    (byte Hours,
    byte Minutes,
    byte Seconds,
    byte Frames,
    byte FramesPerSecond = 24)
{
    public uint FramesPerMinute => FramesPerSecond * 60u;
    public uint FramesPerHour => FramesPerSecond * 3600u;

    public bool HasLegalInputs
        => Minutes < 60 && Seconds < 60 && Frames < FramesPerSecond;

    /// <summary>
    /// Total number of frames, not including the starting/zero frame.
    /// </summary>
    public uint TotalFrames
        => Frames + FramesPerSecond * (Seconds + 60u * (Minutes + 60u * Hours));

    public uint TotalSeconds => (uint)(Frames == 0 ? 0 : 1) + Seconds + 60 * (Minutes + 60 * (uint)Hours);

    public static bool operator <(AnimationTime a, AnimationTime b)
        => a.TotalSeconds == b.TotalSeconds ? a.Frames < b.Frames : a.TotalSeconds < b.TotalSeconds;

    public static bool operator >(AnimationTime a, AnimationTime b) => b < a;

    /// <summary>
    /// </summary>
    /// <param name="timeLimit">The total time.</param>
    /// <returns>True if the inputs of this record are legal (<see cref="Frames"/> &lt; <see cref="FramesPerSecond"/>,
    /// <see cref="Seconds"/> &lt; 60, etc.) and is within the provided time limit, false otherwise.</returns>
    public bool IsValid(AnimationTime timeLimit)
        => TotalFrames <= timeLimit.TotalFrames && HasLegalInputs;

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public AnimationTime LegalForm
    {
        get
        {
            if (HasLegalInputs) return this;

            var totalFrames = TotalFrames;
            var totalSeconds = totalFrames / FramesPerSecond;
            var timeSpan = TimeSpan.FromSeconds(totalSeconds);

            return new AnimationTime((byte)timeSpan.Hours,
                (byte)timeSpan.Minutes,
                (byte)(totalSeconds % 60),
                (byte)(totalFrames % FramesPerSecond));
        }
    }

    public override string ToString() 
        => $"AnimationTime[{FramesPerSecond}]({Hours}: {Minutes}: {Seconds}; {Frames})" +
           $"{(HasLegalInputs ? string.Empty : "?")}";
}