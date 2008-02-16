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
       private readonly ProjectGenerator _ProjectGenerator;
       private string _projectName;
       private string _Directory;

       public NewProjectCommand(IFileSystem fileSystem, ProjectGenerator _ProjectGenerator)
       {
           _fileSystem = fileSystem;
           this._ProjectGenerator = _ProjectGenerator;
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
       

      public void Execute(IEnumerable<IArgument> arguments)
      {
         // verify a project doesn't alread exist.
         // find the project tree manifest
         // build filesystem according to manifest
         // inject any projects named in the manifest
         // have a nice day
          //System.Diagnostics.Debugger.Break();
          _ProjectGenerator.Generate(ProjectName, Directory);
      }

   }
}