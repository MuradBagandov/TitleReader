using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitleReader.Models;

namespace TitleReader.Services.Interfaces
{
    interface ITitleParser
    {
        Title GetTitle(object p);

        Task<Title> GetTitleAsync(object p);

    }
}
