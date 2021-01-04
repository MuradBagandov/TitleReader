﻿using System;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TitleReader.Views
{
    /// <summary>
    /// Логика взаимодействия для ChapterNovell.xaml
    /// </summary>
    public partial class ChapterNovell : Page
    {
        private bool _isAnimated = false;
        public ChapterNovell()
        {
            InitializeComponent();
        }

        private void ContentScroll_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            
            if (e.VerticalChange > 0 && HeaderPanel.Opacity == 1.0)
            {
                HeaderVisibleAnimation(false);
            }
            else if (e.VerticalChange < 0 && HeaderPanel.Opacity == 0.0)
            {
                HeaderVisibleAnimation(true);
            }
        }

        public void HeaderVisibleAnimation(bool show)
        {
            if (_isAnimated)
                return;

            var timeAnimation = TimeSpan.FromMilliseconds(200.0);
            DoubleAnimation HeaderAnimation;

            _isAnimated = true;

            HeaderAnimation = show == true ? new DoubleAnimation(0.0, 1.0, timeAnimation) : new DoubleAnimation(1.0, 0.0, timeAnimation);


            HeaderAnimation.Completed += (s, a) => {
                _isAnimated = false;
            };
            HeaderPanel.BeginAnimation(DockPanel.OpacityProperty, HeaderAnimation);
        }

        private void ContentScroll_MouseWheel(object sender, MouseWheelEventArgs e)
        {
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            ContentScroll.ScrollToVerticalOffset(0);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            try
            {
                NavigationService.GoBack();
            }
            catch
            {
                NavigationService.Navigate(new Uri("Views/MainTitle.xaml", UriKind.Relative));
            }
        }
    }
}
