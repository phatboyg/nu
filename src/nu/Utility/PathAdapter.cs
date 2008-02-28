namespace nu.Utility
{
    using System.IO;

    public class PathAdapter : IPath
    {
        public string Combine(string firstPath, string secondPath)
        {
            return Path.Combine(firstPath, secondPath);
        }

        public char DirectorySeparatorChar
        {
            get { return Path.DirectorySeparatorChar; }
        }
    }
}