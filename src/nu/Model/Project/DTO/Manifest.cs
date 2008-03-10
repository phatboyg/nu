using System.Xml.Serialization;

namespace nu.Model.Project
{
    [XmlRoot(Namespace = "", IsNullable = false)]
    public class Manifest
    {
        [XmlArrayItemAttribute("folder", IsNullable = false)]
        public FolderDTO[] folders;

        [XmlArrayItemAttribute("file", IsNullable = false)]
        public FileDTO[] files;

        [XmlArrayItemAttribute("package", IsNullable = false)]
        public PackageDTO[] packages;
    }
}