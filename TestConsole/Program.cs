using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Windows;

namespace TestConsole
{
    class Program
    {
        static void Main(string[] args)
        {

            MessageBox.Show("Ошибка", "Ошибка чтения данных", MessageBoxButton.OK, MessageBoxImage.Error);
            Console.Read();
        }
        
    }

    public class person
    {
        public person(string name)
        {
            this.name = name;
        }

        public string name { get; set; }
    }
}
