namespace nu
{
    using System.IO;

    public interface IFileSystem
    {
        bool Exists(string filePath);
        Stream Read(string filePath);
        void Write(string filePath, Stream file);
        void CreateDirectory(string directoryPath);
        void Copy(string source, string destination);
        string CurrentDirectory { get; }
        string ExecutingDirectory { get; }
    }
}