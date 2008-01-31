namespace nu.Utility
{
    using System;
    using nu.Model.Project;

   class Generator
    {
        public Generator(IFileSystem fileSystem, IProjectManifest projectManifest)
        {
            _fileSystem = fileSystem;
            _projectManifest = projectManifest;
        }
        private readonly IProjectManifest _projectManifest;
        private readonly IFileSystem _fileSystem;


        public IProjectManifest ProjectManifest
        {
            get { return _projectManifest; }
        }

        private IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }


        public void Generate()
        {
            foreach (String directory in ProjectManifest.Directories)
            {
                FileSystem.CreateDirectory(directory);
            }

            // use some template system to run against the
            // files supplied in the project manifest

        }
    }
}
