namespace nu.core.Model.Files.Package
{
    using System.Collections.Generic;

    public class NugPackage
    {
        public NugPackage(string name)
        {
            Name = name;
        }

        public string Name { get; set; }
        public string Version { get; set; }
        public IList<NugFile> Files { get; set;}
    }
}