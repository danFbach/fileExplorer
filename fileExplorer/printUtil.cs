using System;
using System.Threading;

namespace fileExplorer
{
    public class printUtil
    {
        #region globalVars

        public string blue = "blue";
        public string drkblue = "darkblue";
        public string cyan = "cyan";
        public string drkcyan = "darkcyan";
        public string gray = "gray";
        public string drkGray = "darkgray";
        public string grn = "green";
        public string drkGrn = "darkgreen";
        public string mgnta = "magenta";
        public string drkmgnta = "darkmagenta";
        public string red = "red";
        public string drkRed = "darkred";
        public string ylw = "yellow";
        public string drkYlw = "darkyellow";
        public string wht = "white";
        public string blk = "black";
        public string space = @"                                                                                                    ";
        public string br = "\n\r ";

        #endregion globalVars

        #region printTools
        public void rest(int rest)
        {
            Thread.Sleep(rest);
        }
        public void resetConsole(int rest)
        {
            Thread.Sleep(rest);
            Console.Clear();
        }
        public void proceed()
        {
            Console.ReadKey();
        }

        #endregion printTools

        #region printing
        public void write(string _input, string color)
        {
            ///RESET BACKGROUND TO BLACK IF BLACK FOREGROUND IS CHOSEN AT ANY POINT
            //Console.BackgroundColor = ConsoleColor.Black;
            if (color != null)
            {
                color = color.ToLower();
            }
            pickColor(color);
            Console.Write(_input);
        }

        public string rl(string _input, string colorOUT, string colorIN)
        {
            ///RESET BACKGROUND TO BLACK IF BLACK FOREGROUND IS CHOSEN AT ANY POINT
            //Console.BackgroundColor = ConsoleColor.Black;
            string returnData;
            if (colorOUT != null)
            {
                colorIN = colorIN.ToLower();
            }
            pickColor(colorOUT);
            Console.Write(_input);
            if (colorIN != null)
            {
                colorIN = colorIN.ToLower();
            }
            pickColor(colorIN);
            returnData = Console.ReadLine();
            return returnData;
        }
        public ConsoleKeyInfo rk(string _input, string colorOUT, string colorIN)
        {
            ///RESET BACKGROUND TO BLACK IF BLACK FOREGROUND IS CHOSEN AT ANY POINT
            //Console.BackgroundColor = ConsoleColor.Black;
            ConsoleKeyInfo returnData;
            if (colorOUT != null)
            {
                colorIN = colorIN.ToLower();
            }
            pickColor(colorOUT);
            Console.Write(_input);
            if (colorIN != null)
            {
                colorIN = colorIN.ToLower();
            }
            pickColor(colorIN);
            returnData = Console.ReadKey();
            if (returnData.Key == ConsoleKey.LeftArrow || returnData.Key == ConsoleKey.RightArrow) { return returnData; }
            else
            {
                rest(500);
                return returnData;
            }
        }
        public void pickColor(string color)
        {
            switch (color)
            {
                ///NORMAL COLORS
                case "blue":
                    Console.ForegroundColor = ConsoleColor.Blue;
                    break;
                case "cyan":
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    break;
                case "gray":
                    Console.ForegroundColor = ConsoleColor.Gray;
                    break;
                case "green":
                    Console.ForegroundColor = ConsoleColor.Green;
                    break;
                case "magenta":
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    break;
                case "red":
                    Console.ForegroundColor = ConsoleColor.Red;
                    break;
                case "yellow":
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
                ///DARK COLORS
                case "darkblue":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case "darkcyan":
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case "darkgray":
                    Console.ForegroundColor = ConsoleColor.DarkGray;
                    Console.BackgroundColor = ConsoleColor.White;
                    break;
                case "darkgreen":
                    Console.ForegroundColor = ConsoleColor.DarkGreen;
                    break;
                case "darkmagenta":
                    Console.ForegroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "darkred":
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                    break;
                case "darkyellow":
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    break;
                case "white":
                    Console.ForegroundColor = ConsoleColor.White;
                    break;
                case "black":
                    Console.ForegroundColor = ConsoleColor.Black;
                    break;
                ///DEFAULT COLOR
                default:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
        }

        #endregion printing

        #region printFormatting
        public void topBar(string title) { write(string.Format("\n\r______________________________________________{0}_____________________________________________\n\r| Index | Name                                                                       | Length         |", title), wht); }
        public void topBarBlank(string directory)
        {
            write(br + " _____________________________________________________________________________________________________ ", wht);
            write(br + "|                                                                                                     |\n\r", wht);
            if (directory.Length <= 81)
            {
                write(string.Format(" | Current Directory: {0} " + space, directory).Substring(0, 103) + "|", wht);
            }
            else
            {
                write(string.Format(" | Current Directory: {0} " + space, directory).Substring(0, 102) + " |", wht);
                int dirLength = ((directory.Length - 80) / 100) + 1;
                int subString = 80;
                for(int i = 0;i < dirLength; i++)
                {
                    write(string.Format("\n\r | {0}" + space, directory.Substring(subString)).Substring(0, 104) + " |", wht);
                    subString += 99;
                }
            }
            write("\n\r |_____________________________________________________________________________________________________|\n\r |", wht);
            write(" Index ", cyan);
            write("|", wht);
            write(" Directory / File Name ", cyan);
            write("                                                                      |", wht);
        }
        public void emptyNoSides() { write("\n\r  _____________________________________________________________________________________________________  ", wht); }
        public void bottomBar() { write("\n\r|______________________________________________________________________________________________________|\n\r", wht); }
        public void pagedBottomBar(int curPage)
        {
            write(br + "|____________________________________________Page [", wht);
            write(curPage.ToString(), cyan);
            write("]_________________________________________________|", wht);
        }
        #endregion printFormatting
    }
}
