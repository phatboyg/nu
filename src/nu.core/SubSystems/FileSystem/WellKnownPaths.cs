namespace nu.core.FilePaths
{
    using System.IO;
    using System.Reflection;
    using NDepend.Helpers.FileDirectoryPath;

    public static class WellKnownPaths
    {
        public static DirectoryPath WorkingDirectory
        {
            get
            {
                return new DirectoryPathAbsolute(Directory.GetCurrentDirectory());
            }
        }

        public static DirectoryPath NusExeLocation
        {
            get
            {
                return new FilePathAbsolute(Assembly.GetEntryAssembly().Location).ParentDirectoryPath;
            }
        }
    }
}