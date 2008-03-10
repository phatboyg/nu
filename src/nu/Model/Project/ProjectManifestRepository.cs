namespace nu.Model.Project
{
    public class ProjectManifestRepository
    {

        private static IProjectManifestStore Store
        {
            get { return UnitOfWork.GetItem<IProjectManifestStore>(); }
        }

        public virtual IProjectManifest LoadProjectManifest(IProjectEnvironment environment)
        {
            return Store.Load(environment);
        }

        public virtual void SaveProjectManifest(IProjectManifest projectManifest, IProjectEnvironment environment)
        {
            Store.Save(environment, projectManifest);
        }

        public virtual bool ManifestExists(IProjectEnvironment environment)
        {
            return Store.Exists(environment);
        }
    } 
}