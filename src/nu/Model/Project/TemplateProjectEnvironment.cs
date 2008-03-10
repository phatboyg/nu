namespace nu.Model.Project
{
    public class TemplateProjectEnvironment : ProjectEnvironment
    {

        public TemplateProjectEnvironment()
        {
        }

        public TemplateProjectEnvironment(string directory)
            : base(directory)
        {
        }

        public override string ProjectDirectory
        {
            get
            {
                return Path.Combine(FileSystem.ExecutingDirectory, suppliedDirectory);
            }
        }

        public override string ManifestPath
        {
            get
            {
                return Path.Combine(FileSystem.ExecutingDirectory,
                    Path.Combine(suppliedDirectory, PROJECT_MANIFEST_FILE));
            }
        }
    }
}