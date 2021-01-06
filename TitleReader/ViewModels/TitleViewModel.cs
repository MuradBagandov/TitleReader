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
                    RefreshTitleCommand?.Execute(default);
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

        #region RefreshTitleCommand
        public ICommand RefreshTitleCommand { get; }

        private bool CanRefreshTitleCommandExecuted(object p)
            => true;

        private void OnRefreshTitleCommandExecute(object p)
        {

            if (Title?.Cover != null)
            {
                WebClient client = new WebClient();

                var coverdata = client.DownloadData(Title.Cover);
                Cover = BitmapSourceFromByteArray(coverdata);
            }
        }
        #endregion

        #region GetSelectChapterContentCommand
        public ICommand GetSelectChapterContentCommand { get; }

        private bool CanGetSelectChapterContentCommandExecuted(object p) => SelectChapter != null;


        private void OnGetSelectChapterContentCommandExecute(object p)
        {
            string result = ((string)_contentParser.GetContent(SelectChapter.Uri));

            SelectChapterContent = String.IsNullOrWhiteSpace(result) == true ? String.Empty : result;

            MainWindowViewModel.CurrentPage = ApplicationPages.ChapterNovell;
        }
        #endregion

        #region BeginReaderCommand
        public ICommand FirstChapterCommand { get; }

        private bool CanFirstChapterCommandExecuted(object p) => Title != null && Title?.Chapters != null && Title?.Chapters.Count > 0;

        private void OnFirstChapterCommandExecute(object p)
        {
            SelectChapter = Title.Chapters.Last.Value;
            GetSelectChapterContentCommand?.Execute(p);  
        }
        #endregion

        #region NextChapterCommand
        public ICommand NextChapterCommand { get; }

        private bool CanNextChapterCommandExecuted(object p) => _selectLinkedChapter != null && _selectLinkedChapter?.Previous != null;

        private void OnNextChapterCommandExecute(object p)
        {
            SelectChapter = _selectLinkedChapter.Previous.Value;
            GetSelectChapterContentCommand?.Execute(p);
        }
        #endregion

        #region PrevoiusChapterCommand
        public ICommand PrevoiusChapterCommand { get; }

        private bool CanPrevoiusChapterCommandExecuted(object p) => _selectLinkedChapter != null && _selectLinkedChapter?.Next != null;

        private void OnPrevoiusChapterCommandExecute(object p)
        {
            SelectChapter = _selectLinkedChapter.Next.Value;
            GetSelectChapterContentCommand?.Execute(p);
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

            RefreshTitleCommand = new LambdaCommand(OnRefreshTitleCommandExecute, CanRefreshTitleCommandExecuted);
            GetSelectChapterContentCommand = new LambdaCommand(OnGetSelectChapterContentCommandExecute, CanGetSelectChapterContentCommandExecuted);
            PrevoiusChapterCommand = new LambdaCommand(OnPrevoiusChapterCommandExecute, CanPrevoiusChapterCommandExecuted);
            NextChapterCommand = new LambdaCommand(OnNextChapterCommandExecute, CanNextChapterCommandExecuted);
            FirstChapterCommand = new LambdaCommand(OnFirstChapterCommandExecute, CanFirstChapterCommandExecuted);

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
