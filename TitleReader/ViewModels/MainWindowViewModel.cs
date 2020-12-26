using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitleReader.ViewModels
{
    internal class MainWindowViewModel:Base.ViewModel
    {
        private string _title = "TitleReader";

        public string Title
        {
            get => _title;
            set => Set(ref _title, value);
        }

    }
}
