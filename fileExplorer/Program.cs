using System;

namespace fileExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            globals g = new globals();
            Console.SetWindowSize(105, 28);
            Console.BufferHeight = 28;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Title = "File Explorer - " + g.version;
            Console.Clear();
            mainMenu run = new mainMenu();
            run.menu();
        }
    }
}
