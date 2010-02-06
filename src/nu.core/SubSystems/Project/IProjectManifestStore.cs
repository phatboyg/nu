namespace nu.Model.Project
{
    public interface IProjectManifestStore
    {
        IProjectManifest Load(IProjectEnvironment environment);
        void Save(IProjectEnvironment environment, IProjectManifest projectManifest);
        bool Exists(IProjectEnvironment environment);


        string GetProjectDirectory(IProjectEnvironment environment);
        string GetProjectName(IProjectEnvironment environment);
        string GetManifestPath(IProjectEnvironment environment);
    }
}