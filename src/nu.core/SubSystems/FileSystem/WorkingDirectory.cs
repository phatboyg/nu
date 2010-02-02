namespace nu.core.FilePaths
{
    using System;
    using NDepend.Helpers.FileDirectoryPath;

    /// <summary>
    /// where the 'nu' command was executed from
    /// </summary>
    public class WorkingDirectory
    {

        private readonly DirectoryPathAbsolute _workingPath;
        public WorkingDirectory(DirectoryPathAbsolute path)
        {
            _workingPath = path;
        }

        public string Path
        {
            get { return _workingPath.Path; }
        }

        public DirectoryPathAbsolute WorkWithMe(Func<DirectoryPathAbsolute, DirectoryPathAbsolute> work)
        {
            return work(_workingPath);
        }
    }
}