namespace nu.Commands
{
    using System.Collections.Generic;
    using Utility;

    [Command(Description = "Creates a new project")]
    public class NewProjectCommand : ICommand
    {
        private string _projectName;
        [DefaultArgument(Description = "The name of the project to create")]
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        public void Execute(IEnumerator<IArgument> arguments)
        {
            // verify a project doesn't alread exist.
            // find the project tree manifest
            // build filesystem according to manifest
            // inject any projects named in the manifest
            // have a nice day
        }
    }
}