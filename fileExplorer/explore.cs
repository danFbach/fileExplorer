using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Collections.Generic;

namespace fileExplorer
{
    public class explore
    {
        #region generalVars
        printUtil p = new printUtil();
        public List<string> drives = new List<string>();
        #endregion generalVars

        #region page Creation and Formatting
        public string displayPages(List<pagedData> pages, int currentPage)
        {
            int count = 0;
            string currentDirectory = getCurrentDirec(pages);
            int pCheck = currentDirectory.Split('\\').Count();
            p.resetConsole(0);
            p.topBarBlank(currentDirectory);
            foreach (string item in pages[currentPage].dataList)
            {
                string _count = String.Format(count + ")        ").Substring(0, 6);
                string name = String.Format("| " + item.Split('\\').Last() + p.space).Substring(0, 94) + '|';
                p.write(p.br + "| ", p.wht);
                p.write(_count, p.grn);
                p.write(name, p.wht);
                count += 1;
            }
            int prevPage = 0;
            int nextPage = 0;
            if (currentPage == 0) { prevPage = 0; }
            else { prevPage = currentPage - 1; }
            if (currentPage == pages.Count() - 1) { nextPage = currentPage; }
            else { nextPage = currentPage + 1; }
            p.pagedBottomBar(currentPage, prevPage, nextPage);
            p.write(p.br + "Left arrow ", p.grn);
            p.write("for previous page, ", p.wht);
            p.write("Right arrow ", p.grn);
            p.write("for next page, ", p.wht);
            p.write(p.br + "P ", p.grn);
            p.write("for previous directory. ", p.wht);
            p.write("X ", p.grn);
            p.write("to Exit Explorer. ", p.wht);
            p.write(p.br + "Or, select an item by its ", p.wht);
            ConsoleKeyInfo key = p.rk("index number: ", p.grn, p.cyan);
            if (key.Key == ConsoleKey.LeftArrow) { if (currentPage == 0) { return displayPages(pages, 0); } else { return displayPages(pages, currentPage - 1); } }
            else if (key.Key == ConsoleKey.RightArrow) { if (currentPage == pages.Count() - 1) { return displayPages(pages, currentPage); } else { return displayPages(pages, currentPage + 1); } }
            else
            {
                int _itemSelect;
                bool isANumber = int.TryParse(key.KeyChar.ToString(), out _itemSelect);
                if (isANumber)
                {
                    if (_itemSelect >= 0 && _itemSelect <= pages[currentPage].dataList.Count() - 1)
                    {
                        return pages[currentPage].dataList[_itemSelect];
                    }
                    else { p.write(p.br + _itemSelect + " is not a valid selection.", p.red); Thread.Sleep(1500); return displayPages(pages, currentPage); }
                }
                else
                {
                    if (key.KeyChar == 'p' || key.KeyChar == 'P')
                    {
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
                            return getDrive();
                        }
                    }
                    if (key.KeyChar == 'x' || key.KeyChar == 'X') { return null; }
                    p.write(p.br + "\"" + key.KeyChar + "\" is not a valid selection.", p.red);
                    p.rest(1500);
                    return displayPages(pages, currentPage);
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
            foreach (string _series in _data)
            {
                count += 1;
                dataPage.dataList.Add(_series);
                if (count == 10 || _data.IndexOf(_series) == _data.Count() - 1)
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
            DirectoryInfo[] direcPack = { };
            FileInfo[] filePack = { };
            try
            {
                direcPack = thisDirec.GetDirectories().Where(x => !x.Attributes.HasFlag(FileAttributes.Hidden)).ToArray();
            }
            catch
            {

            }
            try
            {
                filePack = thisDirec.GetFiles();
            }
            catch
            {

            }
            if (direcPack.Count() > 0)
            {
                foreach (var path in direcPack)
                {
                    directoryPaths.Add(path.FullName);
                }
            }
            if (filePack.Count() > 0)
            {
                var filteredFiles = filePack.Where(x => !x.Attributes.HasFlag(FileAttributes.Hidden));
                filteredFiles = filteredFiles.OrderBy(x => x.Name).ToArray();
                foreach (var item in filteredFiles)
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
            int count = 0;
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            p.resetConsole(0);
            p.write(p.br + "Please select the drive you would like to use." + p.br, p.wht);
            foreach (DriveInfo drive in allDrives)
            {
                if (drive.IsReady)
                {
                    if (drive.DriveFormat == "NTFS")
                    {
                        p.write(p.br + count + ") ", p.grn);
                        p.write(drive.Name, p.wht);
                        drives.Add(drive.Name);
                        count += 1;
                    }
                }
            }
            drives.Add("C:\\Users\\Dan DCC\\");
            p.write(p.br + count + ") ", p.grn);
            p.write(drives[count], p.wht);
            p.write(p.br + "X)", p.grn);
            p.write(" Exit Application.", p.wht);
            ConsoleKeyInfo key = p.rk(p.br + p.br + "Select Index: ", p.wht, p.cyan);
            int selDrive;
            bool result = int.TryParse(key.KeyChar.ToString(), out selDrive);
            if (result)
            {
                if (selDrive < allDrives.Count() && selDrive >= 0)
                {
                    return drives[selDrive];
                }
                else
                {
                    p.write(p.br + "There is no drive at index number " + selDrive + ".", p.red);
                    p.resetConsole(1500);
                    return getDrive();
                }
            }
            else
            {
                if(key.KeyChar == 'x' || key.KeyChar == 'X')
                {
                    Environment.Exit(0);
                }
                p.write(p.br + "\"" + key.KeyChar + "\" is not a valid selection.", p.red);
                p.resetConsole(1500);
                return getDrive();
            }
        }
        #endregion get data
    }
}