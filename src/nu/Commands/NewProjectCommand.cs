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
       private readonly IProjectManifest _manifest;
       private readonly ProjectGenerator _ProjectGenerator;
       private readonly IProjectPersister projectPersister;
       private string _projectName;
       private string _Directory;

       public NewProjectCommand(IFileSystem fileSystem, IProjectManifest manifest, ProjectGenerator _ProjectGenerator, IProjectPersister projectPersister)
       {
           _fileSystem = fileSystem;
           _manifest = manifest;
           this._ProjectGenerator = _ProjectGenerator;
           this.projectPersister = projectPersister;
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

       public IFileSystem FileSystem
       {
           get { return _fileSystem; }
       }

       public IProjectPersister ProjectPersister
       {
           get { return projectPersister; }
       }

       public IProjectManifest ProjectManifest
       {
           get { return _manifest; }
       }


       public void Execute(IEnumerable<IArgument> arguments)
      {
         // verify a project doesn't alread exist.
         // find the project tree manifest
         // build filesystem according to manifest
         // inject any projects named in the manifest
         // have a nice day
          //System.Diagnostics.Debugger.Break();
           string rootDirectory = GenerateRootDirectory(ProjectName, Directory);
           if (!ProjectPersister.ManifestExists(rootDirectory))
           {
               _ProjectGenerator.Generate(ProjectName, Directory);
               ProjectPersister.SaveProjectManifest(ProjectManifest, rootDirectory);
           }
          
      }


       private string GenerateRootDirectory(String projectName, String directory)
       {
           string rootDirectory;
           if (!string.IsNullOrEmpty(Directory))
               rootDirectory = Path.Combine(Directory, ProjectName);
           else
               rootDirectory = Path.Combine(FileSystem.CurrentDirectory, ProjectName);
           return rootDirectory;
       }
   }
}