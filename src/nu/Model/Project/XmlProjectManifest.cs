using System.Collections.Generic;
using System.Xml.Serialization;
using nu.Model.Template;

namespace nu.Model.Project
{
    public class XmlProjectManifest : IProjectManifest
    {
        private readonly XmlSerializer _serializer;
        private readonly project _project;

        public XmlProjectManifest(IFileSystem fileSystem, string xmlFile)
        {
            _serializer = new XmlSerializer(typeof (project));
            _project = (project)_serializer.Deserialize(fileSystem.Read(xmlFile));           
        }

        public IList<projectTarget> Directories
        {
            get { return _project.folders; }
        }
    }
}