namespace nu.core
{
    using System.IO;
    using NDepend.Helpers.FileDirectoryPath;

    public static class DirectoryInfoExtensions
    {
        public static void CopyTo(this DirectoryPathAbsolute source, DirectoryPathAbsolute target)
        {
            foreach (FilePathAbsolute file in source.ChildrenFilesPath)
            {
                File.Copy(file.Path, target.GetChildFileWithName(file.FileName).Path);
            }


            foreach (DirectoryPathAbsolute dir in source.ChildrenDirectoriesPath)
            {
                dir.CopyTo(target.GetChildDirectoryWithName(dir.DirectoryName));
            }

        }

        public static bool IsRoot(this DirectoryPathAbsolute source)
        {
            return source.DirectoryInfo.Root.Name.Replace("\\", "").Equals(source.Path);
        }

        public static void Create(this DirectoryPathAbsolute path)
        {
            if(!path.Exists)
                Directory.CreateDirectory(path.Path);
        }
    }
}