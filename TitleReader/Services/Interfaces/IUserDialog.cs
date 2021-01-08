using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TitleReader.Services.Enums;

namespace TitleReader.Services.Interfaces
{
    internal interface IUserDialog
    {
        void ShowMessage(string text, ShowMessageIcon icon);

        (CancellationToken cancellation, Action close) ShowLoading();
    }
}
