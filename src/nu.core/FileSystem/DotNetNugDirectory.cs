namespace nu.core.FileSystem
{
    using System;

    public class DotNetNugDirectory :
        DotNetDirectory, NugDirectory
    {
        public DotNetNugDirectory(Directory directory)
            : base(directory.Name)
        {
        }

        public File GetNug(string name)
        {
            return base.GetChildFile(string.Format("{0}.nug", name));   
        }
    }
}