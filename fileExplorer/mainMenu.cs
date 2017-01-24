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
            path = getPath(path);
            if (path == null) { Environment.Exit(0); }
            FileInfo newFile = new FileInfo(path);
            if (newFile.Exists == true)
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
                directory = e.displayPages(pages, 0);
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
    }
}
