using System.IO;
using System.Xml.Serialization;

namespace nu.Model.Project.Persistence
{
    public class XmlProjectManifestStore : IProjectManifestStore
    {
        private readonly IFileSystem _fileSystem;

        public XmlProjectManifestStore(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IProjectManifest Load(IProjectEnvironment environment)
        {
            Manifest manifest;
            ProjectManifestBuilder builder = new ProjectManifestBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof (Manifest));

            if(!Exists(environment))
                throw new FileNotFoundException(string.Format("'{0}' manifest does not exist", environment.ManifestPath));

            using (Stream stream = _fileSystem.Read(environment.ManifestPath))
            {
                manifest = (Manifest) serializer.Deserialize(stream);
            }
            return builder.Build(manifest);
        }

        public void Save(IProjectEnvironment environment, IProjectManifest projectManifest)
        {
            ProjectManifestBuilder builder = new ProjectManifestBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof (Manifest));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, builder.Build(projectManifest));
                PrepareManifestDirectory(environment);
                _fileSystem.Write(environment.ManifestPath, writer.ToString());
            }
        }

        private void PrepareManifestDirectory(IProjectEnvironment environment)
        {
            if (!Exists(environment))
            {
                _fileSystem.CreateHiddenDirectory(
                    RemoveManifestFileName(environment.ManifestPath));
            }
        }

        private string RemoveManifestFileName(string file)
        {
            return file.Substring(0, file.LastIndexOf(_fileSystem.DirectorySeparatorChar));
        }

        public bool Exists(IProjectEnvironment environment)
        {
            return _fileSystem.Exists(environment.ManifestPath);
        }
    }
}