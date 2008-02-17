using System.Collections.Generic;
using System.IO;
using System.Xml.Serialization;
using nu.Model.Template;

namespace nu.Model.Project
{
    public class XmlProjectManifest : IProjectManifest
    {
        private readonly project _projectManifest;

        public XmlProjectManifest(project projectManifest)
        {
            _projectManifest = projectManifest;
        }

        public IList<projectFolder> Directories
        {
            get { return _projectManifest.folders; }
        }

        public IList<projectFile> Files
        {
            get { return _projectManifest.files; }
        }
    }
}