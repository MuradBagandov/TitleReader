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

        
        public Visibility Base { get; set; } = Visibility.Hidden;

       

        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility vis))
                throw new NotImplementedException();
            return Convert_vis(vis);
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is Visibility vis))
                throw new NotImplementedException();
           
            return Convert_vis(vis);
        }

        public Visibility Convert_vis(Visibility value)
        {
            if (value == Visibility.Visible)
                return Base;
            else
                return Visibility.Visible;
        }
    }
}
