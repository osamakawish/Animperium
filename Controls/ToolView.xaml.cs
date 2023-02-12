using System;
using System.Windows;
using System.Windows.Controls;
using Animperium.Graphics;

namespace Animperium.Controls;

/// <summary>
/// Interaction logic for ToolView.xaml
/// </summary>
public partial class ToolView
{
    private AnimationTool _animationTool = AnimationTools.ItemSelectTool;

    internal AnimationTool AnimationTool {
        get => _animationTool;
        private set { _animationTool = value; ToolChanged?.Invoke(this, value); }
    }

    internal event EventHandler<AnimationTool>? ToolChanged;

    public ToolView()
    {
        InitializeComponent();
    }

    // Oversimplified for easier debugging.
    private void OnCursorButtonClick(object sender, RoutedEventArgs e) => AnimationTool = AnimationTools.ItemSelectTool;
    private void OnCurveButtonClick(object sender, RoutedEventArgs e) { }

    // Oversimplified for easier debugging.
    private void OnShapeButtonClick(object sender, RoutedEventArgs e) => AnimationTool = AnimationTools.EllipseTool;

    private void OnTextButtonClick(object sender, RoutedEventArgs e) { }
    private void OnMediaButtonClick(object sender, RoutedEventArgs e) { }
    private void OnEffectButtonClick(object sender, RoutedEventArgs e) { }
}