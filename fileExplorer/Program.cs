using System;

namespace fileExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            globals g = new globals();
            g.updateVersion();
            Console.CursorVisible = false;
            Console.SetWindowSize(g.width, g.height);
            Console.BufferHeight = g.height;
            Console.BufferWidth = g.width;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Title = g._v;
            Console.Clear();
            mainMenu run = new mainMenu();
            run.menu();
        }
    }
}
