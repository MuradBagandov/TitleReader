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
    [ValueConversion(typeof(List<string>), typeof(string))]
    [MarkupExtensionReturnType(typeof(ListToString))]
    internal class ListToString : Base.Convertor
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is List<string> list))
                throw new NotImplementedException();

            return String.Join(", ", list.ToArray());
        }
    }
}
