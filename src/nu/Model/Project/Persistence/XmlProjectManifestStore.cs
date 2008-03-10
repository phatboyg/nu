using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using nu.Commands;

namespace nu.Model.Project.Persistence
{
    public class XmlProjectManifestStore : IProjectManifestStore
    {

        private static IFileSystem FileSystem
        {
            get { return UnitOfWork.GetItem<IFileSystem>(); }
        }

        public IProjectManifest Load(IProjectEnvironment environment)
        {
            Manifest manifest;
            ProjectManifestBuilder builder = new ProjectManifestBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(Manifest));
            using (Stream stream = FileSystem.Read(environment.ManifestPath))
            {
                manifest = (Manifest)serializer.Deserialize(stream);
            }
            return builder.Build(manifest);
        }

        public void Save(IProjectEnvironment environment, IProjectManifest projectManifest)
        {
            ProjectManifestBuilder builder = new ProjectManifestBuilder();
            XmlSerializer serializer = new XmlSerializer(typeof(Manifest));
            using (StringWriter writer = new StringWriter())
            {
                serializer.Serialize(writer, builder.Build(projectManifest));
                PrepareManifestDirectory(environment);
                FileSystem.Write(environment.ManifestPath, writer.ToString());
            }
        }

        private void PrepareManifestDirectory(IProjectEnvironment environment)
        {
            if(!Exists(environment))
            {
                FileSystem.CreateHiddenDirectory(
                    RemoveManifestFileName(environment.ManifestPath));
            }
        }

        private static string RemoveManifestFileName(string file)
        {
            return file.Substring(0, file.LastIndexOf(FileSystem.DirectorySeparatorChar));
        }

        public bool Exists(IProjectEnvironment environment)
        {
            return FileSystem.Exists(environment.ManifestPath);
        }
    }
}