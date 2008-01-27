namespace Specs_for_HelpCommand
{
    using nu.Commands;
    using NUnit.Framework;
    using XF.Specs;

    [TestFixture]
    public class When_executing_the_help_command_only : Spec
    {
        private HelpCommand command;

        protected override void Before_each_spec()
        {
            command = Create<HelpCommand>();
        }

        [Test]
        public void A_list_of_available_commands_should_be_displayed()
        {
            
        }

    }
}