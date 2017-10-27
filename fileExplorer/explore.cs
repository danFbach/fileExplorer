using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace fileExplorer
{
    public class explore
    {
        #region generalVars
        write w = new write();
        read r = new read();
        printUtil p = new printUtil();
        public List<DirectoryInfo> drivesRAW = new List<DirectoryInfo>();
        public List<DirectoryInfo> myDrives = new List<DirectoryInfo>(new[] { new DirectoryInfo(@"Z:\Main (A)\Shared Videos"), new DirectoryInfo(@"Z:\Main (A)\Shared Documents"), new DirectoryInfo(@"X:\Main (B)"), new DirectoryInfo(@"X:\Main (B)\Music"), new DirectoryInfo(@"C:\Users\Dan DCC") });
        public List<string> drives = new List<string>();
        public List<ConsoleKeyInfo> keystrokes = new List<ConsoleKeyInfo>();
        bool keysMatch;
        #endregion generalVars

        #region page Creation and Formatting
        public string displayPages(List<pagedData> pages, int currentPage, bool atHome, int curRow, bool atFav)
        {
            int count = 1;
            pagedData thisPage = pages[currentPage];
            string currentDirectory = getCurrentDirec(pages);
            if (atHome == true) { currentDirectory = "Home"; }
            if(atFav == true) { currentDirectory = "Favorites"; }
            int pCheck = currentDirectory.Split('\\').Count();
            p.resetConsole(0);
            p.topBarWithCurDir(currentDirectory);
            foreach (string item in pages[currentPage].dataList)
            {
                if (count == 10) { count = 0; }
                string _count = count.ToString();
                string[] nameData = item.Split('\\');
                string itemName = "";
                itemName = item;
                if (!atHome && nameData.Count() <= 1) { itemName = (itemName + p.space).Substring(0, 91); }
                else if (!atHome && nameData.Count() > 1 && !atFav) { itemName = (itemName.Split('\\').Last() + p.space).Substring(0, 91); }
                else { itemName = (itemName + p.space).Substring(0, 91); }
                ///CURRENT LIST ITEM
                if (count == curRow) { p.currentLI(_count, itemName); }
                ///ALL OTHER LIST ITEMS
                else { p.genericLI(_count, itemName); }
                ///FILLER LIST ITEMS
                if (item == pages[currentPage].dataList.Last())
                {
                    if (count == 0) { count = 10; }
                    for (int i = count; i < 10; i++) { p.LIfiller(); }
                }
                count += 1;
            }
            p.pagedBottomBar(currentPage, pages.Count() - 1);
            ///left/right arrow instructions
            p.write(p.br + "Left Arrow - ", p.drkGray); p.write("Previous page", p.wht); p.write(p.space.Substring(0, 54), p.wht); p.write("Right Arrow - ", p.drkGray); p.write("Next page", p.wht);
            p.write(p.br + "X) ", p.drkGray); p.write("to Exit Explorer. ", p.wht);
            p.write(p.br + "F1 for HELP/instructions.", p.grn);
            ConsoleKeyInfo k = p.rk(p.br + "===> ", p.drkGray, p.gray);
            ///Catch for holding a key down, auto closes after 10 identical keystrokes
            keystrokes.Insert(0, k);
            if (keystrokes.Count >= 10) { catchKeyHold(k); }
            if (k.Key == ConsoleKey.LeftArrow || k.Key == ConsoleKey.RightArrow) { p.write(" ▓ Loading ▓ ", p.ylw); return displayPages(pages, leftRight(currentPage, k, pages), atHome, 1, atFav); }
            else if (k.Key == ConsoleKey.UpArrow || k.Key == ConsoleKey.DownArrow) { p.write(" ▓ Loading ▓ ", p.ylw); return displayPages(pages, currentPage, atHome, upDown(k, curRow, thisPage), atFav); }
            else if (k.Key == ConsoleKey.Enter)
            {
                if (atFav && File.Exists(pages[0].dataList[curRow]))
                {
                    if (curRow == 0) { curRow = 9; }
                    else { curRow -= 1; }
                    return pages[0].dataList[curRow];
                    //Process.Start(pages[0].dataList[curRow]);
                    //pages = showFavorites();
                    //return displayPages(pages, currentPage, false, curRow, true);
                }
                else
                {
                    if (curRow == 0) { curRow = 9; }
                    else { curRow -= 1; }
                    if (File.Exists(pages[0].dataList[curRow]) || Directory.Exists(pages[currentPage].dataList[curRow])) { return pages[currentPage].dataList[curRow]; }
                    else { p.write("There was an Error with your request", p.red); return displayPages(pages, currentPage, false, curRow, false); }
                }
            }
            else
            {
                int dataChoice;
                bool isNumber = int.TryParse(k.KeyChar.ToString(), out dataChoice);
                if (isNumber)
                {
                    if (dataChoice == 0) { dataChoice = 10; }
                    if (dataChoice >= 0 && dataChoice <= pages[currentPage].dataList.Count()) { return pages[currentPage].dataList[dataChoice - 1]; }
                    else { if (dataChoice == 10) { dataChoice = 0; } p.write(" \'" + dataChoice + "\' is not an option.", p.red); p.rest(500); return displayPages(pages, 0, false, curRow, false); }
                }
                else
                {
                    switch (k.Key)
                    {
                        case ConsoleKey.F1:
                            p.helpMenu();
                            return null;
                        case ConsoleKey.U:
                            string newDIR = "";
                            List<string> dirRAW = currentDirectory.Split('\\').ToList();
                            if (dirRAW.Count > 2)
                            {
                                dirRAW.RemoveRange(dirRAW.Count - 2, 2);
                                foreach (string d in dirRAW) { newDIR += d + "\\"; }
                                return newDIR;
                            }
                            else
                            {
                                if (atHome == true)
                                {
                                    p.write("In Home Directory. Returning Home.", p.ylw);
                                    p.rest(1000);
                                    return null;
                                }
                                else
                                {
                                    p.write(p.br + "Already in a Root Directory. ", p.red);
                                    p.rest(1000);
                                    return currentDirectory;
                                }
                            }
                        case ConsoleKey.A:
                            if (curRow == 0) { curRow = 9; } else { curRow -= 1; }
                            string selItemToAdd = pages[currentPage].dataList[curRow];
                            w.executeFileWriteAdd(selItemToAdd);
                            p.write(p.br + " Successfully Added ", p.grn); p.write(selItemToAdd, p.wht); p.write(" to Favorites List!", p.grn);
                            p.rk(p.br + "Press Any Key to Continue.", p.wht, p.wht);
                            return currentDirectory;
                        case ConsoleKey.F:
                            pages = showFavorites();
                            return displayPages(pages, currentPage, false, curRow, true);
                        case ConsoleKey.R:
                            if (atFav)
                            {
                                if (curRow == 0) { curRow = 9; } else { curRow -= 1; }
                                string selItemToRemove = pages[currentPage].dataList[curRow];
                                w.executeFileWriteRemove(selItemToRemove);
                                p.write(p.br + selItemToRemove + " Has Been Removed.", p.red);
                                p.rk(p.br + "Press Any Key to Continue.", p.wht, p.wht);
                                return null;
                            }
                            else
                            {
                                return null;
                            }
                        case ConsoleKey.O:
                            if (Directory.Exists(currentDirectory))
                            {
                                p.write("Now opening ", p.blue); p.write(currentDirectory, p.grn); p.write(" in explorer.", p.drkGray);
                                p.rest(2500);
                                Process.Start(currentDirectory);
                                return "OpenFolder";
                            }
                            else
                            {
                                p.write("Error.", p.red);
                                return currentDirectory;
                            }
                        case ConsoleKey.H:
                            return null;
                        case ConsoleKey.Backspace:
                        case ConsoleKey.P:
                            if (pCheck > 2)
                            {
                                string[] previousDirectoryRAW = pages[currentPage].dataList[0].Split('\\');
                                string previousDirectory = "";
                                for (int i = 0; i < previousDirectoryRAW.Count() - 2; i++) { previousDirectory += previousDirectoryRAW[i] + @"\"; }
                                return previousDirectory;
                            }
                            else
                            {
                                p.resetConsole(0);
                                return getDrives();
                            }

                        case ConsoleKey.X:
                            Environment.Exit(0); return null;

                        default:
                            p.write(" " + "\'" + k.KeyChar + "\' is not an option.", p.red); p.rest(500); return displayPages(pages, 0, atHome, curRow, atFav);
                    }
                }
            }
            //else
            //{
            //        int folderChoice;
            //        bool isNumber = int.TryParse(k.KeyChar.ToString(), out folderChoice);
            //        if (isNumber)
            //        {
            //            if (folderChoice > 0 && folderChoice <= 9) { folderChoice -= 1; }
            //            else if (folderChoice == 0) { folderChoice = 9; }
            //            string path = pages[currentPage].dataList[folderChoice];
            //            if (Directory.Exists(path)) { p.write(p.br + "Now opening ", p.blue); p.write(pages[currentPage].dataList[folderChoice], p.grn); p.write(" in explorer.", p.drkGray); p.rest(2500); Process.Start(pages[currentPage].dataList[folderChoice]); return "OpenFolder"; }
            //            else { p.write("Error.", p.red); return displayPages(pages, currentPage, drives, curRow); }
            //        }
            //        else
            //        {
            //            if (k.KeyChar == 'x' || k.KeyChar == 'X') { Environment.Exit(0); return null; }
            //            else { p.write("Invalid selection.", p.red); return displayPages(pages, currentPage, drives, curRow); }
            //        }
            //    }
        }
        public List<pagedData> createPages(List<string> _data)
        {
            int count = 0;
            int pageCount = 0;
            List<pagedData> dataPages = new List<pagedData>();
            pagedData dataPage = new pagedData();
            dataPage.dataList = new List<string>();
            dataPage.pageNumber = pageCount;
            foreach (string d in _data)
            {
                count += 1;
                dataPage.dataList.Add(d);
                if (count == 10 || _data.IndexOf(d) == _data.Count() - 1)
                {
                    count = 0;
                    pageCount += 1;
                    dataPages.Add(dataPage);
                    dataPage = new pagedData();
                    dataPage.dataList = new List<string>();
                    dataPage.pageNumber = pageCount;
                }
            }
            return dataPages;
        }
        #endregion page Creation and Formatting

        #region get data
        public List<string> getFolderPack(string directory)
        {
            List<string> directoryPaths = new List<string>();
            DirectoryInfo thisDirec = new DirectoryInfo(directory);
            List<DirectoryInfo> direcPack = new List<DirectoryInfo>();
            DirectoryInfo[] direcPackRAW = { };
            List<FileInfo> filePack = new List<FileInfo>();
            FileInfo[] filePackRAW = { };
            try { direcPackRAW = thisDirec.GetDirectories().Where(x => !x.Attributes.HasFlag(FileAttributes.Hidden)).ToArray(); }
            catch { }
            try { filePackRAW = thisDirec.GetFiles().Where(x => !x.Attributes.HasFlag(FileAttributes.Hidden)).ToArray(); }
            catch { }
            if (direcPackRAW.Count() > 0)
            {
                direcPack = direcPackRAW.OrderBy(x => x.FullName).ToList();
                foreach (DirectoryInfo path in direcPack)
                {
                    directoryPaths.Add(path.FullName);
                }
            }
            if (filePackRAW.Count() > 0)
            {
                filePack = filePackRAW.OrderBy(x => x.Name).ToList();
                foreach (FileInfo item in filePack)
                {
                    directoryPaths.Add(item.FullName);
                }
            }
            return directoryPaths;
        }
        public string getCurrentDirec(List<pagedData> pages)
        {
            string[] direcRAW = pages[0].dataList[0].Split('\\');
            string direc = "";
            for (int i = 0; i < direcRAW.Count() - 1; i++)
            {
                direc += direcRAW[i] + @"\";
            }
            return direc;
        }

        public List<pagedData> showFavorites()
        {
            List<string> favRaw = r.retrieveFavorites();
            List<DirectoryInfo> favsPack = new List<DirectoryInfo>();
            List<string> favorites = new List<string>();
            foreach (string f in favRaw)
            {
                favsPack.Add(new DirectoryInfo(f));
            }
            favsPack = favsPack.OrderBy(x => x.FullName).ToList();
            foreach (DirectoryInfo f in favsPack)
            {
                if (Directory.Exists(f.FullName) || File.Exists(f.FullName))
                {
                    favorites.Add(f.FullName);
                }
            }
            return createPages(favorites);
        }
        public string getDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            drivesRAW = new List<DirectoryInfo>();
            drives = new List<string>();
            p.resetConsole(0);
            p.write(p.br + "Retrieving all publicly available, local and network drives. Please wait..." + p.br, p.wht);
            foreach (DriveInfo drive in allDrives)
            {
                if (drive.IsReady)
                {
                    if (drive.DriveFormat == "NTFS")
                    {
                        DirectoryInfo newDirectory = new DirectoryInfo(drive.Name);
                        drivesRAW.Add(newDirectory);
                    }
                }
            }
            foreach (DirectoryInfo directory in myDrives)
            {
                if (Directory.Exists(directory.FullName))
                {
                    drivesRAW.Add(directory);
                }
            }
            drivesRAW = drivesRAW.OrderBy(x => x.FullName).ToList();
            foreach (DirectoryInfo drive in drivesRAW)
            {
                drives.Add(drive.FullName);
            }
            List<pagedData> pages = createPages(drives);
            return displayPages(pages, 0, true, 1, false);
        }
        #endregion get data

        #region utils
        public void catchKeyHold(ConsoleKeyInfo _key)
        {
            keysMatch = true;
            keystrokes = keystrokes.Take(10).ToList();
            foreach (ConsoleKeyInfo _k in keystrokes)
            {
                if (!_k.Equals(_key))
                {
                    keysMatch = false; break;
                }
            }
            if (keysMatch) { Environment.Exit(0); }
        }
        public int upDown(ConsoleKeyInfo k, int currentBar, pagedData page)
        {
            int itemCount = page.dataList.Count();
            if (k.Key == ConsoleKey.UpArrow)
            {
                if (itemCount == 10)
                {
                    if (currentBar == 1) { return 0; }
                    else if (currentBar == 0) { return 9; }
                    else { return currentBar - 1; }
                }
                else
                {
                    if (currentBar == 1) { return itemCount; }
                    else { return currentBar - 1; }
                }
            }
            else if (k.Key == ConsoleKey.DownArrow)
            {
                if (itemCount == 10)
                {
                    if (currentBar == itemCount - 1) { return 0; }
                    else { return currentBar + 1; }
                }
                else
                {
                    if (currentBar < itemCount) { return currentBar + 1; }
                    else { return 1; }
                }
            }
            else
            {
                return currentBar;
            }
        }
        public int leftRight(int currentPage, ConsoleKeyInfo k, List<pagedData> pages)
        {
            if (k.Key == ConsoleKey.LeftArrow) { if (currentPage == 0) { return pages.Count() - 1; } else { return currentPage - 1; } }
            else if (k.Key == ConsoleKey.RightArrow) { if (currentPage == pages.Count() - 1) { return 0; } else { return currentPage + 1; } }
            else { return 0; }
        }

        #endregion utils
    }
}