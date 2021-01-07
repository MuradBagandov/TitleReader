using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using TitleReader.Infrastuctures;
using TitleReader.Infrastuctures.Commands;
using TitleReader.Models;
using TitleReader.Services.Interfaces;

namespace TitleReader.ViewModels
{
    internal class TitleViewModel : Base.ViewModel
    {

        #region Properties
        public MainWindowViewModel MainWindowViewModel { get; internal set; }

        #region Title : Title
        private Title _title;

        public Title Title
        {
            get => _title;
            set
            {
                if (Set(ref _title, value))
                {
                    if (Title?.Cover != null)
                    {
                        WebClient client = new WebClient();

                        var coverdata = client.DownloadData(Title.Cover);
                        Cover = BitmapSourceFromByteArray(coverdata);
                    }
                } 
            }
        }
        #endregion

        #region Chapter : SelectChapter - Выбранная глава
        private Chapter _selectChapter;

        public Chapter SelectChapter
        {
            get => _selectChapter;
            set
            {
                if (Set(ref _selectChapter, value))
                    _selectLinkedChapter = Title?.Chapters?.Find(_selectChapter);
            }

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

        #region string : SelectChapterContent
        private string _selectChapterContent;

        public string SelectChapterContent
        {
            get => _selectChapterContent;
            set => Set(ref _selectChapterContent, value);
        }
        #endregion

        #endregion


        #region Commands

        #region ReadSelectChapterCommand
        public ICommand ReadSelectChapterCommand { get; }

        private bool CanReadSelectChapterCommandExecuted(object p) => SelectChapter != null;


        private async void OnReadSelectChapterCommandExecute(object p)
        {
            await ShowSelectChapter();
        }
        #endregion

        #region BeginToReadCommand
        public ICommand BeginToReadCommand { get; }

        private bool CanBeginToReadCommandExecuted(object p) => Title != null && Title?.Chapters != null && Title?.Chapters.Count > 0;

        private async void OnBeginToReadCommandExecute(object p)
        {
            SelectChapter = Title.Chapters.Last.Value;
            await ShowSelectChapter();
        }
        #endregion

        #region ReadNextChapterCommand
        public ICommand ReadNextChapterCommand { get; }

        private bool CanReadNextChapterCommandExecuted(object p) => _selectLinkedChapter != null && _selectLinkedChapter?.Previous != null;

        private async void OnReadNextChapterCommandExecute(object p)
        {
            SelectChapter = _selectLinkedChapter.Previous.Value;
            await ShowSelectChapter();
        }
        #endregion

        #region ReadPrevoiusChapterCommand
        public ICommand ReadPrevoiusChapterCommand { get; }

        private bool CanReadPrevoiusChapterCommandExecuted(object p) => _selectLinkedChapter != null && _selectLinkedChapter?.Next != null;

        private async void OnReadPrevoiusChapterCommandExecute(object p)
        {

            SelectChapter = _selectLinkedChapter.Next.Value;
            await ShowSelectChapter();

        }

       
        #endregion

        #endregion


        #region Fields
        private IContentChapterParser _contentParser;
        private LinkedListNode<Chapter> _selectLinkedChapter;
        private BitmapSource _defaultCover; 
        #endregion

        public TitleViewModel(IContentChapterParser ContentParser) 
        {
            _contentParser = ContentParser;

            ReadSelectChapterCommand = new LambdaCommand(OnReadSelectChapterCommandExecute, CanReadSelectChapterCommandExecuted);
            ReadPrevoiusChapterCommand = new LambdaCommand(OnReadPrevoiusChapterCommandExecute, CanReadPrevoiusChapterCommandExecuted);
            ReadNextChapterCommand = new LambdaCommand(OnReadNextChapterCommandExecute, CanReadNextChapterCommandExecuted);
            BeginToReadCommand = new LambdaCommand(OnBeginToReadCommandExecute, CanBeginToReadCommandExecuted);

            _defaultCover = new BitmapImage(new Uri(@"/Resources/Images/no-image.png", UriKind.Relative));
            Cover = _defaultCover;
        }


        private static BitmapSource BitmapSourceFromByteArray(byte[] buffer)
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

        private async Task ShowSelectChapter()
        {
            try
            {
                MainWindowViewModel.CurrentPage = ApplicationPages.LoadingPage;
                await LoadSelectChapterContent();
                MainWindowViewModel.CurrentPage = ApplicationPages.ChapterNovell;
            }
            catch
            {

            }
        }

        private async Task LoadSelectChapterContent()
        {
            if (SelectChapter != null)
            {
                try
                {
                    string result = (string) await _contentParser.GetContentAsync(SelectChapter.Uri);

                    SelectChapterContent = String.IsNullOrWhiteSpace(result) == true ? String.Empty : result;
                }
                catch
                {

                }
            }
        }
    }
}
