using System.Collections.Generic;
using nu.Commands;
using nu.Model.Package;
using NUnit.Framework;
using Rhino.Mocks;
using XF.Specs;
using nu.Model.ArgumentParsing;

namespace Specs_for_InstallCommand
{
   
    [TestFixture]
    public class When_executing_the_inject_command_with_a_product_name_only : Spec
    {
        private InstallCommand command;
        private IEnumerable<IArgument> args;

        protected override void Before_each_spec()
        {
            command = Create<InstallCommand>();

            args = Mock<IEnumerable<IArgument>>();
        }

        [Test]
        public void Retrieve_the_package_by_product_name()
        {
            using (Mocks.Record())
            {
                Expect
                    .Call(Get<IPackageRepository>().FindByName("nunit"))
                    .Return(new Package("nunit", ".\\nunit"));
            }
            using (Mocks.Playback())
            {
                command.Package = "nunit";
                command.Execute(args);
            }
        }
    }

    [TestFixture]
    public class When_installing_package_contents_into_a_project : Spec
    {
        private InstallCommand command;
        private IEnumerable<IArgument> args;

        protected override void Before_each_spec()
        {
            command = Create<InstallCommand>();
            args = Mock<IEnumerable<IArgument>>();
        }

        public void Check_the_project_instance_manifest_to_be_sure_the_package_is_not_already_installed()
        {
        }

        public void Obtain_the_project_instance_manifest_from_the_project_directory()
        {
        }

        public void Inject_each_package_item_into_sources_defined_in_the_project_instance_manifest()
        {
        }
    }

    public class When_the_specified_directory_does_not_contain_a_project : Spec
    {
        public void Display_an_error()
        {
        }
    }

    public class When_a_directory_is_not_specified : Spec
    {
        public void Attempt_to_find_the_project_in_the_current_working_directory()
        {
        }
    }
}