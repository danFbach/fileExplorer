using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(105, Console.WindowHeight);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Clear();
            mainMenu run = new mainMenu();
            run.menu();
        }
    }
}
