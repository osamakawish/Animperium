using System;

namespace MathAnim.Animation
{
    record Keyframe<T>(T Value, Keyframe<T>? Prev = null, Keyframe<T>? Next = null)
    {
        public static implicit operator T(Keyframe<T> kf) => kf.Value;
        public static implicit operator Keyframe<T>(T value) => new(value);
    }

    record KeyframeTransition<T>(Keyframe<T> Start, Animation<T> Transition)
    {
        internal Keyframe<T> End => Start.Next ?? throw new NullReferenceException();
    }
}
