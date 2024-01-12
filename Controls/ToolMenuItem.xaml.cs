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

namespace NodeGraphTest.Controls
{
    /// <summary>
    /// Interaction logic for ToolMenuItem.xaml
    /// </summary>
    public partial class ToolMenuItem : UserControl
    {
        public ToolMenuItem()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty IconColorProperty = DependencyProperty.Register("IconColor", typeof(Brush), typeof(ToolMenuItem), new PropertyMetadata());

        public Brush IconColor
        {
            get { return IconColor; }
            set { SetValue(IconColorProperty, value); }
        }

        public static readonly DependencyProperty IconProperty = DependencyProperty.Register("Icon", typeof(ImageSource), typeof(ToolMenuItem), new PropertyMetadata());

        public ImageSource Icon
        {
            get { return (ImageSource)GetValue(IconProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public static readonly DependencyProperty IconHoverProperty = DependencyProperty.Register("IconHover", typeof(ImageSource), typeof(ToolMenuItem), new PropertyMetadata());

        public ImageSource IconHover
        {
            get { return (ImageSource)GetValue(IconHoverProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public static readonly DependencyProperty IconPressProperty = DependencyProperty.Register("IconPress", typeof(ImageSource), typeof(ToolMenuItem), new PropertyMetadata());

        public ImageSource IconPress
        {
            get { return (ImageSource)GetValue(IconPressProperty); }
            set { SetValue(IconColorProperty, value); }
        }

        public event RoutedEventHandler Click
        {
            add { ToolMenuBtn.Click += value; }
            remove { ToolMenuBtn.Click -= value; }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            iconHolder.Source = IconHover;
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            iconHolder.Source = Icon;
        }

        private void Button_MouseDown(object sender, MouseButtonEventArgs e)
        {
            iconHolder.Source = IconPress;
        }
    }
}
