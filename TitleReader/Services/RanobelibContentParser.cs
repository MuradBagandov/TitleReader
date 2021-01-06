using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;

namespace TitleReader.Services
{
    class RanobelibContentParser : Interfaces.IContentChapterParser
    { 
        private WebClient _client = new WebClient() { Encoding = Encoding.UTF8 };

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
                throw new Exception("Ошибка подключения");
            }

            var content_matches = Regex.Matches(_web_string, @"((?<=<p>)[^<]+(?=<\/p>))|((?<=>)[^<]+(?=<br>))");



            StringBuilder result = new StringBuilder();

            foreach (Match i in content_matches)
                result.Append($"{i.Value.Replace("\n","")}\n\n");

            return result.ToString();

        }

        public async Task<object> GetContentAsync(object p)
        {
            return await Task.Run(() => GetContent(p));
        }
    }
}
