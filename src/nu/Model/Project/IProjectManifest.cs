namespace nu.Model.Project
{
    using System;
    using System.Collections.Generic;
    using nu.Model.Template;

    public interface IProjectManifest
    {
        string TemplateDirectory { get;}
        IList<nu.Model.Project.projectFolder> Directories { get; }
        IList<projectFile> Files{ get;}
    }
}