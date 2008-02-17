using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace nu.Model.Project
{
    public class XmlProjectManifestStore : IProjectManifestStore
    {
        private readonly IFileSystem _fileSystem;
        private XmlSerializer _serializer;
        private const String PROJECT_FILE_NAME = "project.nu";

        public XmlProjectManifestStore(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IFileSystem FileSystem
        {
            get { return _fileSystem; }
        }

        public IProjectManifest GetProjectManifestTemplate(IProjectEnvironment environment)
        {
            string directory = Path.Combine(environment.FileSystem.ExecutingDirectory, environment.TemplateDirectory);
            if (!environment.FileSystem.DirectoryExists(directory))
                throw new FileNotFoundException(string.Format("Project manifest directory:{0} does not exist.", directory));
            else
                return LoadManifest(directory);
        }

        public IProjectManifest GetProjectManifest(IProjectEnvironment environment)
        {
            string directory = environment.GetProjectDirectory();
            if (!environment.FileSystem.DirectoryExists(directory))
                throw new FileNotFoundException(string.Format("Pproject manifest directory:{0} does not exist.", directory));
            else 
                return LoadManifest(directory);
        }

        private IProjectManifest LoadManifest(string directory)
        {
            XmlProjectManifest projectManifest;
            project projectElement;
            _serializer = new XmlSerializer(typeof(project));
            string projectFilePath = GetFullPath(directory);
            Stream stream = FileSystem.Read(projectFilePath);
            projectElement = (project)_serializer.Deserialize(stream);
            projectManifest = new XmlProjectManifest(projectElement);
            return projectManifest;
        }

        public void SaveProjectManifest(IProjectManifest manifest, IProjectEnvironment environment)
        {
            string directory = environment.GetProjectDirectory();
            string filePath = GetFullPath(directory);
            _serializer = new XmlSerializer(typeof (project));
            project serializedManifest = new project();

            //extract to something else - this is really dirty.
            serializedManifest.files = new List<projectFile>(manifest.Files).ToArray();
            serializedManifest.folders = new List<projectFolder>(manifest.Directories).ToArray();

            using(StringWriter writer = new StringWriter())
            {
                _serializer.Serialize(writer, serializedManifest);
                FileSystem.Write(filePath, writer.ToString());    
            }
        }

        public bool ManifestExists(IProjectEnvironment environment)
        {
            string directory = environment.GetProjectDirectory();
            string manifestFile = GetFullPath(directory);
            return FileSystem.Exists(manifestFile);
        }

        private static string GetFullPath(String directory)
        {
            return Path.Combine(directory, PROJECT_FILE_NAME);
        }
    }
}