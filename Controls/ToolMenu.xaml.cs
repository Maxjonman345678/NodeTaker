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
    /// Interaction logic for ToolMenu.xaml
    /// </summary>
    public partial class ToolMenu : UserControl
    {
        public ToolMenu()
        {
            InitializeComponent();
        }

        public event RoutedEventHandler ColorClick
        {
            add { fillBtn.Click += value; }
            remove { fillBtn.Click -= value; }
        }

        public event RoutedEventHandler EditClick
        {
            add { editBtn.Click += value; }
            remove { editBtn.Click -= value; }
        }

        public event RoutedEventHandler DeleteClick
        {
            add { deleteBtn.Click += value; }
            remove { deleteBtn.Click -= value; }
        }

        private void colorFillBtn_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void editBtn_Click(object sender, RoutedEventArgs e)
        {

        }

        private void deleteBtn_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
