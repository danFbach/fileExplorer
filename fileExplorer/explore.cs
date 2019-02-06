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
        List<ConsoleKeyInfo> keystrokes = new List<ConsoleKeyInfo>();
        #endregion generalVars

        #region page Creation and Formatting
        public string displayPages(List<pagedData> pages, int currentPage, bool atHome, int curRow, bool atFav)
        {
            int count = 1;
            pagedData thisPage = pages[currentPage];
            string currentDirectory = getCurrentDirec(pages);
            if (atHome == true) { currentDirectory = "Home"; }
            if (atFav == true) { currentDirectory = "Favorites"; }
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

                if (itemName.Length > 85) { itemName = "..." + itemName.Substring(itemName.Length - 85, 85); }
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
            p.commandBar();
            //p.write(p.br + "F1 for HELP/instructions.", p.accentColor1);
            ConsoleKeyInfo k = p.rk("", p.mainColor1, p.accentColor0);
            ///Catch for holding a key down, auto closes after 10 identical keystrokes
            keystrokes.Insert(0, k);
            if (keystrokes.Count >= 20) { catchKeyHold(k); }
            ///end catch
            if (k.Key == ConsoleKey.LeftArrow || k.Key == ConsoleKey.RightArrow || k.Key == ConsoleKey.PageDown || k.Key == ConsoleKey.PageUp) { /*p.write(" ▓ Loading ▓ ", p.accentColor0);*/ return displayPages(pages, leftRight(currentPage, k, pages), atHome, 1, atFav); }
            else if (k.Key == ConsoleKey.UpArrow || k.Key == ConsoleKey.DownArrow) { /*p.write(" ▓ Loading ▓ ", p.accentColor0);*/ return displayPages(pages, currentPage, atHome, upDown(k, curRow, thisPage), atFav); }
            else if (k.Key == ConsoleKey.Enter)
            {
                if (curRow == 0) { curRow = 9; }
                else { curRow -= 1; }
                if (atFav)
                {
                    if (File.Exists(pages[currentPage].dataList[curRow]))
                    {
                        FileInfo newFile = new FileInfo(pages[0].dataList[curRow]);
                        p.write(p.br + "Now opening ", p.accentColor0); p.write(newFile.Name + ".", p.accentColor1);
                        p.write(p.br + "Loading ", p.accentColor1);
                        p.write("▓", p.accentColor2); p.rest(100);
                        for (int i = 0; i < 3; i++) { p.write("|", p.mainColor0); p.rest(25); p.write("▒", p.accentColor1); p.rest(100); p.write("|", p.mainColor0); p.rest(25); p.write("▓", p.accentColor1); p.rest(100); }
                        Process.Start(newFile.FullName);
                        return null;
                    }
                    else if (Directory.Exists(pages[currentPage].dataList[curRow]))
                    {
                        return pages[currentPage].dataList[curRow];
                    }
                    else
                    {
                        return null;
                    }
                }
                else
                {
                    if (File.Exists(pages[0].dataList[curRow]) || Directory.Exists(pages[currentPage].dataList[curRow])) { return pages[currentPage].dataList[curRow]; }
                    else { p.write("There was an Error with your request", p.warningColor); return displayPages(pages, currentPage, false, curRow, false); }
                }
            }
            else
            {
                int dataChoice;
                bool isNumber = int.TryParse(k.KeyChar.ToString(), out dataChoice);
                if (isNumber)
                {
                    if (dataChoice == 0) { dataChoice = 10; }
                    if (dataChoice >= 0 && dataChoice <= pages[currentPage].dataList.Count())
                    {
                        if (atFav)
                        {
                            FileInfo newFile = new FileInfo(pages[currentPage].dataList[dataChoice - 1]);
                            if (newFile.Exists)
                            {
                                p.write(p.br + "Now opening ", p.accentColor0); p.write(newFile.Name + ".", p.accentColor1);
                                p.write(p.br + "Loading ", p.accentColor1);
                                p.write("▓", p.accentColor2); p.rest(100);
                                for (int i = 0; i < 3; i++) { p.write("|", p.mainColor0); p.rest(25); p.write("▒", p.accentColor1); p.rest(100); p.write("|", p.mainColor0); p.rest(25); p.write("▓", p.accentColor1); p.rest(100); }
                                Process.Start(newFile.FullName);
                                return null;
                            }
                            else if (Directory.Exists(pages[currentPage].dataList[dataChoice - 1]))
                            {
                                return pages[currentPage].dataList[dataChoice - 1];
                            }
                            else
                            {
                                return null;
                            }
                        }
                        else { return pages[currentPage].dataList[dataChoice - 1]; }
                    }
                    else
                    {
                        if (dataChoice == 10) { dataChoice = 0; }
                        p.write(" \'" + dataChoice + "\' is not an option.", p.warningColor); p.rest(500);
                        return displayPages(pages, 0, false, curRow, false);
                    }
                }
                else
                {
                    switch (k.Key)
                    {
                        case ConsoleKey.F1:
                            p.helpMenu();
                            return null;
                        case ConsoleKey.U:
                        case ConsoleKey.Backspace:
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
                                if (atHome)
                                {
                                    p.write("In Home Directory. Returning Home.", p.accentColor0);
                                    p.rest(1500);
                                    return null;
                                }
                                else if (atFav)
                                {
                                    p.write("In Favorite Directory. There is no Directory Above This.", p.accentColor0);
                                    p.rest(1500);
                                    pages = showFavorites(pages);
                                    return displayPages(pages, 0, false, 1, true);
                                }
                                else
                                {
                                    p.write(p.br + "Already in a Root Directory. ", p.warningColor);
                                    p.rest(1000);
                                    return currentDirectory;
                                }
                            }
                        case ConsoleKey.A:
                            if (curRow == 0) { curRow = 9; } else { curRow -= 1; }
                            string selItemToAdd = pages[currentPage].dataList[curRow];
                            w.executeFileWriteAdd(selItemToAdd);
                            p.write(p.br + " Successfully Added ", p.accentColor1); p.write(selItemToAdd, p.mainColor0); p.write(" to Favorites List!", p.accentColor1);
                            p.rk(p.br + "Press Any Key to Continue.", p.mainColor0, p.mainColor0);
                            return currentDirectory;
                        case ConsoleKey.F:
                            pages = showFavorites(pages);
                            return displayPages(pages, 0, false, 1, true);
                        case ConsoleKey.R:
                            if (atFav)
                            {
                                if (curRow == 0) { curRow = 9; } else { curRow -= 1; }
                                string selItemToRemove = pages[currentPage].dataList[curRow];
                                w.executeFileWriteRemove(selItemToRemove);
                                p.write(p.br + selItemToRemove + " Has Been Removed.", p.warningColor);
                                p.rk(p.br + "Press Any Key to Continue.", p.mainColor0, p.mainColor0);
                                pages = showFavorites(pages);
                                return displayPages(pages, currentPage, false, curRow, true);
                            }
                            else
                            {
                                p.rk("This command has no function here. Press Any Key to Continue.", p.mainColor0, p.mainColor0);
                                if (atHome)
                                {
                                    return null;
                                }
                                else
                                {
                                    return displayPages(pages, currentPage, false, curRow, true);
                                }
                            }
                        case ConsoleKey.O:
                            if(atFav || atHome)
                            {
                                p.rk("This command has no function here. Press Any Key to Continue.", p.mainColor0, p.mainColor0);
                                if (atHome) { return null; }
                                else if (atFav) { pages = showFavorites(pages); return displayPages(pages, currentPage, false, curRow, true); }
                                else { return null; }
                            }
                            if (Directory.Exists(currentDirectory))
                            {
                                p.write("Now opening ", p.accentColor0); p.write(currentDirectory, p.accentColor1); p.write(" in explorer.", p.mainColor1);
                                p.rest(2500);
                                Process.Start(currentDirectory);
                                return "OpenFolder";
                            }
                            else
                            {
                                p.write("Error.", p.warningColor);
                                return currentDirectory;
                            }
                        case ConsoleKey.Home:
                        case ConsoleKey.H:
                            return null;
                        case ConsoleKey.End:
                        case ConsoleKey.X:
                        case ConsoleKey.Escape:
                            Environment.Exit(0); return null;
                        default:
                            p.write(" " + "\'" + k.KeyChar + "\' is not an option.", p.warningColor); p.rest(500); return displayPages(pages, 0, atHome, curRow, atFav);
                    }
                }
            }
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

        public List<pagedData> showFavorites(List<string> current_pages)
        {
            List<string> raw_favorites = r.retrieveFavorites();
            List<string> items = new List<string>();
            if(raw_favorites.Count() > 0){
                foreach (string f in raw_favorites)
                {
                    Directory d = new DirectoryInfo(f);
                    if (Directory.Exists(d.FullName) || File.Exists(d.FullName))
                    {
                        items.Add(d.FullName);
                    }
                }
                if(items.Count() > 0){
                    items = items.OrderBy(x => x).ToList();
                } else {
                    items = current_pages;
                }
            } else {
                items = current_pages;
            }
            return createPages(items);
        }

        public string getDrives()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            drivesRAW = new List<DirectoryInfo>();
            drives = new List<string>();
            p.resetConsole(0);
            p.write(p.br + "Retrieving all publicly available, local and network drives. Please wait..." + p.br, p.mainColor0);
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
            bool keysMatch = true;
            keystrokes = keystrokes.Take(20).ToList();
            foreach (ConsoleKeyInfo _k in keystrokes)
            {
                if (!_k.Equals(_key))
                {
                    keysMatch = false;
                }
            }

            if (keysMatch)
            {
                p.write(p.br + "The same Key has been pushed over 20 times." + p.br + "The program will now close to prevent opening files by accident.", p.warningColor);
                p.write("5", p.warningColor);
                for (int i = 4; i > 0; i--)
                {
                    p.rest(500);
                    p.write(".", p.mainColor0);
                    p.rest(500);
                    p.write(i.ToString(), p.warningColor);
                }
                p.rest(1500);
                Environment.Exit(0);
            }
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
            if (k.Key == ConsoleKey.LeftArrow || k.Key == ConsoleKey.PageDown) { if (currentPage == 0) { return pages.Count() - 1; } else { return currentPage - 1; } }
            else if (k.Key == ConsoleKey.RightArrow || k.Key == ConsoleKey.PageUp) { if (currentPage == pages.Count() - 1) { return 0; } else { return currentPage + 1; } }
            else { return 0; }
        }

        #endregion utils
    }
}
