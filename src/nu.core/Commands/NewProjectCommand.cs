using System;
using System.Collections.Generic;
using System.IO;
using nu.Model.Project;
using nu.Model.Project.Transformation;
using nu.Utility;

namespace nu.Commands
{
    using Model.ArgumentParsing;

    [Command(Description = "Creates a new project")]
    public class NewProjectCommand : ICommand
    {
        private readonly IConsole _console;
        private readonly IFileSystem _fileSystem;
        private readonly IProjectManifestRepository _projectManifestRepository;
        private readonly IProjectTransformationPipeline _pipeline;

        private readonly string _rootTemplateDirectory;
        private readonly string _defaultTemplate;

        private string _projectName;
        private string _Directory;
        private string _template;

        public NewProjectCommand(IFileSystem fileSystem, IProjectManifestRepository projectManifestRepository,
                                 IProjectTransformationPipeline pipeline, IConsole console, String rootTemplateDirectory,
                                 String defaultTemplate)
        {
            _console = console;
            _fileSystem = fileSystem;
            _projectManifestRepository = projectManifestRepository;
            _pipeline = pipeline;
            _rootTemplateDirectory = rootTemplateDirectory;
            _defaultTemplate = defaultTemplate;
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
            IProjectEnvironment projectEnvironment = new ProjectEnvironment(ProjectName, Directory);
            IProjectEnvironment templateEnvironment = new ProjectEnvironment(string.Empty, 
                                                                            BuildTemplateDirectory(), true);

            try
            {
                if (!_projectManifestRepository.ManifestExists(projectEnvironment))
                {
                    IProjectManifest templateManifest = _projectManifestRepository.LoadProjectManifest(templateEnvironment);
                    _pipeline.Process(templateManifest, projectEnvironment, templateEnvironment);
                    _projectManifestRepository.SaveProjectManifest(templateManifest, projectEnvironment);
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