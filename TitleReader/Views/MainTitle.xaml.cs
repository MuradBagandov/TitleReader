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

namespace TitleReader.Views
{
    /// <summary>
    /// Логика взаимодействия для MainTitle.xaml
    /// </summary>
    public partial class MainTitle : Page
    {
        public MainTitle()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigateForward();
        }


        private void Border_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigateForward();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            NavigateBack();
        }

        private void NavigateForward()
        {
            try
            {
                NavigationService.GoForward();
            }
            catch
            {
                NavigationService.Navigate(new Uri("Views/ChapterNovell.xaml", UriKind.Relative));
            }
        }
        private void NavigateBack()
        {
            try
            {
                NavigationService.GoBack();
            }
            catch
            {
                NavigationService.Navigate(new Uri("Views/MainPage.aml", UriKind.Relative));
            }
        }
    }
}
