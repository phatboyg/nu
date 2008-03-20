using nu.Model.Template;

namespace nu.Model.Project.Transformation
{
    /// <summary>
    /// Separate the stuff within the ProjectGenerator class into separate
    /// elements to be run through the pipeline to allow for things like 
    /// transformation, file generation, etc., everything below is just random
    /// thoughts at this point.
    /// </summary>
    public class ProjectTransformationPipeline
    {
        public ProjectTransformationPipeline(ITransformationElement[] elements)
        {
            
        }

        public void Process()
        {
            
        }
    }

    public interface ITransformationElement
    {
        bool Transform(IProjectManifest templateManifest);
    }


    public abstract class AbstractTransformationElement
    {
        protected ITemplateProcessor templateProcessor;
        protected const string PROJECT_KEY = "ProjectName";
        protected const string DIRECTORY_KEY = "Directory";
        protected const string DIRECTORY_SEPARATOR_KEY = "PathSeparator";

        protected virtual ITemplateContext BuildTemplateContext(IFileSystem fileSystem, IProjectEnvironment environment)
        {
            ITemplateContext context = templateProcessor.CreateTemplateContext();
            context.Items[PROJECT_KEY] = environment.ProjectName;
            context.Items[DIRECTORY_KEY] = environment.ProjectDirectory;
            context.Items[DIRECTORY_SEPARATOR_KEY] = fileSystem.DirectorySeparatorChar;
            return context;
        }
    }

    public class FolderTransformationElement : AbstractTransformationElement, ITransformationElement
    {
        private readonly ITemplateProcessor _templateProcessor;
        private readonly IFileSystem _fileSystem;

        public FolderTransformationElement(ITemplateProcessor processor, IFileSystem fileSystem)
        {
            _templateProcessor = processor;
            _fileSystem = fileSystem;
        }

        public bool Transform(IProjectManifest templateManifest)
        {
            ITemplateContext context = BuildTemplateContext(_fileSystem, null);
            foreach (FolderDTO folder in templateManifest.Directories)
            {

            }
            return true;
        }

    }

    public class FileTransformationElement : AbstractTransformationElement, ITransformationElement
    {
        public FileTransformationElement(ITemplateProcessor processor, IFileSystem fileSystem)
        {
            
        }

        public bool Transform(IProjectManifest templateManifest)
        {
            foreach (FileDTO file in templateManifest.Files)
            {

            }
            return true;
        }
    }
}