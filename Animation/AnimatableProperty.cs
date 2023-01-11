using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MathAnim.Animation
{
    class AnimatableProperty<T>
    {
        // DO NOT REMOVE THIS. We aren't using null to allow users
        // to revert to their existing animation for the property.
        bool IsAnimatable { get; set; } = false;
        public T Value { get; set; }
        List<Keyframe<T>> Keyframes { get; } = new();
        Animation<T>? Animation { get; set; }

        public AnimatableProperty(T value) => Value = value;
    }

    class Keyframe<T>
    {
        T Value { get; set; }
        Keyframe<T>? Prev { get; set; }
        Keyframe<T>? Next { get; set; }

        public Keyframe(T value) => Value = value;
    }

    /// <summary>
    /// Animation over time, determined by the first point and last point, such that first animation
    /// is <see cref="Animation{T}"/>(0) and
    /// the last is at <see cref="Animation{T}"/>(<see cref="uint.MaxValue"/>).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="f">It's a percentange of <see cref="uint.MaxValue"/>. This allows for easier modification when 
    /// the frames per second is changed, without sacrificing memory, at the cost of slight overhead 
    /// and additional error testing.</param>
    /// <returns></returns>
    delegate T Animation<T>(uint f);
}
