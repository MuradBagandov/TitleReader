using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TitleReader.Infrastuctures.Commands;
using TitleReader.Models;
using TitleReader.Services.Interfaces;

namespace TitleReader.ViewModels
{
    internal class MainWindowViewModel : Base.ViewModel
    {
        private ITitleParser _parser;
        private BitmapSource _defaultCover;

        #region Properties

        #region string : Address
        private string _address = @"https://ranobelib.me/tales-of-herding-gods";

        public string Adrress
        {
            get => _address;
            set => Set(ref _address, value);
        }
        #endregion

        #region Title : Title
        private Title _title;

        public Title Title
        {
            get => _title;
            set => Set(ref _title, value);
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

        public ICommand LoadTitleCommand { get; }

        private bool CanLoadTitleCommandExecuted(object p) 
            => Uri.TryCreate(Adrress, UriKind.Absolute, out Uri add);

        private void OnLoadTitleCommandExecute(object p)
        {
            Title = null;
            try
            {
                Title = _parser.GetTitle(new Uri(Adrress));
            }
            catch {}
            

            if (Title?.Cover != null)
            {
                WebClient client = new WebClient();

                var coverdata = client.DownloadData(Title.Cover);
                Cover = BitmaSourceFromByteArray(coverdata);
            }
            else
                Cover = _defaultCover;
        }

        #endregion

        public MainWindowViewModel(ITitleParser parser )
        {
            LoadTitleCommand = new LambdaCommand(OnLoadTitleCommandExecute, CanLoadTitleCommandExecuted);

            _parser = parser;

            _defaultCover = new BitmapImage(new Uri(@"/Resources/Images/no-image.png", UriKind.Relative));
            Cover = _defaultCover;
        }

        public static BitmapSource BitmaSourceFromByteArray(byte[] buffer)
        {
            var bitmap = new BitmapImage();

            using (var stream = new MemoryStream(buffer))
            {
                bitmap.BeginInit();
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.StreamSource = stream;
                bitmap.EndInit();
            }

            bitmap.Freeze(); // optionally make it cross-thread accessible
            return bitmap;
        }

    }
}
