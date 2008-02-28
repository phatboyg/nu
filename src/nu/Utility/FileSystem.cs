using System;
using System.IO;

namespace nu.Utility
{
    public class FileSystem : IFileSystem
    {
        private readonly IPath _path;

        public FileSystem(IPath path)
        {
            _path = path;
        }

        public bool Exists(string filePath)
        {
            return File.Exists(filePath);
        }

        public bool DirectoryExists(string directory)
        {
            return Directory.Exists(directory);
        }

        public Stream Read(string filePath)
        {
            return new FileStream(filePath, FileMode.Open);
        }

        public String ReadToEnd(string filePath) 
        {
            string contents;
            using (Stream stream = Read(filePath))
            {
                using(StreamReader reader = new StreamReader(stream))
                {
                    contents = reader.ReadToEnd();
                }
            }
            return contents;
        }


        public void Write(string filePath, String contents)
        {
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate))
            {
                using(StreamWriter writer = new StreamWriter(fs))
                {
                    writer.Write(contents);
                    writer.Flush();
                }
            }
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

        public bool IsRooted(string path)
        {
            return Path.IsPathRooted(path);
        }

        public string Combine(string firstPath, string secondPath)
        {
            return _path.Combine(firstPath, secondPath);
        }

        public char DirectorySeparatorChar
        {
            get { return _path.DirectorySeparatorChar; }
        }
    }
}