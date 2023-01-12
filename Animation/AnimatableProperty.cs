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
        internal bool IsAnimatable { get; set; } = false;
        public T Value { get; set; }
        internal List<Keyframe<T>> Keyframes { get; } = new();
        virtual protected internal Animation<T>? Animation { get; protected set; }

        public AnimatableProperty(T value) => Value = value;

        public static implicit operator T(AnimatableProperty<T> property) => property.Value;
        public static implicit operator AnimatableProperty<T>(T t) => new(t);
    }

    class Keyframe<T>
    {
        Keyframe<T>? prev;
        Keyframe<T>? next;

        internal T Value { get; set; }

        /// <summary>
        /// The next keyframe.
        /// </summary>
        /// <remarks>
        /// Only need to assign <b>either</b> this or the previous keyframe's <see cref="Next"/> property.
        /// The other is updated simultaneously.
        /// </remarks>
        internal Keyframe<T>? Prev
        {
            get => prev;
            set
            {
                if (value is not null) value.next = this;
                prev = value;
            }
        }

        /// <summary>
        /// The next keyframe.
        /// </summary>
        /// <remarks>
        /// Only need to assign <b>either</b> this or the next keyframe's <see cref="Prev"/> property.
        /// The other is updated simultaneously.
        /// </remarks>
        internal Keyframe<T>? Next
        {
            get => next;
            set
            {
                if (value is not null) value.prev = this;
                next = value;
            }
        }

        public Keyframe(T value) => Value = value;
    }

    /// <summary>
    /// Animation over time. Input must be between 0 and 1.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="f">This is a percentange of the total number of frames. 
    /// This opens up for easier modification when and if the user changes the number of frames per second.</param>
    /// <returns></returns>
    delegate T Animation<T>(decimal f);
}
