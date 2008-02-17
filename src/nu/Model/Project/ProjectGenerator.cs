namespace nu.Model.Project
{
    using System;
    using System.IO;
    using nu.Model.Project;
    using nu.Model.Template;

    public class ProjectGenerator : IProjectGenerator
    {
        private const string PROJECT_KEY = "ProjectName";
        private const string DIRECTORY_KEY = "Directory";
        private const string DIRECTORY_SEPARATOR_KEY = "PathSeparator";

        public ProjectGenerator(IFileSystem fileSystem, ITemplateProcessor templateProcessor)
        {
            _fileSystem = fileSystem;
            _templateProcessor = templateProcessor;
        }

        private readonly ITemplateProcessor _templateProcessor;
        private readonly IFileSystem _fileSystem;


        private IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public ITemplateProcessor TemplateProcessor
        {
            get { return _templateProcessor; }
        }


        public virtual void Generate(IProjectManifest manifest, IProjectEnvironment environment)
        {
            string rootDirectory = environment.GetProjectDirectory();
            ITemplateContext context = BuildTemplateContext(environment);

            foreach (projectFolder target in manifest.Directories)
            {
                String pathTemplate = Path.Combine(rootDirectory, target.path);
                String processedPath = TemplateProcessor.Process(pathTemplate, context);
                FileSystem.CreateDirectory(processedPath);
            }
            
            foreach(projectFile file in manifest.Files)
            {
                string templateFilePath = ProjectPathBuilder.Combine(environment.FileSystem.ExecutingDirectory, 
                    ProjectPathBuilder.Combine(environment.TemplateDirectory, file.source));
                string processedFilePath = TemplateProcessor.Process(templateFilePath, context);
                string fileContent = FileSystem.ReadToEnd(processedFilePath);

                string processedFileContent = TemplateProcessor.Process(fileContent, context);
                string fullDestinationFilePath = ProjectPathBuilder.Combine(rootDirectory,  file.destination);
                string processedDestinationPath = TemplateProcessor.Process(fullDestinationFilePath, context);

                FileSystem.Write(processedDestinationPath, processedFileContent);

            }
        }

        public virtual ITemplateContext BuildTemplateContext(IProjectEnvironment environment)
        {
            ITemplateContext context = TemplateProcessor.CreateTemplateContext();
            context.Items[PROJECT_KEY] = environment.ProjectName;
            context.Items[DIRECTORY_KEY] = environment.Directory;
            context.Items[DIRECTORY_SEPARATOR_KEY] = Path.DirectorySeparatorChar;
            return context;
        }

    }
}