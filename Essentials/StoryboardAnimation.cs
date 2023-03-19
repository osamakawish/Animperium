using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace Animperium.Essentials;

using PropertyAnimationDictionary
    = Dictionary<Shape,
        Dictionary<DependencyProperty,
            AnimationTimeline>>;

/// <summary>
/// Handles the properties of items on a canvas that are being animated.
/// </summary>
internal class StoryboardAnimation : ICollection<PropertyAnimation>
{
    private readonly PropertyAnimationDictionary _dictionary = new();
    private readonly HashSet<PropertyAnimation> _set = new();

    /// <summary>
    /// The storyboard for this animation. Do not modify directly, but use this
    /// <see cref="StoryboardAnimation"/> to modify the animation instead, as it keeps track of the information
    /// passed into it.
    /// </summary>
    internal Storyboard Storyboard { get; } = new();

    public IEnumerator<PropertyAnimation> GetEnumerator()
        => _dictionary.SelectMany(x => x.Value
            .Select(y => new PropertyAnimation(x.Key, y.Key, y.Value)))
        .GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void Add(Shape shape, DependencyProperty property, AnimationTimeline animation)
    {
        if (!_dictionary.ContainsKey(shape))
            _dictionary.Add(shape, new Dictionary<DependencyProperty, AnimationTimeline>());

        var shapeAnimations = _dictionary[shape];
        if (shapeAnimations.ContainsKey(property))
            // Rely on record equality to equate references to remove existing property animation from set.
            _set.Remove(new PropertyAnimation(shape, property, shapeAnimations[property]));

        shapeAnimations.Add(property, animation);
        _set.Add(new PropertyAnimation(shape, property, animation));

        Storyboard.Children.Add(animation);
        Storyboard.SetTarget(animation, shape);
        Storyboard.SetTargetProperty(animation, new PropertyPath(property));
    }

    public void Add(PropertyAnimation item) => Add(item.Shape, item.Property, item.Animation);

    public void Clear() { _dictionary.Clear(); _set.Clear(); Storyboard.Children.Clear(); }

    public bool Contains(PropertyAnimation item)
        => _dictionary.ContainsKey(item.Shape)
           && _dictionary[item.Shape].TryGetValue(item.Property, out var value)
           && item.Animation == value;

    public void CopyTo(PropertyAnimation[] array, int arrayIndex)
    {
        foreach (var property in this)
            if (arrayIndex <= array.Length) array[arrayIndex++] = property;
    }

    public bool Remove(PropertyAnimation item)
    {
        if (!_set.Contains(item)) return false;

        var animationTimelines = _dictionary[item.Shape];
        animationTimelines.Remove(item.Property);
        if (animationTimelines.Count == 0) _dictionary.Remove(item.Shape);

        _set.Remove(item);

        Storyboard.Children.Remove(item.Animation);

        return true;
    }

    public int Count => _set.Count;

    public void ForEach(Action<PropertyAnimation> action)
    { foreach (var animationProperty in _set) action(animationProperty); }

    public void ForEach(Action<Shape, DependencyProperty, AnimationTimeline> action)
    {
        foreach (var (shape, animationTimelines) in _dictionary)
        foreach (var (property, animation) in animationTimelines)
            action(shape, property, animation);
    }

    public bool IsReadOnly => false;

    public AnimationTimeline this[Shape shape, DependencyProperty property] {
        get => _dictionary[shape][property];
        set => Add(shape, property, value);
    }
}