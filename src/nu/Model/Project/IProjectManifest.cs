namespace nu.Model.Project
{
    using System;
    using System.Collections.Generic;
    using nu.Model.Template;

    public interface IProjectManifest
    {
        IList<projectTarget> Directories { get; }
    }
}