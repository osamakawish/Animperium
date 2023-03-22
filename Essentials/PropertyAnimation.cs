using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Animperium.Essentials;

internal record PropertyAnimation(Shape Shape, DependencyProperty Property, AnimationTimeline Animation)
{
    internal IKeyFrame? SelectedKeyframe { get; private set; }

    internal IEnumerable<IKeyFrame> Keyframes => ((IKeyFrameAnimation)Animation).KeyFrames.OfType<IKeyFrame>();

    internal Type PropertyType => Property.PropertyType;

    internal void SetValue(object value) => Shape.SetValue(Property, value);

    internal void Select(IKeyFrame keyframe)
    { if (Keyframes.Contains(keyframe)) SelectedKeyframe = keyframe; }

    internal void Deselect() => SelectedKeyframe = null;
}
