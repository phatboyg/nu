namespace nu.Model.Project
{
    public class ProjectManifestRepository : IProjectManifestRepository
    {
        private readonly IProjectManifestStore _store;

        public ProjectManifestRepository(IProjectManifestStore store)
        {
            _store = store;
        }

        public virtual IProjectManifest LoadProjectManifest(IProjectEnvironment environment)
        {
            return _store.Load(environment);
        }

        public virtual void SaveProjectManifest(IProjectManifest projectManifest, IProjectEnvironment environment)
        {
            _store.Save(environment, projectManifest);
        }

        public virtual bool ManifestExists(IProjectEnvironment environment)
        {
            return _store.Exists(environment);
        }

        public virtual string GetProjectDirectory(IProjectEnvironment environment)
        {
            return _store.GetProjectDirectory(environment);
        }

        public virtual string GetProjectName(IProjectEnvironment environment)
        {
            return _store.GetProjectName(environment);
        }

        public virtual string GetManifestPath(IProjectEnvironment environment)
        {
            return _store.GetManifestPath(environment);
        }
    } 
}