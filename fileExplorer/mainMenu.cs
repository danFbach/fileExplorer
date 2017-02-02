using System;
using System.IO;
using System.Linq;
using System.Diagnostics;
using System.Collections.Generic;

namespace fileExplorer
{
    public class mainMenu
    {
        #region globalVars
        printUtil p = new printUtil();
        explore e = new explore();
        #endregion

        #region setupFuncs
        public void menu()
        {
            loopThis(e.getDrive());
        }
        public void loopThis(string path)
        {
            if (path == "OpenFolder")
            {
                menu();
            }
            path = getPath(path);
            if (path == "OpenFolder")
            {
                menu();
            }
            FileInfo newFile = new FileInfo(path);
            if (newFile.Exists)
            {
                p.write(p.br + "Now opening " + newFile.Name + ".", p.grn);
                p.rest(2500);
                Process.Start(newFile.FullName);
                menu();
            }
            else
            {
                loopThis(path);
            }

        }
        public string getPath(string directory)
        {
            List<string> pathPack = e.getFolderPack(directory);
            List<pagedData> pages = e.createPages(pathPack);
            if(pages.Count() > 0)
            {
                directory = e.displayPages(pages, 0, false, false, 1);
            }
            else
            {
                string[] previousDirectoryRAW = directory.Split('\\');
                string previousDirectory = "";
                for(int i = 0;i < previousDirectoryRAW.Count() - 1; i++) { previousDirectory += previousDirectoryRAW[i] + @"\"; }
                directory = previousDirectory;
                p.write(p.br + "This directory does not contain anything or you do not have access to it.", p.red);
                p.rest(2000);
            }
            return directory;
        }
        #endregion setupFuncs

        #region renamingUtil


        public void filter()
        {
            string dir = p.rl(p.br + "Enter directory for files to be renamed. (Replacing periods with spaces.)" + p.br + "Directory: ", p.wht, p.grn);
            string[] _filePackRAW = Directory.GetFiles(dir);
            List<FileInfo> _filePack = new List<FileInfo>();
            List<FileInfo> _newFilePack = new List<FileInfo>();
            foreach (string f in _filePackRAW)
            {
                if (File.Exists(f))
                {
                    _filePack.Add(new FileInfo(f));
                }
            }
            string seasonNum = _filePack[0].DirectoryName.Split('\\').Last().Split(' ').Last();
            for (int i = 0; i < _filePack.Count(); i++)
            {
                string[] fBrk = _filePack[i].Name.Split('.');
                string cleanName = "Psych " + _filePack[i].Name;


                //cleanName += "Psych S";
                //if(Convert.ToInt16(seasonNum) < 10) { cleanName += "0"; }
                //cleanName += seasonNum + "E";
                //string oldName = _filePack[i].Name.Split('.').First();
                //string epNum = oldName.Split(' ').Last();
                //if(Convert.ToInt16(epNum) < 10) { cleanName += "0"; }
                //cleanName += epNum;
                //cleanName += "." + fBrk.Last();
                File.Move(_filePack[i].FullName, _filePack[i].Directory + "\\" + cleanName);
            }
            p.rk("pausing, press any key to continue.", p.grn, p.blk);
            filter();
        }

        #endregion renamingUtil
    }
}
