using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            if (File.Exists(@"C:\Users\Admin\Desktop\lexical_sample_file.txt"))
            {
                string file = File.ReadAllText(@"C:\Users\Admin\Desktop\lexical_sample_file.txt");


                LA lx = new LA();
                lx.WordBreak(file);
                lx.printTokens();
                Console.ReadKey();

            }
        }
    }
}
