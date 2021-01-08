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
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using TitleReader.Services.Enums;
using FontAwesome.WPF;

namespace TitleReader.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для ShowMessageWindow.xaml
    /// </summary>
    public partial class ShowMessageWindow : Window
    {
        public static readonly DependencyProperty ShowMessageTextProperty =
            DependencyProperty.Register(
                nameof(ShowMessageText), 
                typeof(string), 
                typeof(ShowMessageWindow));

        public string ShowMessageText
        {
            get => (string)GetValue(ShowMessageTextProperty);
            set => SetValue(ShowMessageTextProperty, value);
        }

        public static readonly DependencyProperty IconMessageProperty =
           DependencyProperty.Register(
               nameof(IconDialog),
               typeof(ShowMessageIcon),
               typeof(ShowMessageWindow),
               new PropertyMetadata(ShowMessageIcon.None));


        public ShowMessageIcon IconDialog
        {
            get => (ShowMessageIcon)GetValue(IconMessageProperty);
            set => SetValue(IconMessageProperty, value);
        }

        public ShowMessageWindow()
        {
            InitializeComponent();
            this.Loaded += ShowMessageWindow_Loaded;
            
        }

        private void ShowMessageWindow_Loaded(object sender, RoutedEventArgs e)
        {
            switch (IconDialog)
            {
                case ShowMessageIcon.None: IconMessage.Visibility = Visibility.Collapsed; break;
                case ShowMessageIcon.Warning: IconMessage.Icon = FontAwesome.WPF.FontAwesomeIcon.Warning; break;
                case ShowMessageIcon.Infomation: IconMessage.Icon = FontAwesome.WPF.FontAwesomeIcon.InfoCircle; break;
                case ShowMessageIcon.Error: IconMessage.Icon = FontAwesome.WPF.FontAwesomeIcon.Warning; break;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
