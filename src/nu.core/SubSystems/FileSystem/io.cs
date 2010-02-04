namespace nu.core.SubSystems.FileSystem
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using FilePaths;
    using Model.Files.Package;
    using NDepend.Helpers.FileDirectoryPath;

    public class io
    {
        public NugPackage Do(string nugName)
        {
            var nug = new NugPackage(nugName);
            WellKnownPaths.WorkWithTempDir((temp) =>
                {
                    var n = WellKnownPaths.GetNug(nugName);

                    if (!n.Exists)
                        throw new Exception("cant find nug");

                    Zip.Unzip(n, temp);

                    //i now have the unzipped contents @ temp
                    var mani = temp.GetChildFileWithName("MANIFEST");
                    var maniS = File.ReadAllText(mani.Path);
                    //json it
                    var m = JsonUtil.Get<Manifest>(maniS);

                    nug.Name = m.Name;
                    nug.Version = m.Version;

                    foreach (var entry in m.Files)
                    {
                        nug.Files.Add(new NugFile()
                            {
                                Name = entry.Name,
                                //whoa
                                File = new MemoryStream(File.ReadAllBytes(mani.GetBrotherFileWithName(entry.Name).Path))
                            });
                    }
                });
            return nug;
        }
    }

    [Serializable]
    public class Manifest
    {
        public string Name { get; set; }
        public string Version { get; set; }
        public IList<ManifestEntry> Files { get; set; }
    }
    [Serializable]
    public class ManifestEntry
    {
        public string Name { get; set; }
    }
}