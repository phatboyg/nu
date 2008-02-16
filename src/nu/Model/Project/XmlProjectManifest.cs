using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using nu.Model.Template;

namespace nu.Model.Project
{
    public class XmlProjectManifest : IProjectManifest
    {
        private readonly XmlSerializer _serializer;
        private readonly string _xmlManifestFile;
        private readonly project _project;
        private readonly IFileSystem _fileSystem;

        public XmlProjectManifest(IFileSystem fileSystem, string xmlManifestFile)
        {
            _fileSystem = fileSystem;
            _xmlManifestFile = xmlManifestFile;

            _serializer = new XmlSerializer(typeof (project));
            string fullPath = Path.Combine(fileSystem.ExecutingDirectory, xmlManifestFile);
            if (!fileSystem.Exists(fullPath))
                throw new FileNotFoundException(string.Format("Unable to find the project manifest file:{0}.", fullPath));
            _project = (project)_serializer.Deserialize(fileSystem.Read(fullPath));           
        }


        public string TemplateDirectory
        {
            get { return  new FileInfo(Path.Combine(fileSystem.ExecutingDirectory, _xmlManifestFile)).DirectoryName; }
        }

        private IFileSystem fileSystem
        {
            get { return _fileSystem; }
        }

        public IList<projectFolder> Directories
        {
            get { return _project.folders; }
        }

        public IList<projectFile> Files
        {
            get { return _project.files; }
        }
    }
}