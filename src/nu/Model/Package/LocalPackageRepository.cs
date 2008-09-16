namespace nu.Model.Package
{
    using System.Collections.Generic;
    using Utility;

    public class LocalPackageRepository : IPackageRepository
    {
        private readonly IFileSystem _fileSystem;
        private readonly IPath _path;

        public LocalPackageRepository(IPath path, IFileSystem fileSystem)
        {
            _path = path;
            _fileSystem = fileSystem;
        }

        private string PathToPackages
        {
            get { return _path.Combine(_fileSystem.GetNuRoot, "packages"); }
        }

        #region IPackageRepository Members

        public Package FindByName(string package)
        {
            Package result = null;

            foreach (Package p in FindAll())
            {
                if (p.Name.Equals(package))
                {
                    result = p;
                    break;
                }
            }

            return result;
        }

        public IEnumerable<Package> FindAll()
        {
            var result = new List<Package>();

            foreach (var s in GetPackageNames())
            {
                result.Add(new Package(s[0], s[1]));
            }

            return result;
        }

        #endregion

        private List<string[]> GetPackageNames()
        {
            string[] dirs = _fileSystem.GetDirectories(PathToPackages);
            var results = new List<string[]>();

            foreach (string dir in dirs)
            {
                int i = dir.LastIndexOf(_fileSystem.DirectorySeparatorChar);

                results.Add(new[] {dir.Substring(i + 1), dir});
            }

            return results;
        }
    }
}