namespace nu.Commands
{
    using Utility;

    [Command(Description = "Creates a new project")]
    public class NewProjectCommand : Command
    {
        private string _projectName;
        [DefaultArgument(Description = "The name of the project to create")]
        public string ProjectName
        {
            get { return _projectName; }
            set { _projectName = value; }
        }

        public override void Execute()
        {
            // verify a project doesn't alread exist.
            // find the project tree manifest
            // build filesystem according to manifest
            // inject any projects named in the manifest
            // have a nice day
        }
    }
}