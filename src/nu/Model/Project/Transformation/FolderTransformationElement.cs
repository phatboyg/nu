using nu.Model.Template;

namespace nu.Model.Project.Transformation
{
    public class FolderTransformationElement : AbstractTransformationElement
    {
        private readonly ITemplateProcessor _templateProcessor;
        private readonly IFileSystem _fileSystem;


        public FolderTransformationElement(ITemplateProcessor processor, IFileSystem fileSystem)
        {
            _templateProcessor = processor;
            _fileSystem = fileSystem;
        }

        public override bool Transform(IProjectManifest templateManifest, IProjectEnvironment environment, IProjectEnvironment templateEnvironment)
        {
            string rootDirectory = environment.ProjectDirectory;
            ITemplateContext context = BuildTemplateContext(_fileSystem, _templateProcessor, environment);
            foreach (FolderDTO folder in templateManifest.Directories)
            {
                string folderTemplatePath = _fileSystem.Combine(rootDirectory, folder.Path);
                string folderProcessedPath = _templateProcessor.Process(folderTemplatePath, context);
                _fileSystem.CreateDirectory(folderProcessedPath);
                folder.Path = folderProcessedPath;
            }
            return true;
        }
    }
}