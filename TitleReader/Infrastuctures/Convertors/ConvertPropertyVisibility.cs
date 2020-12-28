using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;



namespace TitleReader.Infrastuctures.Convertors
{
    [ValueConversion(typeof(Visibility), typeof(Visibility))]
    [MarkupExtensionReturnType(typeof(ConvertPropertyVisibility))]
    class ConvertPropertyVisibility : Base.Convertor
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility vis))
                throw new NotImplementedException();
            if (vis == Visibility.Visible)
                return Visibility.Hidden;
            else
                return Visibility.Visible;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility vis))
                throw new NotImplementedException();
            if (vis == Visibility.Visible)
                return Visibility.Hidden;
            else
                return Visibility.Visible;
        }
    }
}
