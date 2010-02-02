namespace nu.core
{
    using System.IO;
    using ICSharpCode.SharpZipLib.Zip;
    using NDepend.Helpers.FileDirectoryPath;

    public static class Zip
    {
        public static void Unzip(FilePathAbsolute sourceFile, DirectoryPathAbsolute targetDirectory)
        {
            new FastZip().ExtractZip(sourceFile.Path, targetDirectory.Path, "");
        }

        public static void Compress(FilePathAbsolute file, DirectoryPathAbsolute directory)
        {
            new FastZip().CreateZip(file.Path, directory.Path, true, "*.*");
        }
    }
}