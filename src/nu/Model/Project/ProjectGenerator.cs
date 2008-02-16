namespace nu.Model.Template
{
    using System;
    using System.IO;
    using nu.Model.Project;

    public class ProjectGenerator
    {
        private const string PROJECT_KEY = "ProjectName";
        private const string DIRECTORY_KEY = "Directory";

        public ProjectGenerator(IFileSystem fileSystem, IProjectManifest projectManifest, ITemplateProcessor templateProcessor)
        {
            _fileSystem = fileSystem;
            _projectManifest = projectManifest;
            _templateProcessor = templateProcessor;
        }

        private readonly IProjectManifest _projectManifest;
        private readonly ITemplateProcessor _templateProcessor;
        private readonly IFileSystem _fileSystem;


        public IProjectManifest ProjectManifest
        {
            get { return _projectManifest; }
        }

        private IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public ITemplateProcessor TemplateProcessor
        {
            get { return _templateProcessor; }
        }


        public void Generate(String ProjectName, String Directory)
        {

            string rootDirectory = GenerateRootDirectory(ProjectName, Directory);
            ITemplateContext context = TemplateProcessor.CreateTemplateContext();
            context.Items[PROJECT_KEY] = ProjectName;
            context.Items[DIRECTORY_KEY] = Directory;

            foreach (projectFolder target in ProjectManifest.Directories)
            {
                String pathTemplate = Path.Combine(rootDirectory, target.path);
                String processedPath = TemplateProcessor.Process(pathTemplate, context);
                FileSystem.CreateDirectory(processedPath);
            }
            
            foreach(projectFile file in ProjectManifest.Files)
            {
                string fullSourceFilePath = Path.Combine(ProjectManifest.TemplateDirectory, file.source);
                string contents = FileSystem.ReadToEnd(fullSourceFilePath);
                
                string processedFileContent = TemplateProcessor.Process(contents, context);
                string fullDestinationFilePath = rootDirectory + file.destination;
                string processedDestinationPath = TemplateProcessor.Process(fullDestinationFilePath, context);

                FileSystem.Write(processedDestinationPath, processedFileContent);

            }
        }

        private string GenerateRootDirectory(string ProjectName,String Directory)
        {
            string rootDirectory;
            if (!string.IsNullOrEmpty(Directory))
                rootDirectory = Path.Combine(Directory, ProjectName);
            else
                rootDirectory = Path.Combine(FileSystem.CurrentDirectory, ProjectName);
            return rootDirectory;
        }

    }
}