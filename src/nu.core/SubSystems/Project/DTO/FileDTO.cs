using System;
using System.Xml.Serialization;

namespace nu.Model.Project
{
    public class FileDTO
    {
        private string _source;
        private String _destination;

        public FileDTO()
        {
        }

        public FileDTO(string source, string destination)
        {
            Source = source;
            Destination = destination;
        }

        [XmlAttribute(AttributeName="source")]
        public string Source
        {
            get { return _source; }
            set { _source = value; }
        }

        [XmlAttribute(AttributeName = "destination")]
        public String Destination
        {
            get { return _destination; }
            set { _destination = value; }
        }
    }
}