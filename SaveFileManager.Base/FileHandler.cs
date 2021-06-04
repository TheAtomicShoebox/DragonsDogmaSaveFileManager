using System.IO;

namespace SaveFileManager.Base
{
    public static class FileHandler
    {
        public static SaveItems GetSaveItemsByFilePath(string filepath)
        {
            var fileAsString = File.ReadAllText(filepath);
            var results = new SaveItems(fileAsString);

            return results;
        }
    }
}
