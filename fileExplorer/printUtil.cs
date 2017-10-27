﻿using System;
using System.Threading;

namespace fileExplorer
{
    public class printUtil
    {
        #region globalVars
        globals g = new globals();
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
        public string gold = "darkyellow";
        public string wht = "white";
        public string blk = "black";
        public string bar = "_____________________________________________________________________________________________________";
        public string space = @"                                                                                                                                                                                                        ";
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
        public void pause() { write("Press any Key to Continue.", red); Console.ReadKey(); }

        #endregion printTools

        #region Reading

        public ConsoleKeyInfo rk(string _input, string colorOUT, string colorIN)
        {
            ///RESET BACKGROUND TO BLACK IF BLACK FOREGROUND IS CHOSEN AT ANY POINT

            ConsoleKeyInfo returnData;
            if (colorOUT != null) { colorIN = colorIN.ToLower(); }
            pickColor(colorOUT);
            Console.Write(_input);
            if (colorIN != null) { colorIN = colorIN.ToLower(); }
            pickColor(colorIN);
            returnData = Console.ReadKey(true);
            if (returnData.Key == ConsoleKey.LeftArrow || returnData.Key == ConsoleKey.RightArrow) { return returnData; }
            else
            {
                rest(500);
                return returnData;
            }
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
        #endregion printing

        #region printFormatting
        public void topBarPlain(string directory)
        {
            write(space.Substring(0, (101 - g.version.Length)) + bar.Substring(0, g.version.Length + 2) + br, drkGray);
            write(" " + bar.Substring(0, (100 - 2 - g.version.Length)), blue);
            write("| " + g.version + " |" + br, drkGray);
            write("|                                                                                                     |\n\r", blue);
            write(" | ", blue);
            write(string.Format("{0}{1} " + space,space.Substring(0,32), directory).Substring(0, 100), wht);
            write("|", blue);
            write(br + "|" + bar + "|" + br, blue);
        }
        public void topBarWithCurDir(string directory)
        {
            write(space.Substring(0, (101 - g.version.Length)) + bar.Substring(0, g.version.Length + 2) + br, drkGray);
            write(" " + bar.Substring(0, (100 - 2 - g.version.Length)), blue);
            write("| " + g.version + " |" + br, drkGray);
            write("|                                                                                                     |\n\r", blue);
            write(" | ", blue);
            write("Current Location:  ", null);
            if (directory.Length <= 81)
            {
                write(string.Format("{0} " + space, directory).Substring(0, 81), wht);
                write("|", blue);
            }
            else
            {
                write(string.Format("{0}" + space, directory).Substring(0, 80) + " ", wht);
                write("|", blue);
                int dirLength = ((directory.Length - 80) / 100) + 1;
                int subString = 80;
                for (int i = 0; i < dirLength; i++)
                {
                    write(br + "| ", blue);
                    write(string.Format("{0}" + space, directory.Substring(subString)).Substring(0, 100), wht);
                    write("|", blue);
                    subString += 99;
                }
            }
            write(br + "|" + bar + "|" + br + "|", blue);
            write(" Index", wht);
            write(" |  ", drkGray);
            write(string.Format("Directory / File Name" + space).Substring(0, 91), wht);
            write("|", blue);
        }

        public void pagedBottomBar(int curPage, int lastPage)
        {
            int lastPgStrClip = 5 - (lastPage.ToString().Length - 1);
            int curPgStrClip = 71 - (curPage.ToString().Length + lastPage.ToString().Length);
            write(br + "|_ ", blue);
            write("Current Page ", drkGray);
            write("[", blue);
            write(curPage.ToString(), grn);
            write(("]" + bar).Substring(0, curPgStrClip), blue);
            write(" Last Page ", drkGray);
            write("[", blue);
            write(lastPage.ToString(), wht);
            write("]_" + "|", blue);
        }
        public void currentLI(string LInumber, string LIname)
        {
            write(br + "|", blue);
            write(" - " + LInumber + ") -|- " + LIname, grn);
            write("|", blue);
        }
        public void genericLI(string LInumber, string LIname)
        {
            write(br + "|", blue);
            write(" - ", drkGray);
            write(LInumber + ")", wht);
            write(" -|- ", drkGray);
            write(LIname, wht);
            write("|", blue);
        }
        public void LIfiller()
        {
            write(br + "| ", blue);
            write("- X) -|- ", drkGray);
            write(String.Format(space).Substring(0, 91) + '|', blue);
        }
        public void helpMenu()
        {
            resetConsole(0);
            topBarPlain("Welcome to fileExplorer's Help Menu!");
            write(br + "Standard Commands:", grn);
            write(br + "     Enter)   ", drkGray); write("Opens the", wht); write(" Highlighted", ylw); write(" File or Directory. ", wht);
            write(br + "     0-9)     ", drkGray); write("Select an Item by ", wht); write("INDEX", ylw); write(" Number tp Open it.", wht);
            write(br + "     Up/Down) ", drkGray); write("Changes ", wht); write("Highlighted(selected)", ylw); write(" Menu item.", wht);
            write(br + "     O)       ", drkGray); write("Opens ", wht); write(" Current Location", ylw); write(" in a File Explorer.", wht);
            write(br + "     U)       ", drkGray); write("Moves ", wht); write("UP", ylw); write(" a Direrctory", wht);
            write(br + "     H)       ", drkGray); write("Returns ", wht); write("HOME", ylw); write(". ", wht);
            write(br + "Favorites List: ", grn); write("A list of favorite directories that you can jump to quickly.", gray);
            write(br + "     A) ", drkGray); write("Adds Highlighted Item to Favorites. ", wht);
            write(br + "     F) ", drkGray); write("View Favorites.", wht);
            write(br + "     R) ", drkGray); write("Removes Highlighted Favorite.", wht);
            rk(br + "Press Any Key to return to the main menu. ===>", drkGray, drkGray);
        }
        #endregion printFormatting
    }
}