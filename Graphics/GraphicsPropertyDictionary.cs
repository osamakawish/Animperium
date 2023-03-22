using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;

namespace Animperium.Graphics;

internal static class GraphicsPropertyDictionary
{
    internal static Dictionary<Type, AnimationProperty[]> Properties = new() {
        { typeof(Shape), new[] {
            new AnimationProperty(Canvas.LeftProperty, "X"), // Need to set an AnimationCanvas.X property
            new AnimationProperty(Canvas.TopProperty, "Y"),  // Need to set an AnimationCanvas.Y property
            new AnimationProperty(FrameworkElement.WidthProperty),
            new AnimationProperty(FrameworkElement.HeightProperty),
            new AnimationProperty(Shape.StrokeProperty),
            new AnimationProperty(Shape.FillProperty),
            new AnimationProperty(Shape.StrokeThicknessProperty)
        } }
    };

    // Even better would be to use a dictionary with the associated types, giving a grouped set of elements.
    internal static AnimationProperty[] GetProperties(Type type)
    {
        List<AnimationProperty> properties = new();

        foreach (var (t, animationProperties) in Properties)
            if (type.IsSubclassOf(t)) properties.AddRange(animationProperties);

        return properties.ToArray();
    }
}

internal record struct AnimationProperty(DependencyProperty Property, string Name = "")
{
    internal string Name = string.IsNullOrWhiteSpace(Name) ? Property.Name : Name;
}