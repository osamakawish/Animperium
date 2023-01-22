using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathAnim.Settings;

namespace MathAnim.FileData
{
    using DoubleMapping = Mapping<double>;

    public record struct Mapping<T>(T From, T To);

    public record Transform1D(double Scale, double Shift, double Origin = 0)
    {
        public static Transform1D InvalidTransform => new(double.NaN, double.NaN, double.NaN);
        public static Transform1D Identity => new(1, 0);

        public static Transform1D FromMapping(
            DoubleMapping map1,
            DoubleMapping map2,
            DoubleTolerance tolerance = DoubleTolerance.Medium)
        {
            var deltaFrom = map1.From - map2.From;
            var deltaTo = map1.To - map2.To;
            if (Math.Abs(deltaFrom) < tolerance.AsDouble()) return InvalidTransform;
            var slope = deltaTo / deltaFrom;

            return new Transform1D(slope, map1.To - slope * map1.From);
        }

        public double this[double x] => Scale * (x - Origin) + Shift;

        public IEnumerable<double> this[IEnumerable<double> doubles] => doubles.Select(x => this[x]);

        public bool IsInvalid => double.IsNaN(Scale) || double.IsNaN(Shift) || double.IsNaN(Origin);

        public Transform1D Inverse => new(1 / Scale, Origin, Shift);

        public Transform1D ApplyShift(double s) => this with { Shift = Shift + s };

        public Transform1D ApplyScale(double s) => new(s * Scale, s * Shift, Origin);

        public Transform1D Compose(Transform1D t)
            => new(Scale * t.Scale, Scale * (t.Shift - Origin) + Shift, t.Origin);

        public Transform1D ApplyTransform(Transform1D t)
            => new(Scale * t.Scale, t.Scale * (Shift - t.Origin) + t.Shift, Origin);

        public Transform1D ToZeroOrigin => new(Scale, Shift - Scale * Origin);

        public bool IsSame(Transform1D t, DoubleTolerance tolerance)
            => Math.Abs(Scale - t.Scale) < tolerance.AsDouble() &&
               Math.Abs((Shift - Scale * Origin) - (t.Shift - t.Scale * t.Origin)) < tolerance.AsDouble();
    }
}
