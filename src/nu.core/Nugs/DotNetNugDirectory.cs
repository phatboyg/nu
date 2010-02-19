namespace nu.core.Nugs
{
    using System;
    using Configuration;
    using FileSystem;
    using spec;

    /// <summary>
    /// Represents a ~/nugs/[NugDirectory]
    /// ~/nugs/log4net-1.3.3 <- the nug directory
    /// </summary>
    public class DotNetNugDirectory:
        DotNetDirectory,
        NugDirectory
    {
        readonly Manifest _manifest;

        public DotNetNugDirectory(DirectoryName directoryName)
            : base(directoryName)
        {
            var manifestFile = GetChildFile("MANIFEST.json");
            var manifestContent = manifestFile.ReadToEnd();
            _manifest = JsonUtil.Get<Manifest>(manifestContent);
        }

        public string Version
        {
            get { return _manifest.Version; }
        }

        public string NugName
        {
            get { return _manifest.Name; }
        }
    }
}