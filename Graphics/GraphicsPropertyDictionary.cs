using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Shapes;
using Animperium.Controls;

namespace Animperium.Graphics;

internal static class GraphicsPropertyDictionary
{
    internal static Dictionary<Type, AnimationProperty[]> Properties = new() {
        { typeof(Shape), new[] {
            new AnimationProperty(AnimationCanvas.XProperty),
            new AnimationProperty(AnimationCanvas.YProperty),
            new AnimationProperty(AnimationCanvas.ItemWidthProperty,  "Width"),
            new AnimationProperty(AnimationCanvas.ItemHeightProperty, "Height"),
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