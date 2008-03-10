using System;
using System.IO;
using nu.Utility;

namespace nu.Model.Project
{
    public class ProjectEnvironment : IProjectEnvironment
    {
        protected readonly string suppliedDirectory;
        protected readonly string suppliedProjectName;
        protected const string PROJECT_MANIFEST_DIRECTORY = ".nu";
        protected const string PROJECT_MANIFEST_FILE = "project.nu";

        public ProjectEnvironment()
        {

        }

        public ProjectEnvironment(string directory)
        {
            suppliedDirectory = directory;
        }

        public ProjectEnvironment(string directory, string projectName)
        {
            suppliedDirectory = directory;
            suppliedProjectName = projectName;
        }

        public virtual String ProjectDirectory
        {
            get
            {
                if (!String.IsNullOrEmpty(suppliedDirectory))
                {
                    if (FileSystem.IsRooted(suppliedDirectory))
                    {
                        return
                            String.IsNullOrEmpty(suppliedProjectName)
                                ? suppliedDirectory
                                : Path.Combine(suppliedDirectory, suppliedProjectName);
                    }
                    else
                    {
                        string path = Path.Combine(FileSystem.CurrentDirectory, suppliedDirectory);
                        if (!String.IsNullOrEmpty(suppliedProjectName))
                            path = Path.Combine(path, suppliedProjectName);
                        return path;
                    }
                }
                else
                    return
                        String.IsNullOrEmpty(suppliedProjectName)
                            ? FileSystem.CurrentDirectory
                            : Path.Combine(FileSystem.CurrentDirectory, suppliedProjectName);
            }
        }

        protected static IFileSystem FileSystem
        {
            get { return UnitOfWork.GetItem<IFileSystem>(); }
        }

        protected static IPath Path
        {
            get { return UnitOfWork.GetItem<IPath>(); }
        }

        public virtual String ProjectName
        {
            get
            {
                if(String.IsNullOrEmpty(suppliedProjectName))
                {
                    int startIdx = ProjectDirectory.LastIndexOf(FileSystem.DirectorySeparatorChar.ToString()) + 1;
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