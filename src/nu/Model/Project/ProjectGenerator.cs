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


        public virtual IProjectManifest Generate(IProjectManifest templateManifest, IProjectEnvironment environment, IProjectEnvironment templateEnvironment)
        {
            string rootDirectory = environment.ProjectDirectory;
            ITemplateContext context = BuildTemplateContext(environment);

            foreach (FolderDTO target in templateManifest.Directories)
            {
                String pathTemplate = Path.Combine(rootDirectory, target.Path);
                String processedPath = TemplateProcessor.Process(pathTemplate, context);
                FileSystem.CreateDirectory(processedPath);
                target.Path = processedPath;
            }

            foreach (FileDTO file in templateManifest.Files)
            {
                string templateFilePath = ProjectPathBuilder.Combine(templateEnvironment.ProjectDirectory, file.Source);
                string processedFilePath = TemplateProcessor.Process(templateFilePath, context);

                if(!FileSystem.Exists(processedFilePath))
                    throw new FileNotFoundException(string.Format("ProjectGenerator was provided '{0}' via the manifest which does not exist", processedFilePath));

                string fileContent = FileSystem.ReadToEnd(processedFilePath);

                string processedFileContent = TemplateProcessor.Process(fileContent, context);
                string fullDestinationFilePath = ProjectPathBuilder.Combine(rootDirectory,  file.Destination);
                string processedDestinationPath = TemplateProcessor.Process(fullDestinationFilePath, context);

                FileSystem.Write(processedDestinationPath, processedFileContent);
                file.Source = string.Empty;
                file.Destination = processedDestinationPath;
            }
            return templateManifest;
        }

        public virtual ITemplateContext BuildTemplateContext(IProjectEnvironment environment)
        {
            ITemplateContext context = TemplateProcessor.CreateTemplateContext();
            context.Items[PROJECT_KEY] = environment.ProjectName;
            context.Items[DIRECTORY_KEY] = environment.ProjectDirectory;
            context.Items[DIRECTORY_SEPARATOR_KEY] = Path.DirectorySeparatorChar;
            return context;
        }

    }
}