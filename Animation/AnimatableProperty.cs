using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace MathAnim.Animation;

internal class AnimatableProperty<T>
{
    // DO NOT REMOVE THIS. We aren't using null to allow users
    // to revert to their existing animation for the property.
    internal bool IsAnimatable { get; set; } = false;
    public T Value { get; set; }
    internal List<Keyframe<T>> Keyframes { get; } = new();
    protected internal virtual Animation<T>? Animation { get; protected set; }

    public AnimatableProperty(T value) => Value = value;

    public static implicit operator T(AnimatableProperty<T> property) => property.Value;
    public static implicit operator AnimatableProperty<T>(T t) => new(t);
}

/// <summary>
/// Animation over time. Input must be between 0 and 1.
/// </summary>
/// <typeparam name="T"></typeparam>
/// <param name="f">This is a percentage of the total number of frames. 
/// This opens up for easier modification when and if the user changes the number of frames per second.
/// This <b>must</b> be 0m at start of frame, and 1.0m at end of frame.
/// </param>
/// <returns></returns>
internal delegate T Animation<out T>(decimal f);