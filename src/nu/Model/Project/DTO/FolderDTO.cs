using System.Xml.Serialization;

namespace nu.Model.Project
{
    public class FolderDTO
    {
        public FolderDTO()
        {
        }

        public FolderDTO(string path)
        {
            Path = path;
        }

        public FolderDTO(string path, string key)
        {
            Path = path;
            Key = key;
        }

        private string _path;
        private string key;

        [XmlAttribute(AttributeName = "path")]
        public string Path
        {
            get { return _path; }
            set { _path = value; }
        }

        [XmlAttribute(AttributeName = "key")]
        public string Key
        {
            get { return key; }
            set { key = value; }
        }

    }
}