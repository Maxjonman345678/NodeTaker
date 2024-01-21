using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;

namespace NodeGraphTest.Data.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    class InverseBoolToVisibilityConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            Visibility type;
            if (targetType != typeof(Visibility))
                throw new InvalidOperationException("The target must be a Visibility type");

            if (!(bool)value)
            {
                type = Visibility.Visible;
            }
            else
            {
                type = Visibility.Hidden;
            }

            return type;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
