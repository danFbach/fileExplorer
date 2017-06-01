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
        printUtil p = new printUtil();
        public List<DirectoryInfo> drivesRAW = new List<DirectoryInfo>();
        public List<DirectoryInfo> myDrives = new List<DirectoryInfo>(new[] { new DirectoryInfo(@"Z:\Main (A)\Shared Videos"), new DirectoryInfo(@"Z:\Main (A)\Shared Documents"), new DirectoryInfo(@"Y:\Main (B)"), new DirectoryInfo(@"Y:\Main (B)\Music"), new DirectoryInfo(@"C:\Users\Dan DCC") });
        public List<string> drives = new List<string>();
        #endregion generalVars

        #region page Creation and Formatting
        public string displayPages(List<pagedData> pages, int currentPage, bool drives, bool goToExplorer, int curRow)
        {
            int count = 1;
            pagedData thisPage = pages[currentPage];
            string currentDirectory = getCurrentDirec(pages);
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
                if (!drives && nameData.Count() <= 1) { itemName = (itemName + p.space).Substring(0, 89); }
                else if (!drives && nameData.Count() > 1) { itemName = (itemName.Split('\\').Last() + p.space).Substring(0, 89); }
                else { itemName =(itemName + p.space).Substring(0, 89); }
                if (count == curRow && goToExplorer == false)
                {
                    //CURRENT LIST ITEM
                    p.write(p.br + "|", p.blue);
                    p.write(" - " + _count + ") --|-- " + itemName, p.grn);
                    p.write("|", p.blue);
                }
                else
                {
                    //ALL OTHER LIST ITEMS
                    p.write(p.br + "|", p.blue);
                    p.write(" - ", p.drkGray);
                    p.write(_count + ")", p.wht);
                    p.write(" --|-- ", p.drkGray);
                    p.write(itemName, p.wht);
                    p.write("|", p.blue);
                }
                if (item == pages[currentPage].dataList.Last())
                {
                    if (count == 0) { count = 10; }
                    for (int i = count; i < 10; i++)
                    {
                        p.write(p.br + "| ", p.blue);
                        p.write("- X) --|-- ", p.drkGray);
                        p.write(String.Format(p.space).Substring(0, 89) + '|', p.blue);
                    }
                }
                count += 1;
            }
            p.pagedBottomBar(currentPage, pages.Count() - 1);
            if (goToExplorer == false)
            {
                p.write(p.br + "Left Arrow - ", p.drkGray);
                p.write("Previous page", p.wht);
                p.write(p.space.Substring(0, 54), p.wht);
                p.write("Right Arrow - ", p.drkGray);
                p.write("Next page", p.wht);
                p.write(p.br + p.br + "Up/Down Arrow - ", p.drkGray);
                p.write("select a menu item. ", p.wht);
                p.write(p.br + "Enter - ", p.drkGray);
                p.write("Open file or enter folder. ", p.wht);
                p.write(p.br + "P or Backspace -  ", p.drkGray);
                p.write("Move up a directory. ", p.wht);
                p.write(p.br + p.br + "H) ", p.drkGray);
                p.write("Return to home directory. ", p.wht);
                p.write(p.br + "O) ", p.drkGray);
                p.write("Open active folder or file. ", p.wht);
            }
            p.write(p.br + "X) ", p.drkGray);
            p.write("to Exit Explorer. ", p.wht);
            p.write(p.br + "OR", p.drkGray);
            p.write(" Select an item by its ", p.wht);
            ConsoleKeyInfo k = p.rk("index number: ", p.drkGray, p.gray);
            p.write(" ▓ Loading ▓ " , p.ylw);
            if (goToExplorer == false)
            {
                if (k.Key == ConsoleKey.LeftArrow || k.Key == ConsoleKey.RightArrow) { return displayPages(pages, leftRight(currentPage, k, pages), drives, goToExplorer, 1); }
                else if (k.Key == ConsoleKey.UpArrow || k.Key == ConsoleKey.DownArrow) { return displayPages(pages, currentPage, drives, goToExplorer, upDown(k, curRow, thisPage)); }
                else if (k.Key == ConsoleKey.Enter && goToExplorer == false)
                {
                    if (curRow == 0) { curRow = 9; }
                    else { curRow -= 1; }
                    if (File.Exists(pages[0].dataList[curRow]) || Directory.Exists(pages[currentPage].dataList[curRow])) { return pages[currentPage].dataList[curRow]; }
                    else { p.write("There was an Error with your request", p.red); return displayPages(pages, currentPage, drives, goToExplorer, curRow); }
                }
                else
                {
                    int dataChoice;
                    bool isNumber = int.TryParse(k.KeyChar.ToString(), out dataChoice);
                    if (isNumber)
                    {
                        if(dataChoice == 0) { dataChoice = 10; }
                        if (dataChoice >= 0 && dataChoice <= pages[currentPage].dataList.Count()) { return pages[currentPage].dataList[dataChoice - 1]; }
                        else { if (dataChoice == 10) { dataChoice = 0; } p.write(" \'" + dataChoice + "\' is not an option.", p.red); Thread.Sleep(850); return displayPages(pages, 0, drives, goToExplorer, curRow); }
                    }
                    else
                    {
                        switch (k.Key)
                        {
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
                                p.write(" " + "\'" + k.KeyChar + "\' is not an option.", p.red); p.rest(850); return displayPages(pages, 0, drives, goToExplorer, curRow);
                        }
                    }
                }
            }
            else
            {
                int folderChoice;
                bool isNumber = int.TryParse(k.KeyChar.ToString(), out folderChoice);
                if (isNumber)
                {
                    if (folderChoice > 0 && folderChoice <= 9) { folderChoice -= 1; }
                    else if (folderChoice == 0) { folderChoice = 9; }
                    string path = pages[currentPage].dataList[folderChoice];
                    if (Directory.Exists(path)) { p.write(p.br + "Now opening ", p.blue); p.write(pages[currentPage].dataList[folderChoice], p.grn); p.write(" in explorer.", p.drkGray); p.rest(2500); Process.Start(pages[currentPage].dataList[folderChoice]); return "OpenFolder"; }
                    else { p.write("Error.", p.red); return displayPages(pages, currentPage, drives, goToExplorer, curRow); }
                }
                else
                {
                    if (k.KeyChar == 'x' || k.KeyChar == 'X') { Environment.Exit(0); return null; }
                    else { p.write("Invalid selection.", p.red); return displayPages(pages, currentPage, drives, goToExplorer, curRow); }
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
            return displayPages(pages, 0, true, false, 1);
        }
        #endregion get data

        #region utils

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