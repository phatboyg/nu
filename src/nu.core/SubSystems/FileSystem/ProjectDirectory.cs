namespace nu.core.FilePaths
{
    using System;
    using Model.Files;
    using NDepend.Helpers.FileDirectoryPath;

    /// <summary>
    /// where the '.nu' folder is
    /// </summary>
    public class ProjectDirectory
    {
        private readonly DirectoryPathAbsolute _projectDirectory;
        private readonly DirectoryPathAbsolute _libDirectory;

        public ProjectDirectory(WorkingDirectory workingDirectory)
        {
            _projectDirectory = workingDirectory.WorkWithMe(WalkThePathLookingForNu);
            if (_projectDirectory != null)
                _libDirectory = _projectDirectory.GetBrotherDirectoryWithName("lib");
            _libDirectory.Create();
        }

        public string Path
        {
            get { return _projectDirectory.Path; }
        }

        public void Install(LocalNugInfo info)
        {
            if(!FoundAProject()) throw new Exception("not in a nu project path");

            var targetDir = _libDirectory.GetChildDirectoryWithName(info.Name);
            if(!targetDir.Exists)
            {
                targetDir.Create();
            }

            info.Path.CopyTo(_libDirectory.GetChildDirectoryWithName(info.Name));
        }

        public bool FoundAProject()
        {
            return _projectDirectory != null;
        }

        public DirectoryPathAbsolute WalkThePathLookingForNu(DirectoryPathAbsolute path)
        {
            DirectoryPathAbsolute result = null;

            if (!path.IsRoot())
            {
                DirectoryPathAbsolute bro = path.GetBrotherDirectoryWithName(".nu");
                if (bro.Exists)
                {
                    return bro;
                }
                if (path.HasParentDir)
                {
                    result = WalkThePathLookingForNu(path.ParentDirectoryPath);
                }

            }


            return result;
        }
    }
}