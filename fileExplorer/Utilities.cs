using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileExplorer
{
    class Utilities
    {
        #region Variables
        printUtil p = new printUtil();
        explore e = new explore();
        #endregion Variables

        #region pathFileRetrieval
        public string getPath(string directory)
        {
            List<string> pathPack = e.getFolderPack(directory);
            List<pagedData> pages = e.createPages(pathPack);
            if(pages.Count == 0)
            {
                string[] previousDirectoryRAW = directory.Split('\\');
                string previousDirectory = "";
                for (int i = 0; i < previousDirectoryRAW.Count() - 1; i++) { previousDirectory += previousDirectoryRAW[i] + @"\"; }
                directory = previousDirectory;
                p.write(p.br + "This directory does not contain any files or folders." + p.br, p.warningColor);
                p.rk("Press Any Key to Continue.", p.mainColor0, p.mainColor0);
            }
            else
            {
                directory = e.displayPages(pages, 0, false, 1, false);
            }
            return directory;
        }

        public string getFileName(string rawFilepath, int location)
        {
            /// C:/Users/Dan DCC/Documents
            /// Location - 0 is filename or last piece of filepath | >= 1 counts back on filepath
            if (!String.IsNullOrEmpty(rawFilepath))
            {
                string newFilename = "";
                string[] pathPack = rawFilepath.Split('\\');
                int locSelector = 1;
                locSelector += location;
                /// retrieve filename from path.
                if (locSelector < (pathPack.Count() - 1)) { newFilename = pathPack.ElementAt(pathPack.Length - locSelector); }
                else { newFilename = pathPack.Last(); }
                /// return filename.
                if (!String.IsNullOrEmpty(newFilename)) { return newFilename; }
                else { return ""; }
            }
            else { return " "; }
        }
        #endregion pathFileRetrieval
        #region errorCatch
        #endregion errorCatch
    }
}
