using System;
using System.IO;
using nu.Utility;

namespace nu.Model.Project
{
    public class ProjectEnvironment : IProjectEnvironment
    {
        private readonly string _projectDirectory;
        private readonly string _projectName;
        private readonly bool _isTemplate;

        public ProjectEnvironment(string projectName, string projectDirectory)
            : this(projectName, projectDirectory, false)
        {

        }

        public ProjectEnvironment(string projectName, string projectDirectory, bool isTemplate)
        {
            _projectName = projectName;
            _projectDirectory = projectDirectory;
            _isTemplate = isTemplate;
        }

        public string ProjectDirectory
        {
            get { return _projectDirectory; }
        }

        public string ProjectName
        {
            get { return _projectName; }
        }

        public bool IsTemplate
        {
            get { return _isTemplate; }
        }
    }
}