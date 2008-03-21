using System;
using System.IO;
using nu.Utility;

namespace nu.Model.Project
{
    public class ProjectEnvironment : IProjectEnvironment
    {
        protected readonly string suppliedDirectory;
        protected readonly string suppliedProjectName;
        protected readonly IFileSystem _fileSystem;
        protected const string PROJECT_MANIFEST_DIRECTORY = ".nu";
        protected const string PROJECT_MANIFEST_FILE = "project.nu";

        public ProjectEnvironment()
        {

        }

        public ProjectEnvironment(string directory, IFileSystem fileSystem)
        {
            suppliedDirectory = directory;
            _fileSystem = fileSystem;
        }

        public ProjectEnvironment(string directory, string projectName, IFileSystem fileSystem)
        {
            suppliedDirectory = directory;
            suppliedProjectName = projectName;
            _fileSystem = fileSystem;
        }

        public virtual String ProjectDirectory
        {
            get
            {
                if (!String.IsNullOrEmpty(suppliedDirectory))
                {
                    if (_fileSystem.IsRooted(suppliedDirectory))
                    {
                        return
                            String.IsNullOrEmpty(suppliedProjectName)
                                ? suppliedDirectory
                                : _fileSystem.Combine(suppliedDirectory, suppliedProjectName);
                    }
                    else
                    {
                        string path = _fileSystem.Combine(_fileSystem.CurrentDirectory, suppliedDirectory);
                        if (!String.IsNullOrEmpty(suppliedProjectName))
                            path = _fileSystem.Combine(path, suppliedProjectName);
                        return path;
                    }
                }
                else
                    return
                        String.IsNullOrEmpty(suppliedProjectName)
                            ? _fileSystem.CurrentDirectory
                            : _fileSystem.Combine(_fileSystem.CurrentDirectory, suppliedProjectName);
            }
        }


        public virtual String ProjectName
        {
            get
            {
                if(String.IsNullOrEmpty(suppliedProjectName))
                {
                    int startIdx = ProjectDirectory.LastIndexOf(_fileSystem.DirectorySeparatorChar.ToString()) + 1;
                    return ProjectDirectory.Substring(startIdx); 
                }
                else
                {
                    return suppliedProjectName;
                }

            }
        }

        public virtual string ManifestPath
        {
            get
            {
                return Path.Combine(ProjectDirectory,
                    Path.Combine(PROJECT_MANIFEST_DIRECTORY, PROJECT_MANIFEST_FILE));
            }
        }
    }
}