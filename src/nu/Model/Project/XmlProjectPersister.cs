using System;
using System.IO;
using System.Xml.Serialization;

namespace nu.Model.Project
{
    public class XmlProjectPersister : IProjectPersister
    {
        private readonly IFileSystem _fileSystem;
        private const String PROJECT_FILE_NAME = "project.nu";

        public XmlProjectPersister(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public void SaveProjectManifest(IProjectManifest manifest, String directory)
        {
            string filePath = GetFullPath(directory);
            XmlSerializer serializer = new XmlSerializer(typeof (project));
            using(StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, manifest);
                FileSystem.Write(filePath, writer.ToString());    
            }
        }

        public bool ManifestExists(string directory)
        {
            string manifestFile = GetFullPath(directory);
            return FileSystem.Exists(manifestFile);
        }

        private static string GetFullPath(String directory)
        {
            return Path.Combine(directory, PROJECT_FILE_NAME);
        }
    }
}