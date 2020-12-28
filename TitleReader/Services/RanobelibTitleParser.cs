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
        private WebClient _client = new WebClient() { Encoding = Encoding.UTF8 };

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

            string name, alterName, description, dateOfRelease, status;

            string info_section = Regex.Match(_web_string, @"<section([\s\S]+)<\/section>").Value;

            name = Regex.Match(info_section,
                @"<meta itemprop=.name. content=.([^<>]+).\s+\/>").Groups[1].Value;

            name = name.Replace("(Новелла)", "");

            alterName = Regex.Match(info_section,
                @"<meta itemprop=.alternativeHeadline. content=.([^<>]+).\s+\/>").Groups[1].Value;
            description = Regex.Match(info_section,
                @"<meta itemprop=.description. content=.([^<>]+).\s+\/>").Groups[1].Value;

            description = description.Replace("&quot;", "\u0022");
            description = Regex.Replace(description, @"&([^;&\s]+;)?[^;&\s]+;", " ");

            description = description.Replace("&amp;nbsp;", " ");

            dateOfRelease = Regex.Match(info_section,
                @"<strong>Дата релиза<\/strong>[\n\s]*<span>([^<>]*)<\/span>").Groups[1].Value;

            status = Regex.Match(info_section,
                @"<strong>Статус тайтла<\/strong>[\n\s]*<span>([^<>]*)<\/span>").Groups[1].Value;


            string cover_uri =
                Regex.Match(info_section, @"<img src=.([^<>\u0022]+)..*class=.manga__cover.>").Groups[1].Value;

            Uri.TryCreate(cover_uri, UriKind.Absolute, out Uri cover);


            Title title = new Title()
            {
                Name = name,
                Names = "Новелла",
                AlterName = alterName,
                Description = description,
                DateOfRelease = dateOfRelease,
                Status = status,
                Uri = uri,
                Cover = cover,
                Chapters = GetChapters(_web_string)
            };

            return title;
        }

        private List<Chapter> GetChapters(string web_string)
        {
            List<Chapter> result;
            string name;
            string number;
            Uri uri;

            
            var chapter_matches = Regex.Matches(web_string, 
                @"<a\s*class=.link-default.\s*title=([^<>]*)href=.(.*).>([^<]*)(?=<)");

            var chapter_dates_matches = Regex.Matches(web_string, 
                @"class=.chapter-item__date.>\s*([^<>\n]*)");

            if (chapter_matches.Count == 0)
                return default;

            List<string> dates = new List<string>();
            foreach (Match match in chapter_dates_matches)
                dates.Add(match.Groups[1].Value);

            List<(string name, string number, Uri uri)> chapters = new List<(string name, string number, Uri uri)>();

            foreach (Match match in chapter_matches)
            {
                name = Regex.Replace(Regex.Replace(match.Groups[1].Value, @"[\n\u0022]", ""), @"\s+", " ");
                number = Regex.Replace(match.Groups[3].Value, @"[\s\n]+", " ");
                uri = new Uri(match.Groups[2].Value);
                chapters.Add((name, number, uri));
            }
            result = chapters.Zip(dates, (chapters, dates) => 
            new Chapter(chapters.uri) 
            {
                Name = chapters.name,
                Number = chapters.number,
                Date = dates
            }).ToList();

            return result;
        }
    }
}
