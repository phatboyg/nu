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

        public FileTransformationElement(ITemplateProcessor processor, IFileSystem fileSystem)
        {
            _processor = processor;
            _fileSystem = fileSystem;
        }

        public override bool Transform(IProjectManifest templateManifest, IProjectEnvironment environment, IProjectEnvironment templateEnvironment)
        {
            string rootDirectory = environment.ProjectDirectory;
            ITemplateContext context = BuildTemplateContext(_fileSystem, _processor, environment);
            foreach (FileDTO file in templateManifest.Files)
            {
                string fileTemplatePath = JoinTemplatePath(templateEnvironment.ProjectDirectory, file.Source);
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