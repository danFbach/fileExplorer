using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;


namespace fileExplorer
{
    class write
    {
        string filePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "\\fileExplorerFavorites\\";
        string _file = "favorites.txt";

        public void executeFileWriteAdd(string newFavorite)
        {
            bool add = true;
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
                if (!File.Exists(filePath + _file))
                {
                    createIfNew();
                }
            }
            List<string> favoritesData = retrieveExisting();
            foreach(string f in favoritesData)
            {
                if(f.ToLower() == newFavorite.ToLower())
                {
                    add = false;
                }
            }
            if (add)
            {
                favoritesData.Add(newFavorite);
            }
            writeToFile(favoritesData);

        }
        public void executeFileWriteRemove(string remove)
        {
            List<string> favoritesData = retrieveExisting();
            favoritesData = removeFavorite(favoritesData, remove);
            writeToFile(favoritesData);
        }
        public void createIfNew()
        {
            StreamWriter writer = new StreamWriter(filePath + _file);
            writer.Close();
        }
        public List<string> retrieveExisting()
        {
            List<string> existing = new List<string>();
            string[] existingRaw = File.ReadAllLines(filePath + _file);
            foreach(string dataLine in existingRaw)
            {
                existing.Add(dataLine);
            }
             return existing;
        }

        public List<string> removeFavorite(List<string> favorites, string toRemove)
        {
            int indx = -1;
            foreach(string f in favorites)
            {
                if(f == toRemove)
                {
                    indx = favorites.IndexOf(f);
                }
            }
            if (indx >= 0)
            {
                favorites.RemoveAt(indx);
            }
            return favorites;
        }
        public void writeToFile(List<string> data)
        {
            using (StreamWriter writer = new StreamWriter(filePath + _file, false))
            {
                foreach(string fav in data)
                {
                    writer.WriteLine(fav);
                }
            }
        }
    }
}
