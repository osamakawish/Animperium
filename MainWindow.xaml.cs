using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MathAnim
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Timer _timer = new();

        public MainWindow()
        {
            InitializeComponent();

            TestAnimate();
        }   

        private void TestAnimate()
        {
            var ellipse = new Ellipse()
            {
                Width = 20,
                Height = 50,
                Fill = new SolidColorBrush(Colors.Red)
            };

            Canvas.SetTop(ellipse, 0);
            Canvas.SetLeft(ellipse, 0);

            MainCanvas.Children.Add(ellipse);

            _timer = new Timer(1000d / 24);

            _timer.Elapsed += (_, e) => MainCanvas.Dispatcher.Invoke(() =>
            {
                Canvas.SetTop(ellipse, Canvas.GetTop(ellipse) + 1);
            });

            _timer.Start();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            _timer.Stop();
        }
    }
}
