namespace nu.Model.Project
{
    public interface IProjectEnvironment
    {
        string GetProjectDirectory();
        string ProjectName { get; }
        string Directory { get; }
        string TemplateDirectory { get; }
        IFileSystem FileSystem{ get;}
    }
}