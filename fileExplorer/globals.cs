using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace fileExplorer
{
    class globals
    {
        Version v = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        public int height = 18;
        public int heightTall = 22;
        public int width = 105;
        public string _v = "";
        public void updateVersion()
        {
            _v = "fileExplorer - v" + v.ToString();
        }
    }
}