using System.IO;

namespace nu.Model.Project
{
    public class ProjectEnviornment : IProjectEnvironment
    {
        private readonly IFileSystem _fileSystem;
        private readonly string _templateDirectory;
        private readonly string _projectName;
        private readonly string _directory;

        public ProjectEnviornment(IFileSystem fileSystem, string ProjectName, string Directory, string templateDirectory)
        {
            _fileSystem = fileSystem;
            _templateDirectory = templateDirectory;
            _projectName = ProjectName;
            _directory = Directory;
        }

        public string GetProjectDirectory()
        {
            string projectDirectory;
            if (!string.IsNullOrEmpty(_directory))
                projectDirectory = Path.Combine(_directory, _projectName);
            else
                projectDirectory = Path.Combine(_fileSystem.CurrentDirectory, _projectName);
            return projectDirectory;
        }

        public string ProjectName
        {
            get { return _projectName; }
        }

        public string Directory
        {
            get { return _directory; }
        }

        public string TemplateDirectory
        {
            get { return _templateDirectory; }
        }

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

    }
}