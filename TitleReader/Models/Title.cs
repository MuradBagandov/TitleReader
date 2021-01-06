using System;
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

        public override bool Equals(object obj)
        {
            if (!(obj is Title title))
                return false;

            if (this.Name != title.Name)
                return false;

            if (this.AlterName != title.AlterName)
                return false;

            if (this.Names != title.Names)
                return false;

            if (this.Description != title.Description)
                return false;

            if (this.Status != title.Status)
                return false;

            if (this.StatusTranslate != title.StatusTranslate)
                return false;

            if (this.DateOfRelease != title.DateOfRelease)
                return false;

            if (!Cover.Equals(title.Cover))
                return false;

            if (!Uri.Equals(title.Uri))
                return false;

            if (Genres.Equals(title.Genres))
                return false;

            if (Authors.Equals(title.Authors))
                return false;

            if (Chapters.Equals(title.Chapters))
                return false;

            return true;
        }
    }
}
