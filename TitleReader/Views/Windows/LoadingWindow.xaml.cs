using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TitleReader.Views.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoadingWindow.xaml
    /// </summary>
    public partial class LoadingWindow : Window
    {
        public LoadingWindow()
        {
            InitializeComponent();
        }


        private IProgress<double> _progress;

        public IProgress<double> Progress => _progress ??= new Progress<double>(v => { if (v == 1) this.Close(); });


        private CancellationTokenSource _cancelationToken;

        public CancellationToken Cancel
        {
            get
            {
                if (_cancelationToken == null)
                {
                    _cancelationToken = new CancellationTokenSource();
                    Button_Close.Visibility = Visibility.Visible;
                }
                   
                return _cancelationToken.Token;
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            _cancelationToken?.Cancel();
            this.Close();
        }
    }
}
