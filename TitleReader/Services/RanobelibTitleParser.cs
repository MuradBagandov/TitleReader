using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;
using TitleReader.Models;

namespace TitleReader.Services
{
    class RanobelibTitleParser : Interfaces.ITitleParser, IDisposable
    {
        private WebClient _client = new WebClient() { Encoding = Encoding.UTF8 };

        public CancellationToken Cancellation { get; set; }

        public RanobelibTitleParser()
        {

            _client.DownloadProgressChanged += (s, e) =>
            {
                if (Cancellation != null)
                    if (Cancellation.IsCancellationRequested)
                    {
                        _client.CancelAsync();
                    }
            };
        }


        public Title GetTitle(object p)
        {
            if (!(p is Uri uri))
                throw new ArgumentException("Неккорректный аргумент");
            
            string _web_string;
            try
            {
                _web_string = _client.DownloadString(uri);

            }
            catch (Exception e)
            {
                throw new Exception("Произошла ошибка при загрузке ресурса");
            }

            return GetTitle(_web_string, uri);
        }

        public async Task<Title> GetTitleAsync(object p)
        {
            if (!(p is Uri uri))
                throw new ArgumentException("Неккорректный аргумент");

            string _web_string = default;
            try
            {
                _web_string = await _client.DownloadStringTaskAsync(uri);
            }
            catch (Exception e)
            {
                if (e.Message == "Запрос был прерван: Запрос отменен.")
                    throw new OperationCanceledException(e.Message);
                else
                    throw new Exception("Произошла ошибка при загрузке ресурса");
            }
           

            return GetTitle(_web_string, uri);
        }

        


        public object GetContent(object p)
        {
            if (!(p is Uri uri))
                throw new ArgumentException();

            string _web_string;
            try
            {
                _web_string = _client.DownloadString(uri);
            }
            catch
            {
                throw new Exception("Произошла ошибка при загрузке ресурса");
            }

            return GetChapterContent(_web_string);

        }

       

        public async Task<object> GetContentAsync(object p)
        {
            if (!(p is Uri uri))
                throw new ArgumentException();

            string _web_string;
            try
            {
                _web_string = await _client.DownloadStringTaskAsync(uri);
            }
            catch (Exception e)
            {
                if (e.Message == "Запрос был прерван: Запрос отменен.")
                    throw new OperationCanceledException(e.Message);
                else
                    throw new Exception("Произошла ошибка при загрузке ресурса");
            }

            return GetChapterContent(_web_string);
        }


        private Title GetTitle(string page, Uri uri)
        {
            string name, alterName, description, dateOfRelease, statusTranslate, status;
            List<string> authors = new List<string>();
            List<string> genres = new List<string>();

            string info_section = Regex.Match(page, @"<section([\s\S]+)<\/section>").Value;

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

            statusTranslate = Regex.Match(info_section,
                @"<strong>Статус перевода<\/strong>[\n\s]*<span>([^<>]*)<\/span>").Groups[1].Value;


            var authors_text = Regex.Match(info_section, @"<strong>Автор<\/strong>(?:[^<>]*<a[^<>]*>[^<>]*<\/a>)*").Value;
            var autors_matches = Regex.Matches(authors_text, @">([^<>]+)<\/a>");


            foreach (Match item in autors_matches)
                authors.Add(item.Groups[1].Value);

            var genres_text = Regex.Match(info_section, @"<strong>Жанры<\/strong>(?:[^<>]*<a[^<>]*>[^<>]*<\/a>)*").Value;
            var genres_matches = Regex.Matches(genres_text, @">([^<>]+)<\/a>");


            foreach (Match item in genres_matches)
                genres.Add(item.Groups[1].Value);


            string cover_uri =
                Regex.Match(info_section, @"<img src=.([^<>\u0022]+)..*class=.manga__cover.>").Groups[1].Value;

            Uri.TryCreate(cover_uri, UriKind.Absolute, out Uri cover);


            Title title = new Title()
            {
                Uri = uri,
                Name = name,
                AlterName = alterName,
                Names = "Новелла",
                Authors = authors,
                Status = status,
                StatusTranslate = statusTranslate,
                Genres = genres,
                Description = description,
                DateOfRelease = dateOfRelease,
                Cover = cover,
                Chapters = GetChapters(page)
            };

            return title;
        }

        private LinkedList<Chapter> GetChapters(string page)
        {
            LinkedList<Chapter> result;
            string name;
            string number;
            Uri uri;


            var chapter_matches = Regex.Matches(page,
                @"<a\s*class=.link-default.\s*title=([^<>]*)href=.(.*).>([^<]*)(?=<)");

            var chapter_dates_matches = Regex.Matches(page,
                @"class=.chapter-item__date.>\s*([^<>\n]*)");

            if (chapter_matches.Count == 0)
                return default;

            List<string> dates = new List<string>();
            foreach (Match match in chapter_dates_matches)
                dates.Add(match.Groups[1].Value);

            LinkedList<(string name, string number, Uri uri)> chapters = new LinkedList<(string name, string number, Uri uri)>();

            foreach (Match match in chapter_matches)
            {
                name = Regex.Replace(Regex.Replace(match.Groups[1].Value, @"[\n\u0022]", ""), @"\s+", " ");
                number = Regex.Replace(match.Groups[3].Value, @"[\s\n]+", " ");
                uri = new Uri(match.Groups[2].Value);
                chapters.AddLast((name, number, uri));
            }
            result = new LinkedList<Chapter>(chapters.Zip(dates, (chapters, dates) =>
            new Chapter(chapters.uri)
            {
                Name = chapters.name,
                Number = chapters.number,
                Date = dates
            }));

            return result;
        }

        private static string GetChapterContent(string page)
        {
            var content_matches = Regex.Matches(page, @"((?<=<p>)[^<]+(?=<\/p>))|((?<=>)[^<]+(?=<br>))|((?<=<p><[^<>]+>)[^<>]+(?=<[^<>]+><\/p>))");

            StringBuilder result = new StringBuilder();

            foreach (Match i in content_matches)
                result.Append($"{i.Value.Replace("\n", "")}\n\n");

            return result.ToString();
        }

        #region Dispose
        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {

                }
                _client.Dispose();
                disposed = true;
            }
        }
        ~RanobelibTitleParser()
        {
            Dispose(false);
        } 
        #endregion
    }
}
