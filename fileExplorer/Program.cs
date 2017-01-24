using System;

namespace fileExplorer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(105, Console.WindowHeight);
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Title = "File Explorer.";
            Console.Clear();
            mainMenu run = new mainMenu();
            run.menu();
        }
    }
}
