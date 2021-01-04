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
            LinkedList<person> persons = new LinkedList<person>();
            persons.AddLast(new person("tom"));

            persons.AddLast(new person("toni"));

            persons.AddFirst(new person("Jojo"));
            
            LinkedListNode <person> ps = persons.First;

            while (ps != null)
            {
                Console.WriteLine(ps.Value.name);
                ps = ps.Next;
            }

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
