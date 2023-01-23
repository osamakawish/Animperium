using System.Collections.Generic;
namespace MathAnim.FileData;

using MathAnim.Controls;
using System.Windows.Shapes;

using TimeMarkersDictionary = Dictionary<uint, TimeMarker>;

internal record struct TimeMarker(TimeDividers Divider, Line Line);

public class TimeMarkerData
{

}