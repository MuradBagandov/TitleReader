using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using TitleReader.Infrastuctures.Commands;
using TitleReader.Models;
using TitleReader.Services.Interfaces;

namespace TitleReader.ViewModels
{
    internal class MainWindowViewModel : Base.ViewModel
    {
        private ITitleParser _parser;

        #region Properties

        #region string : Address
        private string _address;

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

        public ICommand LoadTitleCommand { get; }

        private bool CanLoadTitleCommandExecuted(object p) 
            => Uri.TryCreate(Adrress, UriKind.Absolute, out Uri add);

        private void OnLoadTitleCommandExecute(object p)
        {
            Title =  _parser.GetTitle(new Uri(Adrress));
        }

        #endregion

        public MainWindowViewModel(ITitleParser parser )
        {
            LoadTitleCommand = new LambdaCommand(OnLoadTitleCommandExecute, CanLoadTitleCommandExecuted);

            _parser = parser;

        }
    }
}
