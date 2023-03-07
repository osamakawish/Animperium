using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Animperium.Essentials;

using PropertyAnimationDictionary
    = Dictionary<Shape, Dictionary<DependencyProperty, AnimationTimeline>>;

internal record AnimationProperty(Shape Shape, DependencyProperty Property, AnimationTimeline Animation);

/// <summary>
/// Handles the properties of items on a canvas that are being animated.
/// </summary>
internal class AnimationProperties : ICollection<AnimationProperty>
{
    private readonly PropertyAnimationDictionary _dictionary = new();

    public IEnumerator<AnimationProperty> GetEnumerator()
        => _dictionary.SelectMany(x => x.Value
            .Select(y => new AnimationProperty(x.Key, y.Key, y.Value)))
        .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(Shape shape, DependencyProperty property, AnimationTimeline animation)
    {
        if (!_dictionary.ContainsKey(shape))
            _dictionary.Add(shape, new Dictionary<DependencyProperty, AnimationTimeline>());

        _dictionary[shape].Add(property, animation);
    }
    public void Add(AnimationProperty item) => Add(item.Shape, item.Property, item.Animation);

    public void Clear() => _dictionary.Clear();
    public bool Contains(AnimationProperty item) => false;

    public void CopyTo(AnimationProperty[] array, int arrayIndex)
    {
        foreach (var property in this)
            if (arrayIndex <= array.Length) array[arrayIndex++] = property;
    }
    public bool Remove(AnimationProperty item) => false;

    public int Count => _dictionary.Values.Sum(animation => animation.Count);

    public void ForEach(Action<AnimationProperty> action)
    { foreach (var property in this) action(property); }

    public bool IsReadOnly => false;
}