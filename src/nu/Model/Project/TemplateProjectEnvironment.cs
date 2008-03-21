namespace nu.Model.Project
{
    public class TemplateProjectEnvironment : ProjectEnvironment
    {

        public TemplateProjectEnvironment()
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
                return _fileSystem.Combine(_fileSystem.ExecutingDirectory,
                    _fileSystem.Combine(suppliedDirectory, PROJECT_MANIFEST_FILE));
            }
        }
    }
}