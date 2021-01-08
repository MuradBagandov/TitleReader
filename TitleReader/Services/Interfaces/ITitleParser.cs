using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using TitleReader.Models;

namespace TitleReader.Services.Interfaces
{
    interface ITitleParser
    {
        CancellationToken Cancellation { get; set; }

        Title GetTitle(object p);

        Task<Title> GetTitleAsync(object p);

        object GetContent(object p);

        Task<object> GetContentAsync(object p);

    }
}
