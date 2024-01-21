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
    /// Interaction logic for ToggleableTextBox.xaml
    /// </summary>
    public partial class ToggleableTextBox : UserControl
    {
        public ToggleableTextBox()
        {
            InitializeComponent();
        }

        public static readonly DependencyProperty PreviewTextProperty = DependencyProperty.Register("PreviewText", typeof(string), typeof(ToggleableTextBox), new PropertyMetadata());
        public string PreviewText
        {
            get { return (string)GetValue(PreviewTextProperty); }
            set { SetValue(PreviewTextProperty, value); }
        }

        public static readonly DependencyProperty CanEditProperty = DependencyProperty.Register("CanEdit", typeof(bool), typeof(ToggleableTextBox), new PropertyMetadata());
        public bool CanEdit
        {
            get { return (bool)GetValue(CanEditProperty); }
            set { SetValue(CanEditProperty, value); }
        }



        public string GetText()
        {
            return textTB.Text;
        }

        private void textTB_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (textTB.Text.Length <= 0)
            {
                previewText.Visibility = Visibility.Visible;
            }
            else
            {
                previewText.Visibility = Visibility.Hidden;
            }
        }
    }
}
