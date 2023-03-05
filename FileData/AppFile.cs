using System;
using System.Collections.Generic;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
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

    private byte _framesPerSecond = FileSettings.Default.AnimationTime.FramesPerSecond;
    private AnimationTime _totalTime = FileSettings.Default.AnimationTime;

    internal DoubleTolerance DoubleTolerance = FileSettings.Default.Tolerance;

    internal List<Storyboard> Storyboards { get; } = new() { new Storyboard() };
    private int _currentStoryboardIndex;

    internal Storyboard CurrentStoryboard => Storyboards[_currentStoryboardIndex];
    internal Storyboard GoToNextStoryboard() => Storyboards[++_currentStoryboardIndex % Storyboards.Count];
    internal Storyboard GoToPreviousStoryboard() => Storyboards[--_currentStoryboardIndex % Storyboards.Count];

    internal byte FramesPerSecond
    {
        get => _framesPerSecond;
        set
        {
            if (_framesPerSecond == value) return;
            _framesPerSecond = value;
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
        
    }

    public AppFile(byte framesPerSecond, AnimationTime animationTime)
    { _framesPerSecond = framesPerSecond; _totalTime = animationTime; }
}