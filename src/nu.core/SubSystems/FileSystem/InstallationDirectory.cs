namespace nu.core.FilePaths
{
    using System.Collections.Generic;
    using System.IO;
    using Files;
    using NDepend.Helpers.FileDirectoryPath;

    /// <summary>
    /// Where the 'nu.exe' is located
    /// </summary>
    public class InstallationDirectory
    {
        private readonly DirectoryPathAbsolute _installationPath;
        private readonly DirectoryPathAbsolute _installedNugPath;

        public InstallationDirectory(DirectoryPathAbsolute installationPath)
        {
            _installationPath = installationPath;
            _installedNugPath = _installationPath.GetChildDirectoryWithName("nugs");
        }

        public NuRegistry GetRegistry()
        {
            string rawJson = GetRegistryFile();
            return JsonUtil.Get<NuRegistry>(rawJson);
        }

        public string GetRegistryFile()
        {
            var fi = _installationPath.GetChildFileWithName(NuRegistry.FileName);
            return File.ReadAllText(fi.Path);
        }

        public string[] InstalledNugs()
        {
            var result = new List<string>();
            var bob = _installedNugPath.ChildrenDirectoriesPath;
            foreach (DirectoryPathAbsolute absolute in bob)
            {
                result.Add(absolute.DirectoryName);
            }
            return result.ToArray();
        }
        public bool IsNugAlreadyLocal(string name)
        {
            return _installedNugPath.GetChildDirectoryWithName(name).Exists;
        }

        public void StoreNug(NugSpec spec, Stream stream)
        {
            var nugDirectory = _installedNugPath.GetChildDirectoryWithName(spec.Name);
            if (!nugDirectory.Exists)
                Directory.CreateDirectory(nugDirectory.Path);

            var tempName = string.Format("{0}.zip", spec.Name);
            var tempFile = _installedNugPath.GetChildFileWithName(tempName);
            stream.WriteToDisk(tempFile.Path);
            Zip.Unzip(tempFile, nugDirectory);
            File.Delete(tempFile.Path);
        }

        public LocalNugInfo GetNug(string name)
        {
            //take from here
            return new LocalNugInfo();
        }
    }
}