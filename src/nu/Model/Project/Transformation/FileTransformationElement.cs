using System.Globalization;
using System.IO;
using nu.Model.Template;
using nu.Resources;

namespace nu.Model.Project.Transformation
{
    public class FileTransformationElement : AbstractTransformationElement
    {
        private readonly ITemplateProcessor _processor;
        private readonly IFileSystem _fileSystem;
        private readonly IProjectManifestRepository _manifestRepository;

        public FileTransformationElement(ITemplateProcessor processor, IFileSystem fileSystem, IProjectManifestRepository manifestRepository)
        {
            _processor = processor;
            _fileSystem = fileSystem;
            _manifestRepository = manifestRepository;
        }

        public override bool Transform(IProjectManifest templateManifest, IProjectEnvironment environment, IProjectEnvironment templateEnvironment)
        {
            string rootDirectory = _manifestRepository.GetProjectDirectory(environment);
            ITemplateContext context = BuildTemplateContext(_fileSystem, _processor, environment);
            foreach (FileDTO file in templateManifest.Files)
            {
                string templateRoot = _manifestRepository.GetProjectDirectory(templateEnvironment);
                string fileTemplatePath = JoinTemplatePath(templateRoot, file.Source);
                string fileProcessedPath = _processor.Process(fileTemplatePath, context);

                if (!_fileSystem.Exists(fileProcessedPath))
                    throw new FileNotFoundException(
                        string.Format(CultureInfo.CurrentUICulture, nuresources.FileTransformation_MissingFile, fileProcessedPath));

                string fileContent = _fileSystem.ReadToEnd(fileProcessedPath);
                string processedFileContent = _processor.Process(fileContent, context);

                string fileTemplateDestinationPath = JoinTemplatePath(rootDirectory, file.Destination);
                string fileProcessedDestinationPath = _processor.Process(fileTemplateDestinationPath, context);

                _fileSystem.Write(fileProcessedDestinationPath, processedFileContent);
                file.Source = string.Empty;
                file.Destination = fileProcessedDestinationPath;
            }
            return true;
        }

        public virtual string JoinTemplatePath(string firstPath, string secondPath)
        {
            return firstPath + secondPath;
        }
    }
}