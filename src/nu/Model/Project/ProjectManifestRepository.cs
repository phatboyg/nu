namespace nu.Model.Project
{
    public class ProjectManifestRepository
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
    } 
}