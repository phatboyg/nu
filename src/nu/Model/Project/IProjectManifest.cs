namespace nu.Model.Project
{
    using System;
    using System.Collections.Generic;
    using nu.Model.Template;

    public interface IProjectManifest
    {
        IList<projectFolder> Directories { get; }
        IList<projectFile> Files{ get;}
    }
}