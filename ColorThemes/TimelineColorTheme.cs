using System;
using Animperium.FileData;

namespace Animperium.ColorThemes;

public record TimelineColorTheme
(PenStrokeData Hours,
    PenStrokeData Minutes,
    PenStrokeData Seconds,
    PenStrokeData Frames)
{
    public PenStrokeData this[TimeDividers dividers]
        => dividers switch
        {
            TimeDividers.Frames => Frames,
            TimeDividers.Seconds => Seconds,
            TimeDividers.Minutes => Minutes,
            TimeDividers.Hours => Hours,
            _ => throw new ArgumentOutOfRangeException(nameof(dividers), dividers, null)
        };
}