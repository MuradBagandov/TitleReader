using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Markup;

namespace TitleReader.Infrastuctures.Convertors
{
    [ValueConversion(typeof(string), typeof(string))]
    [MarkupExtensionReturnType(typeof(ChapterNameFormatToListBox))]
    internal class ChapterNameFormatToListBox : Base.Convertor
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                throw new NotImplementedException();

            if (!(value is string str))
                throw new NotImplementedException();

            if (!string.IsNullOrWhiteSpace(str))
                str = " - " + str;

            return str;
        }

        public override object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
