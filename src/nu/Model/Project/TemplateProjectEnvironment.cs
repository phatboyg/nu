using System.IO;

namespace nu.Model.Project
{
    public class TemplateProjectEnvironment : ProjectEnvironment
    {

        public TemplateProjectEnvironment(IFileSystem fileSystem) : base(fileSystem)
        {

        }

        public TemplateProjectEnvironment(string directory, IFileSystem fileSystem)
            : base(directory, fileSystem)
        {
        }

        public override string ProjectDirectory
        {
            get
            {
                return _fileSystem.Combine(_fileSystem.ExecutingDirectory, suppliedDirectory);
            }
        }

        public override string ManifestPath
        {
            get
            {
                return Path.Combine(_fileSystem.ExecutingDirectory,
                    Path.Combine(suppliedDirectory, PROJECT_MANIFEST_FILE));
            }
        }
    }
}