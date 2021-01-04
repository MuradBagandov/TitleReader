﻿using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TitleReader.Services.Interfaces;

namespace TitleReader.Models
{
    internal class Title
    {
        public string Name { get;  set; }

        public string AlterName { get; set; }

        public string Names { get;  set; }

        public string Description { get; set; }

        public string DateOfRelease { get; set; }

        public string Status { get; set; }

        public string StatusTranslate { get; set; }

        public List<string> Authors { get; set; }

        public List<string> Genres { get; set; }

        public Uri Uri { get;  set; }

        public Uri Cover { get; set; }

        public LinkedList<Chapter> Chapters { get;  set; }
    }
}
