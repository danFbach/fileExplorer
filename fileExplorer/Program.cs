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
            mainMenu run = new mainMenu();
            run.menu();
        }
    }
}
