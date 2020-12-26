using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace TitleReader
{
    class Program
    {
        [STAThread]
        public static void Main()
        {
            var app = new App();
            app.InitializeComponent();
            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] Args)
        {
            var host_builder = Host.CreateDefaultBuilder(Args);

            host_builder.UseContentRoot(App.CurrentDirectory);
            host_builder.ConfigureAppConfiguration((host, cfg) =>
            {
                cfg.SetBasePath(App.CurrentDirectory);
                cfg.AddJsonFile("appSettings.json", true, true);
            });

            host_builder.ConfigureServices(App.ConfigureServices);
            return host_builder;
        }
    }
}
