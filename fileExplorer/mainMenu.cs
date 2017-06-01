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
        Utilities u = new Utilities();
        #endregion globalVars

        #region setupFuncs
        public void menu()
        {
            loopThis(e.getDrives());
        }
        public void loopThis(string path)
        {
            if (path == "OpenFolder" || path == null)
            {
                menu();
            }
            path = u.getPath(path);
            if (path == "OpenFolder" || path == null)
            {
                menu();
            }
            FileInfo newFile = new FileInfo(path);
            if (newFile.Exists)
            {
                p.write(p.br + "Now opening " , p.blue); p.write(newFile.Name + ".", p.grn);
                p.write(p.br + "Loading ",p.grn);
                p.write("▓", p.drkGrn); p.rest(100);
                for (int i = 0; i < 3; i++) { p.write("|", p.wht); p.rest(25); p.write("▒", p.grn); p.rest(100); p.write("|", p.wht); p.rest(25); p.write("▓", p.grn); p.rest(100); }
                Process.Start(newFile.FullName);
                menu();
            }
            else if(path == null) { menu(); }
            else { loopThis(path); }
        }

        #endregion miscUtils

        #region renamingUtil

        /// NOT BEING USED YET

        //    public void filter()
        //    {
        //        string dir = p.rl(p.br + "Enter directory for files to be renamed. (Replacing periods with spaces.)" + p.br + "Directory: ", p.wht, p.blue);
        //        string[] _filePackRAW = Directory.GetFiles(dir);
        //        List<FileInfo> _filePack = new List<FileInfo>();
        //        List<FileInfo> _newFilePack = new List<FileInfo>();
        //        foreach (string f in _filePackRAW)
        //        {
        //            if (File.Exists(f))
        //            {
        //                _filePack.Add(new FileInfo(f));
        //            }
        //        }
        //        string seasonNum = _filePack[0].DirectoryName.Split('\\').Last().Split(' ').Last();
        //        for (int i = 0; i < _filePack.Count(); i++)
        //        {
        //            string[] fBrk = _filePack[i].Name.Split('.');
        //            string cleanName = "Psych " + _filePack[i].Name;    
        //            File.Move(_filePack[i].FullName, _filePack[i].Directory + "\\" + cleanName);
        //        }
        //        p.rk("pausing, press any key to continue.", p.blue, p.blk);
        //        filter();
        //    }
        #endregion renamingUtil
    }
}
