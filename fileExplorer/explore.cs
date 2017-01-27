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
        public string displayPages(List<pagedData> pages, int currentPage, bool drives, bool goToExplorer)
        {
            int count = 1;
            string currentDirectory = getCurrentDirec(pages);
            int pCheck = currentDirectory.Split('\\').Count();
            p.resetConsole(0);
            p.topBarBlank(currentDirectory);
            foreach (string item in pages[currentPage].dataList)
            {
                if (count == 10)
                {
                    count = 0;
                }
                string _count = count + ")--- ";
                string[] nameData = item.Split('\\');
                string itemName = "";
                if (drives == false)
                {
                    if (nameData.Count() <= 2)
                    {
                        itemName = item;
                        itemName = String.Format(itemName + p.space).Substring(0, 90) + '|';
                    }
                    else if (nameData.Count() > 2)
                    {
                        itemName = String.Format(item.Split('\\')[nameData.Count() - 1] + p.space).Substring(0, 90) + '|';
                    }
                }
                else
                {
                    itemName = String.Format(item + p.space).Substring(0, 90) + '|';
                }
                p.write(p.br + "| ", p.wht);
                p.write(_count, p.grn);
                p.write("|", p.wht);
                p.write(" - ", p.grn);
                p.write(itemName, p.wht);
                if (item == pages[currentPage].dataList.Last())
                {
                    if(count == 0) { count = 10; }
                    for (int i = count; i < 10; i++)
                    {
                        p.write(p.br + "| ", p.wht);
                        p.write("X)--- ", p.grn);
                        p.write("|", p.wht);
                        p.write(" - ", p.grn);
                        p.write(String.Format(p.space).Substring(0, 90) + '|', p.wht);
                    }
                }
                count += 1;
            }
            p.pagedBottomBar(currentPage, pages.Count() - 1);
            if (goToExplorer == false)
            {
                p.write(p.br + "Left Arrow ", p.grn);
                p.write("- Previous page", p.wht);
                p.write(p.space.Substring(0, 54), p.wht);
                p.write("Right Arrow ", p.grn);
                p.write("- Next page", p.wht);
                p.write(p.br + p.br + "P) ", p.grn);
                p.write("for previous directory. ", p.wht);
                p.write(p.br + "O) ", p.grn);
                p.write("Open a folder in Explorer. ", p.wht);
                p.write(p.br + "N) ", p.grn);
                p.write("Open Current folder in Explorer. ", p.wht);
            }
            p.write(p.br + "X ", p.grn);
            p.write("to Exit Explorer. ", p.wht);
            p.write(p.br + "Select an item by its ", p.wht);
            ConsoleKeyInfo thisKey = p.rk("index number: ", p.grn, p.cyan);
            if (goToExplorer == false)
            {
                if (thisKey.Key == ConsoleKey.LeftArrow)
                {
                    if (currentPage == 0)
                    {
                        return displayPages(pages, 0, drives, goToExplorer);
                    }
                    else
                    {
                        return displayPages(pages, currentPage - 1, drives, goToExplorer);
                    }
                }
                else if (thisKey.Key == ConsoleKey.RightArrow)
                {
                    if (currentPage == pages.Count() - 1)
                    {
                        return displayPages(pages, 0, drives, goToExplorer);
                    }
                    else
                    {
                        return displayPages(pages, currentPage + 1, drives, goToExplorer);
                    }
                }
                else
                {
                    int dataChoice;
                    bool isNumber = int.TryParse(thisKey.KeyChar.ToString(), out dataChoice);
                    if (isNumber)
                    {
                        if (dataChoice >= 0 && dataChoice <= pages[currentPage].dataList.Count())
                        {
                            if (dataChoice == 0)
                            {
                                dataChoice = 10;
                            }
                            return pages[currentPage].dataList[dataChoice - 1];
                        }
                        else
                        {
                            p.write(p.br + dataChoice + " is not a valid selection.", p.red);
                            Thread.Sleep(1500);
                            return displayPages(pages, 0, drives, goToExplorer);
                        }
                    }
                    else
                    {
                        if (thisKey.KeyChar == 'p' || thisKey.KeyChar == 'P')
                        {
                            if (pCheck > 2)
                            {
                                string[] previousDirectoryRAW = pages[currentPage].dataList[0].Split('\\');
                                string previousDirectory = "";
                                for (int i = 0; i < previousDirectoryRAW.Count() - 2; i++)
                                {
                                    previousDirectory += previousDirectoryRAW[i] + @"\";
                                }
                                return previousDirectory;
                            }
                            else
                            {
                                p.resetConsole(0);
                                return getDrive();
                            }
                        }
                        else if (thisKey.KeyChar == 'n' || thisKey.KeyChar == 'N')
                        {
                            string[] currentDirRAW = pages[0].dataList[0].Split('\\');
                            string currentDir = "";
                            for(int i =0;i<currentDirRAW.Count() - 1; i++)
                            {
                                currentDir += currentDirRAW[i] + '\\';
                            }
                            if (Directory.Exists(currentDir))
                            {
                                Process.Start(currentDir);
                                return "OpenFolder";
                            }
                            else
                            {
                                p.write("Error.", p.red);
                                return currentDir;
                            }
                        }
                        else if (thisKey.KeyChar == 'x' || thisKey.KeyChar == 'X')
                        {
                            Environment.Exit(0); return null;
                        }
                        else if (thisKey.KeyChar == 'o' || thisKey.KeyChar == 'O')
                        {
                            string folder = displayPages(pages, currentPage, drives, true);
                            return folder;
                        }
                        else
                        {
                            p.write(p.br + "\"" + thisKey.KeyChar + "\" is not a valid selection.", p.red);
                            p.rest(1500);
                            return displayPages(pages, 0, drives, goToExplorer);
                        }
                    }
                }
            }
            else
            {
                int folderChoice;
                bool isNumber = int.TryParse(thisKey.KeyChar.ToString(), out folderChoice);
                if (isNumber)
                {
                    if (folderChoice > 0 && folderChoice <= 9)
                    {
                        folderChoice -= 1;
                    }
                    else if (folderChoice == 0)
                    {
                        folderChoice = 9;
                    }
                    string path = pages[currentPage].dataList[folderChoice];
                    if (Directory.Exists(path))
                    {
                        p.write(p.br + "Now opening " + pages[currentPage].dataList[folderChoice] + " in explorer.", p.grn);
                        p.rest(2500);
                        Process.Start(pages[currentPage].dataList[folderChoice]);
                        return "OpenFolder";
                    }
                    else
                    {
                        p.write("Error.", p.red);
                        return displayPages(pages, currentPage, drives, goToExplorer);
                    }
                }
                else
                {
                    if (thisKey.KeyChar == 'x' || thisKey.KeyChar == 'X') { Environment.Exit(0); return null; }
                    else
                    {
                        p.write("Invalid selection.", p.red);
                        return displayPages(pages, currentPage, drives, goToExplorer);

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

        public string getDrive()
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            drivesRAW = new List<DirectoryInfo>();
            drives = new List<string>();
            p.resetConsole(0);
            p.write(p.br + "Please select the drive you would like to use." + p.br, p.wht);
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
            return displayPages(pages, 0, true, false);
        }
        #endregion get data
    }
}