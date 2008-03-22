using System;

namespace nu
{
    using System.IO;

    public interface IFileSystem
    {
        bool Exists(string filePath);
        bool DirectoryExists(string directory);
        Stream Read(string filePath);
        String ReadToEnd(string filePath);
        void Write(string filePath, Stream file);
        void Write(string filePath, String content);
        void CreateDirectory(string directoryPath);
        void CreateHiddenDirectory(string directoryPath);
        void Copy(string source, string destination);
        string CurrentDirectory { get; }
        string ExecutingDirectory { get; }
        bool IsRooted(string path);
        string Combine(string firstPath, string secondPath);
        char DirectorySeparatorChar{ get;}
        string[] GetDirectories(string path);
    }
}