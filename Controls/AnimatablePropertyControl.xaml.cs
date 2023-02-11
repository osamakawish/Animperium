using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace Animperium.Controls;

/// <summary>
/// Interaction logic for AnimatablePropertyControl.xaml
/// </summary>
public partial class AnimatablePropertyControl : UserControl, IAnimatablePropertyControl
{
    internal UserControl PropertyModifierControl
    {
        get => (UserControl)AnimatablePropertyModifier.Content;
        set => AnimatablePropertyModifier.Content = value;
    }

    public bool IsAnimatable { get; private set; }

    public AnimatablePropertyControl()
    {
        InitializeComponent();
    }

    private void IsAnimatableButton_Click(object sender, RoutedEventArgs e)
    {
        IsAnimatable = !IsAnimatable;
        IsAnimatableButton.Background = new SolidColorBrush
            (IsAnimatable ? Color.FromRgb(0x40, 0xa0, 0x40) : Color.FromRgb(0x60, 0x60, 0x60));
    }
}