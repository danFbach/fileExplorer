using System;

namespace fileExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            globals g = new globals();
            Console.SetWindowSize(g.width, g.height);
            Console.BufferHeight = g.height;
            Console.BufferWidth = g.width;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Title = g.version;
            Console.Clear();
            mainMenu run = new mainMenu();
            run.menu();
        }
    }
}
