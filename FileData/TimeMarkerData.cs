using System.Collections.Generic;

namespace MathAnim.FileData;

using Controls;
using Settings;

/// <summary>
/// Handles the non-graphical computational component of the time markers.
/// </summary>
public class TimeMarkerData
{
    // Essentially only manipulates these properties.
    private AnimationTime _totalTime = FileSettings.Default.AnimationTime;
    public AnimationTime TotalTime
    {
        get => _totalTime;
        set
        {
            if (_totalTime == value) return;

            // Remove surplus markers if new value less than previous.
            if (value < _totalTime)
                for (var i = _totalTime.TotalFrames + 1; i <= value.TotalFrames; i++)
                { var div = FrameDividers[i]; FrameDividers.Remove(i); DividerFrames[div].Remove(i); }

            else if (value > _totalTime)
            {
                // TODO
            }

            _totalTime = value;
        }
    }

    public void Add(uint frame, TimeDividers divider)
    { FrameDividers.Add(frame, divider); DividerFrames[divider].Add(frame); }

    public void Clear() { FrameDividers.Clear(); DividerFrames.Clear(); }

    internal Dictionary<uint, TimeDividers> FrameDividers { get; } = new();
    internal Dictionary<TimeDividers, List<uint>> DividerFrames { get; } = new()
    {
        { TimeDividers.Frames, new List<uint>() },
        { TimeDividers.Seconds, new List<uint>() },
        { TimeDividers.Minutes, new List<uint>() },
        { TimeDividers.Hours, new List<uint>() }
    };
}