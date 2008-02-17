using System;
using nu.Model.Project;
using nu.Model.Template;

namespace nu.Commands
{
   using System.Collections.Generic;
   using System.IO;
   using Utility;

   [Command(Description = "Creates a new project")]
   public class NewProjectCommand : ICommand
   {
       private readonly IFileSystem _fileSystem;
       private readonly IProjectGenerator _ProjectGenerator;
       private readonly string _templateDirectory;
       private readonly IProjectManifestStore _projectManifestStore;
       private string _projectName;
       private string _Directory;

       public NewProjectCommand(IFileSystem fileSystem, IProjectManifestStore projectManifestStore, 
           IProjectGenerator projectGenerator, String templateDirectory)
       {
           _fileSystem = fileSystem;
           _ProjectGenerator = projectGenerator;
           _templateDirectory = templateDirectory;
           _projectManifestStore = projectManifestStore;
       }

      [DefaultArgument(Required = true, Description = "The name of the project to create")]
      public string ProjectName
      {
          get {return _projectName; }
          set { _projectName = value; }
      }

      [Argument(DefaultValue = "", Key = "d", AllowMultiple = false, Required = false, Description = "The directory to create the project")]
       public string Directory
       {
           get { return _Directory; }
           set{ _Directory = value;}
       }

       public IProjectManifestStore ProjectManifestStore
       {
           get { return _projectManifestStore; }
       }

       public IFileSystem FileSystem
       {
           get { return _fileSystem; }
       }


       public void Execute(IEnumerable<IArgument> arguments)
      {
           IProjectEnvironment environment = new ProjectEnviornment(FileSystem, ProjectName, Directory, _templateDirectory);
           if (!ProjectManifestStore.ManifestExists(environment))
           {
               IProjectManifest manifest = ProjectManifestStore.GetProjectManifestTemplate(environment);
               _ProjectGenerator.Generate(manifest, environment);
               ProjectManifestStore.SaveProjectManifest(manifest, environment);
               // need to perform 'inject' on all packages associated to manifest.
           }
          
      }
   }
}