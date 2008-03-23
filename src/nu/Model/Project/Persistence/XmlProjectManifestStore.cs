using System;
using System.Globalization;
using System.IO;
using System.Xml.Serialization;
using nu.Resources;

namespace nu.Model.Project.Persistence
{
    public class XmlProjectManifestStore : IProjectManifestStore
    {
        private readonly IFileSystem _fileSystem;
        protected const string PROJECT_MANIFEST_DIRECTORY = ".nu";
        protected const string PROJECT_MANIFEST_FILE = "project.nu";

        public XmlProjectManifestStore(IFileSystem fileSystem) 
        {
            _fileSystem = fileSystem;
        }

        public virtual IProjectManifest Load(IProjectEnvironment environment)
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

        public virtual void Save(IProjectEnvironment environment, IProjectManifest projectManifest)
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

        private void PrepareManifestDirectory(IProjectEnvironment environment)
        {
            if (!Exists(environment))
            {
                string manifestPath = GetManifestPath(environment);
                _fileSystem.CreateHiddenDirectory(
                    RemoveManifestFileName(manifestPath));
            }
        }

        private string RemoveManifestFileName(string file)
        {
            return file.Substring(0, file.LastIndexOf(_fileSystem.DirectorySeparatorChar));
        }

        public virtual bool Exists(IProjectEnvironment environment)
        {
            string manifestPath = GetManifestPath(environment);
            return _fileSystem.Exists(manifestPath);
        }

        public virtual string GetProjectDirectory(IProjectEnvironment environment)
        {
            if (!String.IsNullOrEmpty(environment.ProjectDirectory))
            {
                if (_fileSystem.IsRooted(environment.ProjectDirectory))
                {
                    return
                        String.IsNullOrEmpty(environment.ProjectName)
                            ? environment.ProjectDirectory
                            : _fileSystem.Combine(environment.ProjectDirectory, environment.ProjectName);
                }
                else
                {
                    string path;
                    if(environment.IsTemplate)
                        path = _fileSystem.Combine(_fileSystem.ExecutingDirectory, environment.ProjectDirectory);
                    else
                        path = _fileSystem.Combine(_fileSystem.CurrentDirectory, environment.ProjectDirectory);

                    if (!String.IsNullOrEmpty(environment.ProjectName))
                        path = _fileSystem.Combine(path, environment.ProjectName);

                    return path;
                }
            }
            else
                return
                    String.IsNullOrEmpty(environment.ProjectName)
                        ? _fileSystem.CurrentDirectory
                        : _fileSystem.Combine(_fileSystem.CurrentDirectory, environment.ProjectName);

        }

        public virtual string GetProjectName(IProjectEnvironment environment)
        {
            if (String.IsNullOrEmpty(environment.ProjectName))
            {
                int startIdx = environment.ProjectDirectory.LastIndexOf(_fileSystem.DirectorySeparatorChar.ToString()) + 1;
                return environment.ProjectDirectory.Substring(startIdx);
            }
            else
            {
                return environment.ProjectName;
            }
        }

        public virtual string GetManifestPath(IProjectEnvironment environment)
        {
            return _fileSystem.Combine(GetProjectDirectory(environment),
                    _fileSystem.Combine(PROJECT_MANIFEST_DIRECTORY, PROJECT_MANIFEST_FILE));
        }
    }
}