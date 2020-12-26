using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            WebClient _client = new WebClient();

            string str = _client.DownloadString(new Uri(@"https://ranobelib.me/zloklyuceniya-darka-i-sani"));

            Console.WriteLine(str);

            Console.ReadLine();
        }
    }
}
