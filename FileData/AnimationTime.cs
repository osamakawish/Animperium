namespace MathAnim.FileData;

public record struct AnimationTime(byte Hours, byte Minutes, byte Seconds, byte Frames)
{
    public bool HasLegalInputs(byte framesPerSecond=24)
        => Minutes < 60 && Seconds < 60 && Frames < framesPerSecond;

    public uint TotalFrames(byte framesPerSecond = 24)
        => Frames + framesPerSecond * TotalSeconds;

    public uint TotalSeconds => (uint)(Frames == 0 ? 0 : 1) + Seconds + 60 * (Minutes + 60 * (uint)Hours);

    public static bool operator <(AnimationTime a, AnimationTime b)
        => a.TotalSeconds == b.TotalSeconds ? a.Frames < b.Frames : a.TotalSeconds < b.TotalSeconds;

    public static bool operator >(AnimationTime a, AnimationTime b) => b < a;

    /// <summary>
    /// Returns true if the inputs of this record are legal (<see cref="Frames"/> &lt; <see cref="framesPerSecond"/>
    /// , <see cref="Seconds"/> &lt; 60, etc.) and is within the provided time limit.
    /// </summary>
    /// <param name="timeLimit">The total time.</param>
    /// <param name="framesPerSecond">The FPS, or the number of frames per second in a given second.</param>
    /// <returns></returns>
    public bool IsValid(AnimationTime timeLimit, byte framesPerSecond = 24)
        => TotalFrames(framesPerSecond) <= timeLimit.TotalFrames(framesPerSecond) && HasLegalInputs(framesPerSecond);
}