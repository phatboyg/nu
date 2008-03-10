using System;
using System.Collections.Generic;
using nu.Model.Project;
using nu.Utility;

namespace nu.Commands
{
    [Command(Description = "Creates a new project")]
    public class NewProjectCommand : ICommand
    {
        private readonly IFileSystem _fileSystem;
        private readonly IProjectGenerator _ProjectGenerator;
        private readonly IProjectManifestStore _projectManifestStore;

        private readonly string _rootTemplateDirectory;
        private readonly string _defaultTemplate;

        private string _projectName;
        private string _Directory;
        private string _template;


        public NewProjectCommand(IFileSystem fileSystem, IProjectManifestStore projectManifestStore,
                                 IProjectGenerator projectGenerator, String rootTemplateDirectory,
                                 String defaultTemplate)
        {
            _fileSystem = fileSystem;
            _ProjectGenerator = projectGenerator;
            _rootTemplateDirectory = rootTemplateDirectory;
            _defaultTemplate = defaultTemplate;
            _projectManifestStore = projectManifestStore;
        }

        [DefaultArgument(Required = true, Description = "The name of the project to create. (required)")]
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        [Argument(DefaultValue = "", Key = "d", AllowMultiple = false, Required = false,
            Description = "The directory to create the project.")]
        public string Directory
        {
            get { return _Directory; }
            set { _Directory = value; }
        }

        [Argument(DefaultValue = "", Key = "t", AllowMultiple = false, Required = false,
            Description = "The template directory to use.")]
        public string Template
        {
            get { return _template; }
            set { _template = value; }
        }

        public IProjectManifestStore ProjectManifestStore
        {
            get { return _projectManifestStore; }
        }

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }


        public void Execute(IEnumerable<IArgument> arguments)
        {
            UnitOfWork.RegisterItem<IFileSystem>(new FileSystem(new PathAdapter()));
            UnitOfWork.RegisterItem<IPath>(new PathAdapter());
            UnitOfWork.RegisterItem<IProjectManifestStore>(ProjectManifestStore);

            ProjectManifestRepository repository = new ProjectManifestRepository();
            IProjectEnvironment projectEnvironment = new ProjectEnvironment(Directory, ProjectName);
            IProjectEnvironment templateEnvironment = new TemplateProjectEnvironment(BuildTemplateDirectory());

            if (!repository.ManifestExists(projectEnvironment))
            {
                IProjectManifest templateManifest = repository.LoadProjectManifest(templateEnvironment);
                IProjectManifest generatedManifest =
                    _ProjectGenerator.Generate(templateManifest, projectEnvironment, templateEnvironment);
                repository.SaveProjectManifest(generatedManifest, projectEnvironment);
            }
        }

        private string BuildTemplateDirectory()
        {
            return
                String.IsNullOrEmpty(Template)
                    ? FileSystem.Combine(_rootTemplateDirectory, _defaultTemplate)
                    : FileSystem.Combine(_rootTemplateDirectory, Template);
        }
    }
}