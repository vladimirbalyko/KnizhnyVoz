using System.IO;

namespace KnizhnyVoz.Services
{
    interface IFileSystemService
    {
        string CreateDirectory(string path);
        void CreateFile(string fileName, string text);
        void CreateFile(string path, string fileName, string text);
        string RemoveInvalidChars(string filename);
        string ReplaceInvalidChars(string filename);
    }

    public class FileSystemService: IFileSystemService
    {
        private readonly string _rootPath;

        public FileSystemService(string path)
        {
            CreateDictionaryIfNotExist(path);

            _rootPath = path;
        }

        public string CreateDirectory(string path)
        {
            var folderPath = $"{_rootPath}/{RemoveInvalidChars(path)}";
            return CreateDictionaryIfNotExist(folderPath) ? folderPath : null;
        }

        public void CreateFile(string fileName, string text)
        {
            CreateFile(_rootPath, fileName, text);
        }

        public void CreateFile(string path, string fileName, string text)
        {
            string filePath = $"{path}/{fileName}.txt";
            if (!File.Exists(filePath))
            {
                using StreamWriter sw = File.CreateText(filePath);
                sw.WriteLine(text);
            }
        }

        private bool CreateDictionaryIfNotExist(string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
                return true;
            }
            return false;
        }

        public string RemoveInvalidChars(string filename)
        {
            return string.Concat(filename.Split(Path.GetInvalidFileNameChars()));
        }

        public string ReplaceInvalidChars(string filename)
        {
            return string.Join("_", filename.Split(Path.GetInvalidFileNameChars()));
        }
    }
}
