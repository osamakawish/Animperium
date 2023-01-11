using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathAnim.Controls
{
    /// <summary>
    /// Interaction logic for AnimatablePropertyControl.xaml
    /// </summary>
    public partial class AnimatablePropertyControl : UserControl, IAnimatablePropertyControl
    {
        private bool isAnimatable = false;

        public AnimatablePropertyControl()
        {
            InitializeComponent();
        }

        public bool IsAnimatable => isAnimatable;

        void SetPropertyModifierControl(UserControl control) => AnimatablePropertyModifier.Content = control;

        private void IsAnimatableButton_Click(object sender, RoutedEventArgs e)
        {
            isAnimatable = !isAnimatable;
            IsAnimatableButton.Background = new SolidColorBrush
                (isAnimatable ? Color.FromRgb(0x40, 0xa0, 0x40) : Color.FromRgb(0x60, 0x60, 0x60));
        }
    }
}
