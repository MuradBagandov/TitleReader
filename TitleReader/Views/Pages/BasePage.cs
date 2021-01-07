using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TitleReader.Views.Pages
{
    public class BasePage : Page
    {
        public BasePage()
        {
            Loaded += BasePage_Loaded;
        }

        private  void BasePage_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {
            AnimatedLoad();
        }

       

        public void AnimatedLoad()
        {
            var sb = new Storyboard();
            
            ThicknessAnimation animationSlide = new ThicknessAnimation();
            animationSlide.From = new System.Windows.Thickness(WindowWidth,0 , -WindowWidth, 0);
            animationSlide.To = new System.Windows.Thickness(0);
            animationSlide.Duration = TimeSpan.FromMilliseconds(300);
            animationSlide.DecelerationRatio = 0.9;

            DoubleAnimation animationOpacity = new DoubleAnimation();
            animationOpacity.From = 0.0;
            animationOpacity.To = 1.0;
            animationOpacity.Duration = TimeSpan.FromMilliseconds(200);


            Storyboard.SetTargetProperty(animationSlide, new System.Windows.PropertyPath("Margin"));
            Storyboard.SetTargetProperty(animationOpacity, new System.Windows.PropertyPath("Opacity"));

            //sb.Children.Add(animationSlide);
            sb.Children.Add(animationOpacity);

            sb.Begin(this);
        }

        public void  AnimatedUnLoad()
        {
            var sb = new Storyboard();

            DoubleAnimation animationOpacity = new DoubleAnimation();
            animationOpacity.From = 1.0;
            animationOpacity.To = 0.0;
            animationOpacity.Duration = TimeSpan.FromMilliseconds(500);


            Storyboard.SetTargetProperty(animationOpacity, new System.Windows.PropertyPath("Opacity"));
            sb.Children.Add(animationOpacity);

            sb.Begin(this);
        }
    }
}
