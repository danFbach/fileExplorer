using System;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace fileExplorer
{
    public class mainMenu
    {
        /// GLOBAL VARS
        public string basePath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
        printUtil p = new printUtil();
        explore e = new explore();

        public void menu()
        {
            loopThis(basePath);
        }
        public void loopThis(string path)
        {
            path = getPath(path);
            if (path == null) { Environment.Exit(0); }
            FileInfo newFile = new FileInfo(path);
            if (newFile.Exists == true)
            {
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
                for(int i = 0;i < previousDirectoryRAW.Count() - 1; i++)
                {
                    previousDirectory += previousDirectoryRAW[i] + @"\";
                }
                directory = previousDirectory;
                p.write(p.br + "This directory does not contain anything or you do not have access to it.", p.red);
                p.rest(2000);
            }
            return directory;
        }
    }
}
