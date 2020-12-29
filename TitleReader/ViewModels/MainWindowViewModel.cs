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

        #region Properties

        public MainTitleViewModel MainTitleViewModel { get;  }

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

        #endregion


        #region Commands

        #region LoadTitleCommand
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
            catch { }
            MainTitleViewModel.RefreshTitleCommand.Execute(p);
        } 
        #endregion

        #endregion

        public MainWindowViewModel(ITitleParser parser, MainTitleViewModel TitleWindow )
        {
            MainTitleViewModel = TitleWindow;
            MainTitleViewModel.MainWindowViewModel = this;

            LoadTitleCommand = new LambdaCommand(OnLoadTitleCommandExecute, CanLoadTitleCommandExecuted);

            _parser = parser;   
        }
    }
}
