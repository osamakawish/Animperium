using System.Collections.Generic;

namespace MathAnim.FileData;

using Controls;

internal class TimeMarkerData
{
    // Essentially only manipulates these properties.
    internal required AnimationTime TotalTime
    {
        get;
        set;
    }
    private Dictionary<TimeDividers, uint> TimeMarkers { get; } = new();
    private FrameDividersDictionary FrameDividers { get; } = new()
    {
        { TimeDividers.Frames, new List<uint>() },
        { TimeDividers.Seconds, new List<uint>() },
        { TimeDividers.Minutes, new List<uint>() },
        { TimeDividers.Hours, new List<uint>() }
    };
}