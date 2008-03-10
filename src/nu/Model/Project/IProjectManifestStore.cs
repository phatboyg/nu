namespace nu.Model.Project
{
    public interface IProjectManifestStore
    {
        IProjectManifest Load(IProjectEnvironment environment);
        void Save(IProjectEnvironment environment, IProjectManifest projectManifest);
        bool Exists(IProjectEnvironment environment);
    }
}