using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using MathAnim.Controls;
using MathAnim.FileData;

namespace MathAnim.ColorThemes;

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