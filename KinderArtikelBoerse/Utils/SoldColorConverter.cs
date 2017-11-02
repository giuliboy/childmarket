using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace KinderArtikelBoerse.Utils
{
    public class SoldColorConverter : IValueConverter
    {
        public object Convert( object value, Type targetType, object parameter, CultureInfo culture )
        {
            if((bool)value )
            {
                return "#DD00EE00";
            }
            else
            {
                return "#DDFFFFFF";
            }
        }

        public object ConvertBack( object value, Type targetType, object parameter, CultureInfo culture )
        {
            throw new NotImplementedException();
        }
    }
}