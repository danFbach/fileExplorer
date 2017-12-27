using System;
using System.Threading;

namespace fileExplorer
{
    public class printUtil
    {
        #region globalVars
        globals g = new globals();
        public string accentColor0 = "magenta";
        public string accentColor1 = "cyan";
        public string accentColor2 = "darkcyan";
        public string mainColor0 = "white";
        public string mainColor1 = "darkgray";
        public string warningColor = "red";
        public string background0 = "white";
        public string background1 = "black";
        public string bar = "_____________________________________________________________________________________________________";
        public string space = @"                                                                                                                                                                                                        ";
        public string br = "\n\r ";
        /// Color Picker
        public void pickColor(string color)
        {
            switch (color)
            {
                /// NORMAL COLORS
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
                /// DARK COLORS
                case "darkblue":
                    Console.ForegroundColor = ConsoleColor.DarkBlue;
                    break;
                case "darkcyan":
                    Console.ForegroundColor = ConsoleColor.DarkCyan;
                    break;
                case "darkgray":
                    Console.ForegroundColor = ConsoleColor.DarkGray;
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
                    Console.BackgroundColor = ConsoleColor.White;
                    break;
                default:
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    break;
            }
        }
        public void pickBackground(string color)
        {
            switch (color)
            {
                /// NORMAL COLORS
                case "blue":
                    Console.BackgroundColor = ConsoleColor.Blue;
                    break;
                case "cyan":
                    Console.BackgroundColor = ConsoleColor.Cyan;
                    break;
                case "gray":
                    Console.BackgroundColor = ConsoleColor.Gray;
                    break;
                case "green":
                    Console.BackgroundColor = ConsoleColor.Green;
                    break;
                case "magenta":
                    Console.BackgroundColor = ConsoleColor.Magenta;
                    break;
                case "red":
                    Console.BackgroundColor = ConsoleColor.Red;
                    break;
                case "yellow":
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    break;
                /// DARK COLORS
                case "darkblue":
                    Console.BackgroundColor = ConsoleColor.DarkBlue;
                    break;
                case "darkcyan":
                    Console.BackgroundColor = ConsoleColor.DarkCyan;
                    break;
                case "darkgray":
                    Console.BackgroundColor = ConsoleColor.DarkGray;
                    break;
                case "darkgreen":
                    Console.BackgroundColor = ConsoleColor.DarkGreen;
                    break;
                case "darkmagenta":
                    Console.BackgroundColor = ConsoleColor.DarkMagenta;
                    break;
                case "darkred":
                    Console.BackgroundColor = ConsoleColor.DarkRed;
                    break;
                case "darkyellow":
                    Console.BackgroundColor = ConsoleColor.DarkYellow;
                    break;
                case "white":
                    Console.BackgroundColor = ConsoleColor.White;
                    break;
                case "black":
                    Console.BackgroundColor = ConsoleColor.Black;
                    break;
                default:
                    Console.BackgroundColor = ConsoleColor.Yellow;
                    break;
            }
        }
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
            Console.CursorVisible = false;
        }
        public void pause() { write("Press any Key to Continue.", warningColor); Console.ReadKey(); }

        #endregion printTools

        #region Reading

        public ConsoleKeyInfo rk(string _input, string colorOUT, string colorIN)
        {
            ///RESET BACKGROUND TO BLACK IF BLACK FOREGROUND IS CHOSEN AT ANY POINT

            ConsoleKeyInfo returnData;
            write(_input, colorIN);
            returnData = Console.ReadKey(true);
            if (returnData.Key == ConsoleKey.LeftArrow || returnData.Key == ConsoleKey.RightArrow) { return returnData; }
            else{return returnData;}
        }

        #endregion Reading

        #region printing
        public void write(string _input, string color)
        {
            ///RESET BACKGROUND TO BLACK IF BLACK FOREGROUND IS CHOSEN AT ANY POINT
            Console.BackgroundColor = ConsoleColor.Black;
            if (color != null)
            {
                color = color.ToLower();
            }
            pickColor(color);
            Console.Write(_input);
        }
        public void write(string _input, string color, string backgroundColor)
        {
            ///RESET BACKGROUND TO BLACK IF BLACK FOREGROUND IS CHOSEN AT ANY POINT
            if(backgroundColor != null)
            {
                backgroundColor = backgroundColor.ToLower();
            }
            else
            {
                backgroundColor = "black";
            }
            if (color != null)
            {
                color = color.ToLower();
            }
            else
            {
                color = mainColor0;
            }
            pickColor(color);
            pickBackground(backgroundColor);
            Console.Write(_input);
        }
        #endregion printing

        #region printFormatting
        public void versionBar()
        {
            g.updateVersion();
            write(space.Substring(0, (101 - g._v.Length)) + bar.Substring(0, g._v.Length + 2) + br, mainColor1);
            write(" " + bar.Substring(0, (100 - 2 - g._v.Length)), accentColor0); write("| ", mainColor1); write(g._v, accentColor1); write(" |" + br, mainColor1);
            write("|                                                                                                     |\n\r", accentColor0);
            write(" | ", accentColor0);
        }
        public void topBarPlain(string directory)
        {
            versionBar();
            write(string.Format("{0}{1} " + space,space.Substring(0,32), directory).Substring(0, 100), mainColor0);
            write("|", accentColor0);
            write(br + "|" + bar + "|" + br, accentColor0);
        }
        public void topBarWithCurDir(string directory)
        {
            versionBar();
            write("Current Location:  ", accentColor0);
            if (directory.Length <= 81)
            {
                write(string.Format("{0} " + space, directory).Substring(0, 81), mainColor0);
                write("|", accentColor0);
            }
            else
            {
                write(string.Format("{0}" + space, directory).Substring(0, 80) + " ", mainColor0);
                write("|", accentColor0);
                int dirLength = ((directory.Length - 80) / 100) + 1;
                int subString = 80;
                for (int i = 0; i < dirLength; i++)
                {
                    write(br + "| ", accentColor0);
                    write(string.Format("{0}" + space, directory.Substring(subString)).Substring(0, 100), mainColor0);
                    write("|", accentColor0);
                    subString += 99;
                }
            }
            write(br + "|" + bar + "|" + br + "|", accentColor0);
            write(" Index", mainColor0);
            write(" |  ", mainColor1);
            write(string.Format("Directory / File Name" + space).Substring(0, 91), mainColor0);
            write("|", accentColor0);
        }

        public void pagedBottomBar(int curPage, int lastPage)
        {
            int lastPgStrClip = 5 - (lastPage.ToString().Length - 1);
            int curPgStrClip = 69 - (curPage.ToString().Length + lastPage.ToString().Length);
            write(br + "|_ ", accentColor0);
            write("Current Page ", mainColor1);
            write("[", accentColor0);
            write(curPage.ToString(), mainColor0);
            write(("]" + bar).Substring(0, curPgStrClip), accentColor0);
            write(" Total Pages ", mainColor1);
            write("[", accentColor0);
            write(lastPage.ToString(), mainColor0);
            write("]_" + "|", accentColor0);
        }
        public void currentLI(string LInumber, string LIname)
        {
            write(br + "|", accentColor0);
            write(" - " + LInumber + ") -|- " + LIname, accentColor1);
            write("|", accentColor0);
        }
        public void genericLI(string LInumber, string LIname)
        {
            write(br + "|", accentColor0);
            write(" - ", mainColor1);
            write(LInumber + ")", mainColor0);
            write(" -|- ", mainColor1);
            write(LIname, mainColor0);
            write("|", accentColor0);
        }
        public void LIfiller()
        {
            write(br + "| ", accentColor0);
            write("- X) -|- ", mainColor1);
            write(String.Format(space).Substring(0, 91) + '|', accentColor0);
        }
        public void commandBar()
        {
            write(br + "Arrows", accentColor0);
            write("Change Item/Page", background1, accentColor0);
            write(" Enter", accentColor0);
            write("Open Current Item/Directory", background1, accentColor0);
            write(" 0-9", accentColor0);
            write("Open Item/Page", background1, accentColor0);
            write(" U", accentColor0);
            write("Up Directory", background1, accentColor0);
            write(" F1", accentColor0);
            write("Help/Commands", background1, accentColor0);
        }
        public void helpMenu()
        {
            resetConsole(0);
            Console.SetWindowSize(g.width, g.heightTall);
            Console.BufferHeight = g.heightTall;
            Console.BufferWidth = g.width;
            topBarPlain("Welcome to fileExplorer's Help Menu!");
            write(br + "Directional/Selection Commands:", accentColor1);
            write(br + "     Page Down & Left Arrow |", mainColor1); write("Previous page", mainColor0);
            write(br + "     Page Up & Right Arrow  |", mainColor1); write("Next page", mainColor0);
            write(br + "     Up & Down Arrows       |", mainColor1); write("Changes ", mainColor0); write("Highlighted(selected)", accentColor0); write(" Menu item.", mainColor0);
            write(br + "Standard Commands:", accentColor1);
            write(br + "     Enter   |", mainColor1); write("Opens the", mainColor0); write(" Selected ", accentColor0); write("File or Directory. ", mainColor0);
            write(br + "     0-9     |", mainColor1); write("Select an Item by ", mainColor0); write("INDEX", accentColor0); write(" Number tp Open it.", mainColor0);
            write(br + "     O       |", mainColor1); write("Opens ", mainColor0); write(" Current Location", accentColor0); write(" in a File Explorer.", mainColor0);
            write(br + "     U       |", mainColor1); write("Moves ", mainColor0); write("UP", accentColor0); write(" a Direrctory", mainColor0);
            write(br + "     Home    |", mainColor1); write("Returns ", mainColor0); write("HOME", accentColor0); write(". ", mainColor0);
            write(br + "     End     |", mainColor1); write("Exits Explorer. ", mainColor0);
            write(br + "Favorites List: ", accentColor1); write("A list of favorite directories that you can jump to quickly.", accentColor0);
            write(br + "     A       |", mainColor1); write("Adds ", mainColor0); write("Selected", accentColor0); write(" Item to Favorites. ", mainColor0);
            write(br + "     F       |", mainColor1); write("View", accentColor0); write(" Favorites.", mainColor0);
            write(br + "     R       |", mainColor1); write("Removes ", mainColor0); write("Highlighted", accentColor0); write(" Favorite.", mainColor0);
            rk(br + "Press Any Key to return to the main menu. ===>", mainColor1, mainColor1);
            Console.SetWindowSize(g.width, g.height);
            Console.BufferHeight = g.height;
            Console.BufferWidth = g.width;
        }
        #endregion printFormatting
    }
}