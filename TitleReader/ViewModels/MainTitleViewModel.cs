using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TitleReader.Infrastuctures.Commands;
using TitleReader.Models;
using TitleReader.Services.Interfaces;

namespace TitleReader.ViewModels
{
    internal class MainTitleViewModel : Base.ViewModel
    {
       

        #region Properties
        public MainWindowViewModel MainWindowViewModel { get; internal set; }

        #region Chapter : SelectChapter - Выбранная глава
        private Chapter _selectChapter;

        public Chapter SelectChapter
        {
            get => _selectChapter;
            set => Set(ref _selectChapter, value);
        }
        #endregion


        #region BitmapImage : Cover
        private BitmapSource _cover;

        public BitmapSource Cover
        {
            get => _cover;
            set => Set(ref _cover, value);
        }
        #endregion

        #endregion


        #region Commands

        #region RefreshTitleCommand
        public ICommand RefreshTitleCommand { get; }

        private bool CanRefreshTitleCommandExecuted(object p)
            => true;

        private void OnRefreshTitleCommandExecute(object p)
        {

            if (MainWindowViewModel.Title?.Cover != null)
            {
                WebClient client = new WebClient();

                var coverdata = client.DownloadData(MainWindowViewModel.Title.Cover);
                Cover = BitmapSourceFromByteArray(coverdata);
            }
        } 
        #endregion

        #endregion

        private BitmapSource _defaultCover;

        public MainTitleViewModel()
        {
            RefreshTitleCommand = new LambdaCommand(OnRefreshTitleCommandExecute, CanRefreshTitleCommandExecuted);

            _defaultCover = new BitmapImage(new Uri(@"/Resources/Images/no-image.png", UriKind.Relative));
            Cover = _defaultCover;
        }


        public static BitmapSource BitmapSourceFromByteArray(byte[] buffer)
        {
            var bitmap = new BitmapImage();

            using (var stream = new MemoryStream(buffer))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }

            bitmap.Freeze(); 
            return bitmap;
        }
    }
}
