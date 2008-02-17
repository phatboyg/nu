using nu.Commands;

namespace Specs_for_ProjectCommand
{
    using XF.Specs;
    using Rhino.Mocks;
    using NUnit.Framework;
    
    [TestFixture]
    public class When_executing_a_command_argument : Spec
    {
        private NewProjectCommand _command;

        protected override void Before_each_spec()
        {
            _command = Create<NewProjectCommand>();
        }

        [Test]
        public void Check_project_directory_for_existing_manifest()
        {
            _command.ProjectName = "test";
            _command.Directory = "";

           
            
            _command.Execute(null);
        }


    }
}