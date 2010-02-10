namespace nu.core.FileSystem
{
    using System;
    using Configuration;
    using Model.Files.Package;
    using Nugs;

    public class DotNetNugDirectory :
        DotNetDirectory, NugDirectory
    {
        readonly FileSystem _fileSystem;

        public DotNetNugDirectory(FileSystem fileSystem, Directory directory)
            : base(directory.Name)
        {
            this._fileSystem = fileSystem;
        }

        public NugPackage GetNug(string name)
        {
            var path = GetNugFile(name);
            
            var np = new NugPackage(name);
            _fileSystem.WorkWithTempDir(temp =>
            {
                Directory target = new DotNetDirectory(new AbsoluteDirectoryName(temp.Path));
                Zip.Unzip(path, target);
                var manifest = target.GetChildFile("MANIFEST");
                var manifestContent = manifest.ReadAllText();
                var m = JsonUtil.Get<Manifest>(manifestContent);

                np.Version = m.Version;

                foreach (var entry in m.Files)
                {
                    np.Files.Add(new NugFile
                    {
                        Name = entry.Name,
                        //whoa
                        File = new System.IO.MemoryStream(System.IO.File.ReadAllBytes(manifest.Parent.GetChildFile(entry.Name).Path))
                    });
                }
            });


            return np;
        }

        public File GetNugFile(string name)
        {
            return base.GetChildFile(string.Format("{0}.nug", name));
        }
    }
}