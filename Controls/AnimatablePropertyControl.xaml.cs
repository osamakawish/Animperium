﻿using System;
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
        private UserControl PropertyModifierControl
        {
            get => (UserControl)AnimatablePropertyModifier.Content;
            set => AnimatablePropertyModifier.Content = value;
        }

        private bool isAnimatable = false;
        public bool IsAnimatable => isAnimatable;

        public AnimatablePropertyControl()
        {
            InitializeComponent();
        }

        private void IsAnimatableButton_Click(object sender, RoutedEventArgs e)
        {
            isAnimatable = !isAnimatable;
            IsAnimatableButton.Background = new SolidColorBrush
                (isAnimatable ? Color.FromRgb(0x40, 0xa0, 0x40) : Color.FromRgb(0x60, 0x60, 0x60));
        }
    }
}
