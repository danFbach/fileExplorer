using System;

namespace fileExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(105, 27);
            Console.BufferHeight = 27;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Title = "File Explorer.";
            Console.Clear();
            mainMenu run = new mainMenu();
            run.menu();
        }
    }
}
