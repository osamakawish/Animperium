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
    private VisualAnimationTool _visualAnimationTool = AnimationTools.ItemSelectTool;

    internal VisualAnimationTool VisualAnimationTool {
        get => _visualAnimationTool;
        private set { _visualAnimationTool = value; ToolChanged?.Invoke(this, value); }
    }

    internal event EventHandler<VisualAnimationTool>? ToolChanged;

    public ToolView()
    {
        InitializeComponent();
    }

    // Oversimplified for easier debugging.
    private void OnCursorButtonClick(object sender, RoutedEventArgs e) => VisualAnimationTool = AnimationTools.ItemSelectTool;
    private void OnCurveButtonClick(object sender, RoutedEventArgs e) { }

    // Oversimplified for easier debugging.
    private void OnShapeButtonClick(object sender, RoutedEventArgs e) => VisualAnimationTool = AnimationTools.RectangleTool;

    private void OnTextButtonClick(object sender, RoutedEventArgs e) { }
    private void OnMediaButtonClick(object sender, RoutedEventArgs e) { }
    private void OnEffectButtonClick(object sender, RoutedEventArgs e) { }
}