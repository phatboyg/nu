using System;
using System.Collections.Generic;
using System.IO;
using nu.Model.Project;
using nu.Model.Project.Transformation;
using nu.Utility;

namespace nu.Commands
{
    [Command(Description = "Creates a new project")]
    public class NewProjectCommand : ICommand
    {
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;
        private readonly IProjectManifestStore _projectManifestStore;
        private readonly IProjectTransformationPipeline _pipeline;

        private readonly string _rootTemplateDirectory;
        private readonly string _defaultTemplate;

        private string _projectName;
        private string _Directory;
        private string _template;

        public NewProjectCommand(IFileSystem fileSystem, IProjectManifestStore projectManifestStore,
                                 IProjectTransformationPipeline pipeline, IConsole console, String rootTemplateDirectory,
                                 String defaultTemplate)
        {
            _console = console;
            _fileSystem = fileSystem;
            _pipeline = pipeline;
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


        public void Execute(IEnumerable<IArgument> arguments)
        {
            ProjectManifestRepository repository = new ProjectManifestRepository(_projectManifestStore);
            IProjectEnvironment projectEnvironment = new ProjectEnvironment(Directory, ProjectName, _fileSystem);
            IProjectEnvironment templateEnvironment = new TemplateProjectEnvironment(BuildTemplateDirectory(), _fileSystem);

            try
            {
                if (!repository.ManifestExists(projectEnvironment))
                {
                    IProjectManifest templateManifest = repository.LoadProjectManifest(templateEnvironment);
                    _pipeline.Process(templateManifest, projectEnvironment, templateEnvironment);
                    repository.SaveProjectManifest(templateManifest, projectEnvironment);
                }
            }
            catch(FileNotFoundException ex)
            {
                _console.WriteError(ex.Message);
            }

        }

        private string BuildTemplateDirectory()
        {
            return
                String.IsNullOrEmpty(Template)
                    ? _fileSystem.Combine(_rootTemplateDirectory, _defaultTemplate)
                    : _fileSystem.Combine(_rootTemplateDirectory, Template);
        }
    }
}