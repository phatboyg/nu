namespace nu.core.FilePaths
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using Model.Files.Package;
    using NDepend.Helpers.FileDirectoryPath;

    public static class WellKnownPaths
    {
        public static DirectoryPath WorkingDirectory
        {
            get
            {
                return new DirectoryPathAbsolute(Directory.GetCurrentDirectory());
            }
        }

        public static DirectoryPath NusExeLocation
        {
            get
            {
                return new FilePathAbsolute(Assembly.GetEntryAssembly().Location).ParentDirectoryPath;
            }
        }

        public static DirectoryPath ExtensionsFolder
        {
            get
            {
                return new DirectoryPathAbsolute(NusExeLocation.Path).GetChildDirectoryWithName("extensions");
            }
        }

        public static DirectoryPathAbsolute NugsFolder
        {
            get
            {
                return new DirectoryPathAbsolute(NusExeLocation.Path).GetChildDirectoryWithName("nugs");
            }
        }

        public static FilePathAbsolute GetNug(string nugName)
        {
            var x = WellKnownPaths.NugsFolder;
            var n = x.GetChildFileWithName(nugName);
            return n;
        }

        public static NugPackage GetNugPackage(string name)
        {
            var path = NugsFolder.GetChildFileWithName(string.Format("{0}.nug", name));
            var target = new DirectoryPathAbsolute("");
            Zip.Unzip(path, target);
            var manifest = target.GetChildFileWithName("MANIFEST");
            var manifestContent = File.ReadAllText(manifest.Path);
            var m = JsonUtil.Get<Manifest>(manifestContent);
            var np = new NugPackage(m.Name);
            np.Version = m.Version;

            foreach (var entry in m.Entries)
            {
                //get the file
                //load into a stream
                np.Files.Add(new NugFile()
                                 {
                                     Name = entry.Name,
                                     File = null //the file stream
                                 });
            }
            
            return np;
        }

        public static void WorkWithTempDir(Action<DirectoryPathAbsolute> tempAction)
        {
            var tempDir = Path.Combine(Path.GetTempPath(), "nu");
            tempDir = Path.Combine(tempDir, Guid.NewGuid().ToString());
            var d = new DirectoryPathAbsolute(tempDir);
            if (!d.Exists)
            {
                d.Create();
            }
            tempAction(d);
            d.Delete();
        }
    }

    [Serializable]
    public class Manifest
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public IList<ManifestEntry> Entries { get; set; }
    }

    [Serializable]
    public class ManifestEntry
    {
        public string Name { get; set; }
    }
}