namespace nu.core.FileSystem
{
    public class DotNetCurrentWorkingDirectory :
        DotNetDirectory,
        CurrentWorkingDirectory
    {
        public DotNetCurrentWorkingDirectory(FileSystem fileSystem)
            : base(fileSystem.GetCurrentDirectory().Name)
        {
        }
    }
}