using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Data;
using System.Windows.Controls;
using System.Windows.Markup;
using TitleReader;


namespace TitleReader.Infrastuctures.Convertors
{
    [MarkupExtensionReturnType(typeof(ApplicationPagesConvertor))]
    [ValueConversion(typeof (ApplicationPages), typeof(Page))]
    class ApplicationPagesConvertor : Base.Convertor
    {
        static Page MainPage = new Views.MainPage();
        static Page TitlePage = new Views.MainTitle();
        static Page ChapterPage = new Views.ChapterNovell();
        static Page LoadingPage = new Views.LoadingPage();
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is ApplicationPages page))
                throw new NotImplementedException();

            return page switch
            {
                ApplicationPages.Main => MainPage,
                ApplicationPages.Title => TitlePage,
                ApplicationPages.ChapterNovell => ChapterPage,
                ApplicationPages.LoadingPage => LoadingPage,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
