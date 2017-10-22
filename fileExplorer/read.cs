using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace fileExplorer
{
    class read
    {
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\fileExplorerFavorites\favorites.txt";

        public List<string> retrieveFavorites()
        {
            List<string> favorites = new List<string>();
            if (File.Exists(filePath))
            {
                string[] favoritesRaw = File.ReadAllLines(filePath);
                foreach(string f in favoritesRaw)
                {
                    favorites.Add(f);
                }
            }
            return favorites;
        }
    }
}
