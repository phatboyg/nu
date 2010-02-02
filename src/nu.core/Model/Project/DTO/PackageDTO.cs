using System.Xml.Serialization;

namespace nu.Model.Project
{
    public class PackageDTO
    {
        private string _name;

        public PackageDTO()
        {
        }

        public PackageDTO(string name)
        {
            Name = name;
        }

        [XmlAttribute(AttributeName = "name")]
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
    }
}