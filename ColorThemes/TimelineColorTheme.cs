using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace MathAnim.ColorThemes;

public record TimelineColorTheme
    (PenStrokeData Hours,
    PenStrokeData Minutes,
    PenStrokeData Seconds,
    PenStrokeData Frames);