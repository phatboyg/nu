namespace nu.core.Model.Files.Package
{
    using FilePaths;

    public class NugRegistry
    {
        public static NugPackage GetNug(string name)
        {
            var dir = WellKnownPaths.NusExeLocation;
            
            return new NugPackage(name);
        }
    }
}