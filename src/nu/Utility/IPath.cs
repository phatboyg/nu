namespace nu.Utility
{
    public interface IPath
    {
        string Combine(string firstPath, string secondPath);
        char DirectorySeparatorChar{ get;}
        string GetDirectoryName(string path);
    }
}