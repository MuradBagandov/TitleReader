using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitleReader.Models
{
    internal class Chapter
    {
        public string Name { get; set; }

        public string Number { get; set; }

        public Uri Uri { get; }

        public string Date { get; set; }

        public Chapter(Uri uri) => Uri = uri;

    }
}
