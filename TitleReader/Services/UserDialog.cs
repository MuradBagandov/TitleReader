using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TitleReader.Services.Enums;
using TitleReader.Views.Windows;

namespace TitleReader.Services
{
    class UserDialog : Interfaces.IUserDialog
    {
        public (CancellationToken cancellation, Action close) ShowLoading()
        {
            var parent_window = App.ActiveWindow;
            var _LoadingWindow = new LoadingWindow()
            {
                Owner = parent_window,
            };

            App.ActiveWindow.IsEnabled = false;
            _LoadingWindow.Show();

            return (_LoadingWindow.Cancel, ()=> { _LoadingWindow.Close(); parent_window.IsEnabled = true; });
        }

        public void ShowMessage(string text, ShowMessageIcon icon)
        {
            var DialogWindow = new ShowMessageWindow()
            {
                ShowMessageText = text,
                IconDialog = icon,
                Owner = App.ActiveWindow,
                ShowInTaskbar = false
            };

            DialogWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterOwner;
            DialogWindow.ShowDialog();
        }
    }
}
