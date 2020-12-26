using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TitleReader.Models
{
    internal class Chapter
    {
        public string Name { get; }

        public Uri Uri { get; }

        public Chapter(Uri uri) => Uri = uri;

        public Chapter(string name, Uri uri) : this(uri) => Name = name;
    }
}
