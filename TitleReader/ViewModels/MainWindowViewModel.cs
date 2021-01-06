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
using TitleReader.Infrastuctures;
using TitleReader.Infrastuctures.Commands;
using TitleReader.Models;
using TitleReader.Services.Interfaces;

namespace TitleReader.ViewModels
{
    internal class MainWindowViewModel : Base.ViewModel
    {
        private ITitleParser _parser;

        #region Properties

        public TitleViewModel MainTitleViewModel { get;  }

        #region ApplicationPages : CurrentPage
        private ApplicationPages _currentPage = ApplicationPages.Main;

        public ApplicationPages CurrentPage
        {
            get => _currentPage;
            set => Set(ref _currentPage, value);
        } 
        #endregion

        #region string : Address
        private string _address = @"https://ranobelib.me/tales-of-herding-gods";

        public string Adrress
        {
            get => _address;
            set => Set(ref _address, value);
        }
        #endregion

        #endregion


        #region Commands

        #region LoadTitleCommand
        public ICommand LoadTitleCommand { get; }

        private bool CanLoadTitleCommandExecuted(object p)
            => Uri.TryCreate(Adrress, UriKind.Absolute, out Uri add);

        private void OnLoadTitleCommandExecute(object p)
        {
            MainTitleViewModel.Title = null;
            try
            {
                MainTitleViewModel.Title = _parser.GetTitle(new Uri(Adrress));
                CurrentPage = ApplicationPages.Title;
            }
            catch { }
        }
        #endregion

        #region ShowTitleCommand
        public ICommand ShowTitleCommand { get; }

        private void OnShowTitleCommandExecute(object p) => CurrentPage = ApplicationPages.Title;

        #endregion

        #region ShowMainCommand
        public ICommand ShowMainCommand { get; }

        private void OnShowMainCommandExecute(object p) => CurrentPage = ApplicationPages.Main;

        #endregion

        #region ShowChapterCommand
        public ICommand ShowChapterCommand { get; }

        private void OnShowChapterCommandExecute(object p) => CurrentPage = ApplicationPages.ChapterNovell;

        #endregion

        #endregion

        public MainWindowViewModel(ITitleParser parser, TitleViewModel TitleWindow )
        {
            _parser = parser;
            MainTitleViewModel = TitleWindow;
            MainTitleViewModel.MainWindowViewModel = this;

            LoadTitleCommand = new LambdaCommand(OnLoadTitleCommandExecute, CanLoadTitleCommandExecuted);
            ShowMainCommand = new LambdaCommand(OnShowMainCommandExecute);
            ShowTitleCommand = new LambdaCommand(OnShowTitleCommandExecute);
            ShowChapterCommand = new LambdaCommand(OnShowChapterCommandExecute);
        }
    }
}
