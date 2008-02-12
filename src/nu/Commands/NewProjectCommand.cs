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

       public NewProjectCommand(IFileSystem fileSystem, ProjectGenerator _ProjectGenerator)
       {
           _fileSystem = fileSystem;
           this._ProjectGenerator = _ProjectGenerator;
       }


       private string _projectName;

      [DefaultArgument(Required = true, Description = "The name of the project to create")]
      public string ProjectName
      {
          get { return string.IsNullOrEmpty(_projectName) ? _fileSystem.CurrentDirectory : _projectName; }
         set { _projectName = value; }
      }
       

      public void Execute(IEnumerable<IArgument> arguments)
      {
         // verify a project doesn't alread exist.
         // find the project tree manifest
         // build filesystem according to manifest
         // inject any projects named in the manifest
         // have a nice day
          _ProjectGenerator.Generate(ProjectName);
      }
   }
}