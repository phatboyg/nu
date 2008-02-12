using System;
using System.Collections.Generic;
using System.Text;

namespace nu.Utility
{
    class ProjectManifest : IProjectManifest
    {
        private readonly IList<string> directories;
        private readonly IList<string> files;

        public ProjectManifest()
        {
            files = new List<string>();
            directories = new List<string>();

            directories.Add("src");
            directories.Add("lib");
            directories.Add("tools");
            directories.Add("build");
        }

        public IEnumerable<string> Directories
        {
            get { return directories; }
        }

        public IEnumerable<string> Files
        {
            get { return files; }
        }
    }
}
