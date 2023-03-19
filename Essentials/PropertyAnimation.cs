using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Animperium.Essentials;

internal interface IPropertyKeyframeAnimation<in TAnimType>
{
    void SetValue(TAnimType value);
}

internal abstract record PropertyAnimation(Shape Shape, DependencyProperty Property, AnimationTimeline Animation)
{
    Type PropertyType => Property.PropertyType;

    void SetValue(object value) => Shape.SetValue(Property, value);

    // AddKeyframe
    

    // AnimationTimeline: Should also be an 
}

internal abstract record PropertyAnimation<TAnimType>(Shape Shape, DependencyProperty Property) : PropertyAnimation(Shape, Property)
{

}

// Implement cases, start with double
