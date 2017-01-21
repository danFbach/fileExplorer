using System;
using System.IO;
using System.Text;
using System.Linq;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;

namespace fileExplorer
{
    public class explore
    {
        printUtil p = new printUtil();
        public string displayPages(List<pagedData> pages, int currentPage)
        {
            int count = 0;
            p.resetConsole();
            p.topBarBlank(getCurrentDirec(pages));
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
            ConsoleKeyInfo key = p.rk("Or, select by index number: ", p.wht, p.grn);
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
                    else { p.write(p.br + _itemSelect + " is not a valid selection.", p.wht); return displayPages(pages, currentPage); }
                }
                else
                {
                    if (key.KeyChar == 'p' || key.KeyChar == 'P')
                    {
                        string[] previousDirectoryRAW = pages[currentPage].dataList[0].Split('\\');
                        string previousDirectory = "";
                        for(int i = 0;i < previousDirectoryRAW.Count() - 2; i++)
                        {
                            previousDirectory += previousDirectoryRAW[i] + @"\";
                        }
                        return previousDirectory;
                    }
                    if (key.KeyChar == 'x' || key.KeyChar == 'X') { return null; }
                    p.write(p.br + _itemSelect + " is not a valid selection.", p.wht);
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
        public List<string> getFolderPack(string directory)
        {
            List<string> directoryPaths = new List<string>();
            //string[] directoriesRAW =  Directory.GetDirectories(directory);
            //directoriesRAW = directoriesRAW.OrderBy(x => x.Split('\\').Last()).ToArray();
            DirectoryInfo thisDirec = new DirectoryInfo(directory);
            DirectoryInfo[] direcPack = { };
            FileInfo[] filePack = { };
            try
            {
                direcPack = thisDirec.GetDirectories();
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
            for(int i = 0; i < direcRAW.Count() - 1; i++)
            {
                direc += direcRAW[i] + @"\";
            }
            return direc;
        } 
    }
}