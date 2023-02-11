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
    private GraphicsTool _graphicsTool = GraphicsTools.ItemSelectTool;

    internal GraphicsTool GraphicsTool {
        get => _graphicsTool;
        private set { _graphicsTool = value; ToolChanged?.Invoke(this, value); }
    }

    internal event EventHandler<GraphicsTool>? ToolChanged;

    public ToolView()
    {
        InitializeComponent();
    }

    // Oversimplified for easier debugging.
    private void OnCursorButtonClick(object sender, RoutedEventArgs e) => GraphicsTool = GraphicsTools.ItemSelectTool;
    private void OnCurveButtonClick(object sender, RoutedEventArgs e) { }

    // Oversimplified for easier debugging.
    private void OnShapeButtonClick(object sender, RoutedEventArgs e) => GraphicsTool = GraphicsTools.EllipseTool;

    private void OnTextButtonClick(object sender, RoutedEventArgs e) { }
    private void OnMediaButtonClick(object sender, RoutedEventArgs e) { }
    private void OnEffectButtonClick(object sender, RoutedEventArgs e) { }
}