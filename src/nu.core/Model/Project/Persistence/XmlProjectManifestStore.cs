using System;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using nu.Resources;

namespace nu.Model.Project.Persistence
{
    public class XmlProjectManifestStore : BaseProjectManifestStore, IProjectManifestStore
    {
        private readonly IFileSystem _fileSystem;

        public XmlProjectManifestStore(IFileSystem fileSystem) : base(fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public override IProjectManifest Load(IProjectEnvironment environment)
        {
            Manifest manifest;
            string manifestPath = GetManifestPath(environment);
            ProjectManifestBuilder builder = new ProjectManifestBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof (Manifest));

            if(!Exists(environment))
                throw new FileNotFoundException(string.Format(CultureInfo.CurrentUICulture,
                    nuresources.ProjectManifest_ManifestDoesNotExist, manifestPath));

            using (Stream stream = _fileSystem.Read(manifestPath))
            {
                manifest = (Manifest) serializer.Deserialize(stream);
            }
            return builder.Build(manifest);
        }

        public override void Save(IProjectEnvironment environment, IProjectManifest projectManifest)
        {
            string manifestPath = GetManifestPath(environment);
            ProjectManifestBuilder builder = new ProjectManifestBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof (Manifest));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, builder.Build(projectManifest));
                PrepareManifestDirectory(environment);
                _fileSystem.Write(manifestPath, writer.ToString());
            }
        }
    }
}