using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TitleReader.Models;

namespace TitleReader.Services
{
    class RanobelibTitleParser : Interfaces.ITitleParser
    {
        private WebClient _client = new WebClient();

        public Title GetTitle(object p)
        {
            if (!(p is Uri uri))
                throw new ArgumentException("Неккорректный аргумент");
            
            string _web_string;
            try
            {
                _web_string = _client.DownloadString(uri);
            }
            catch
            {
                throw new Exception("Ошибка подключения");
            }

            string name, alterName, description;

            string info_section = Regex.Match(_web_string, @"<section([\s\S]+)<\/section>").Value;

            name = Regex.Match(info_section,
                @"<meta itemprop=.name. content=.([^<>]+).\s*\/>").Value;
            alterName = Regex.Match(info_section,
                @"<meta itemprop=.alternativeHeadline. content=.([^<>]+).\s*\/>").Value;
            description = Regex.Match(info_section,
                @"<meta itemprop=.description. content=.([^<>]+).\s*\/>").Value;

            string cover_uri =
                Regex.Match(info_section, @"<img src=.([^<>\u0022]+)..*class=.manga__cover.>").Value;

            Uri.TryCreate(cover_uri, UriKind.Absolute, out Uri cover);


            Title title = new Title()
            {
                Name = name,
                AlterName = alterName,
                Description = description,
                Uri = uri,
                Cover = cover,
                Chapters = GetChapters(_web_string)
            };

            return title;
        }

        private List<Chapter> GetChapters(string web_string)
        {
            List<Chapter> result = new List<Chapter>();
            string name;
            string number;
            Uri uri;

            var chapter_matches = Regex.Matches(web_string, 
                @"<a\s*class=.link-default.\s*title=([^<>]*)href=.(.*).>([^<]*)(?=<)");

            if (chapter_matches.Count == 0)
                return default;

            foreach (Match match in chapter_matches)
            {
                name = match.Groups[1].Value;
                number = Regex.Replace(match.Groups[3].Value, @"[\s\n]+", " ");
                uri = new Uri(match.Groups[2].Value);

                result.Add(new Chapter($"{number} {name}", uri));
            }
            return result;
        }
    }
}
