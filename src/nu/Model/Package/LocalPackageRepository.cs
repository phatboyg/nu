namespace nu.Model.Package
{
    using System;
    using System.Collections.Generic;
    using Utility;

    public class LocalPackageRepository : IPackageRepository
    {
        //TODO: How does it know where the packages are?

        private IPath _path;
        private IFileSystem _fileSystem;

        public LocalPackageRepository(IPath path, IFileSystem fileSystem)
        {
            _path = path;
            _fileSystem = fileSystem;
        }

        public string GetNuRoot
        {
            get
            {
                return _fileSystem.ExecutingDirectory;
            }
        }

        public string PathToPackages
        {
            get { return _path.Combine(GetNuRoot, "packages"); }
        }

        public List<string> GetPackageNames()
        {
            string[] dirs =  _fileSystem.GetDirectories(PathToPackages);
            List<string> results = new List<string>();

            foreach (string dir in dirs)
            {
                int i = dir.LastIndexOf(_fileSystem.DirectorySeparatorChar);

                results.Add(dir.Substring(i+1));
            }

            return results;
        }

        public IEnumerable<Package> FindAll()
        {
            List<Package> result = new List<Package>();
            
            foreach (string s in GetPackageNames())
            {
                result.Add(new Package(s));
            }
            
            return result;
        }

        public Package FindCurrentVersionOf(string package)
        {
            throw new NotImplementedException();
        }
    }
}