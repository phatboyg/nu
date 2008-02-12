using System;
using System.IO;

namespace nu.Utility
{
    public class FileSystem : IFileSystem
    {
        private string currentDirectory;
        private string executingDirectory;

        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public Stream Read(string filePath)
        {
            return new FileStream(filePath, FileMode.Open);
        }

        public void Write(string filePath, Stream file)
        {
            using(FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using(StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Flush();
                }
            }
        }

        public void CreateDirectory(string directoryPath)
        {
            Directory.CreateDirectory(directoryPath);
        }

        public void Copy(string source, string destination)
        {
            File.Copy(source, destination);
        }

        public string CurrentDirectory
        {
            get { return Directory.GetCurrentDirectory(); }
        }

        public string ExecutingDirectory
        {
            get { return AppDomain.CurrentDomain.BaseDirectory; }
        }
    }
}