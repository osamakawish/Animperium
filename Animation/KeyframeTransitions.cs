using System;
using System.Numerics;

namespace MathAnim.Animation;

internal static class KeyframeTransitions
{
    internal static KeyframeTransition<T> Transition<T>(Keyframe<T> start, Animation<T> transition)
        => start.Next is not null ? (new KeyframeTransition<T>(start, transition)) : throw new NullReferenceException();

    internal static KeyframeTransition<T> ConstantTransition<T>(Keyframe<T> start)
        where T : INumber<T>
        => start.Next is not null
            ? (new KeyframeTransition<T>(start, f => f != 1 ? start.Value : start.Next.Value))
            : throw new NullReferenceException();

    internal static KeyframeTransition<float> FloatLinearTransition(Keyframe<float> start)
        => start.Next is not null
            ? (new KeyframeTransition<float>(start, f =>
            {
                var _f = (float)f;
                return (1 - _f) * start.Value + _f * start.Next.Value;
            }))
            : throw new NullReferenceException();

    internal static KeyframeTransition<double> DoubleLinearTransition(Keyframe<double> start)
        => start.Next is not null
            ? (new KeyframeTransition<double>(start, f =>
            {
                var _f = (double)f;
                return (1 - _f) * start.Value + _f * start.Next.Value;
            }))
            : throw new NullReferenceException();

    internal static KeyframeTransition<decimal> DecimalLinearTransition(Keyframe<decimal> start)
        => start.Next is not null
            ? (new KeyframeTransition<decimal>(start, f => (1 - f) * start.Value + f * start.Next.Value))
            : throw new NullReferenceException();
}