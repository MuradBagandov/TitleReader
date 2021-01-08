using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Net;

namespace TitleReader.Services
{
    class RanobelibContentParser : Interfaces.IContentChapterParser, IDisposable
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
                throw new Exception("Произошла ошибка при загрузке ресурса");
            }

            var content_matches = Regex.Matches(_web_string, @"((?<=<p>)[^<]+(?=<\/p>))|((?<=>)[^<]+(?=<br>))|((?<=<p><[^<>]+>)[^<>]+(?=<[^<>]+><\/p>))");



            StringBuilder result = new StringBuilder();

            foreach (Match i in content_matches)
                result.Append($"{i.Value.Replace("\n","")}\n\n");

            return result.ToString();

        }

        public async Task<object> GetContentAsync(object p)
        {
            try
            {
                return await Task.Run(() => GetContent(p));
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

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
        ~RanobelibContentParser()
        {
            Dispose(false);
        }
    }
}
