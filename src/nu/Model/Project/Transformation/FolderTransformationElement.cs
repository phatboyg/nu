using nu.Model.Template;

namespace nu.Model.Project.Transformation
{
    public class FolderTransformationElement : AbstractTransformationElement
    {
        private readonly ITemplateProcessor _templateProcessor;
        private readonly IFileSystem _fileSystem;
        private readonly IProjectManifestRepository _manifestRepository;


        public FolderTransformationElement(ITemplateProcessor processor, IFileSystem fileSystem, IProjectManifestRepository manifestRepository)
        {
            _templateProcessor = processor;
            _fileSystem = fileSystem;
            _manifestRepository = manifestRepository;
        }

        public override bool Transform(IProjectManifest templateManifest, IProjectEnvironment environment, IProjectEnvironment templateEnvironment)
        {
            string rootDirectory = _manifestRepository.GetProjectDirectory(environment);
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