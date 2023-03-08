using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Animperium.Essentials;
using Animperium.Graphics;
using Animperium.Settings;

namespace Animperium.FileData;

internal class AppFile
{
    public const uint SecondsPerMinute = 60;
    public const uint SecondsPerHour = 60 * 60;

    internal FileSettings Settings = FileSettings.Default;

    internal EventHandler<AnimationTime>? TotalTimeChanged;
    internal EventHandler<byte>? FramesPerSecondChanged;
    
    private AnimationTime _totalTime;

    internal DoubleTolerance DoubleTolerance = FileSettings.Default.Tolerance;

    private readonly List<AnimationProperties> _animationProperties;
    private int _currentStoryboardIndex;

    internal ReadOnlyCollection<AnimationProperties> AnimationProperties => _animationProperties.AsReadOnly();
    internal ReadOnlyCollection<Storyboard> Storyboards
        => _animationProperties.Select(x => x.Storyboard).ToList().AsReadOnly();

    internal Storyboard CurrentStoryboard => AnimationProperties[_currentStoryboardIndex].Storyboard;
    internal Storyboard GoToNextStoryboard()
        => AnimationProperties[++_currentStoryboardIndex % AnimationProperties.Count].Storyboard;
    internal Storyboard GoToPreviousStoryboard()
        => AnimationProperties[--_currentStoryboardIndex % AnimationProperties.Count].Storyboard;

    internal byte FramesPerSecond
    {
        get => _totalTime.FramesPerSecond;
        set
        {
            if (_totalTime.FramesPerSecond == value) return;
            _totalTime.FramesPerSecond = value;
            FramesPerSecondChanged?.Invoke(this, value);
        }
    }

    internal AnimationTime TotalTime
    {
        get => _totalTime;
        set
        {
            if (!value.HasLegalInputs) return;
            if (_totalTime == value) return;
            _totalTime = value;
            TotalTimeChanged?.Invoke(this, value);
        }
    }

    internal Shape? SelectedItem { get; set; }
    internal GraphicsObjectTree GraphicsObjectTree { get; } = new();

    public AppFile()
    {
        _totalTime = AppSettings.Default.AnimationTime;
        _animationProperties = new List<AnimationProperties> { new() };
    }

    public AppFile(byte framesPerSecond, AnimationTime animationTime)
    {
        _totalTime = animationTime;
        _animationProperties = new List<AnimationProperties> { new() };
    }
}