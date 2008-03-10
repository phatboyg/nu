using System;

namespace nu.Model.Project
{
    public interface IProjectEnvironment
    {
        String ProjectDirectory { get; }
        String ProjectName { get; }
        string ManifestPath { get; }
    }
}