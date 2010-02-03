using System.IO;
using nu.Model.Project;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using XF.Specs;

namespace Specs_for_ProjectCommand
{
	using nu.core.Commands;
	using nu.core.SubSystems.FileSystem;

    [TestFixture]
    public class When_executing_a_command_argument : Spec
    {
        private NewProjectCommand _command;

        protected override void Before_each_spec()
        {
            _command = Create<NewProjectCommand>();
        }
    }


    public class When_building_a_project_environment : Spec
    {
        private IFileSystem fileSystem;

        protected override void Before_each_spec()
        {
            fileSystem = Mock<IFileSystem>();
        }

        [Test]
        public void Should_default_to_the_current_executing_directory_when_not_supplied()
        {
            using (Record)
            {
                SetupResult.For(fileSystem.CurrentDirectory).Return(@"c:\work\is\fun");
            }
            using (Playback)
            {
                IProjectEnvironment environment = new ProjectEnvironment("", @"c:\work\is\fun");
                Assert.That(environment.ProjectDirectory, Is.EqualTo(fileSystem.CurrentDirectory));
            }
        }

        [Test]
        public void Should_provide_the_last_folder_name_for_the_project_name_when_using_the_default_executing_directory()
        {
            using (Record)
            {
                SetupResult.For(fileSystem.DirectorySeparatorChar).Return(Path.DirectorySeparatorChar);
                SetupResult.For(fileSystem.CurrentDirectory).Return(@"c:\work\is\fun");
            }
            using (Playback)
            {
                IProjectEnvironment environment = new ProjectEnvironment("", @"c:\work\is\fun");
                Assert.That(environment.ProjectName, Is.EqualTo(""));
            }
        }

        [Test]
        public void Should_provide_the_directory_supplied_to_the_project_environment_for_the_project_directory()
        {
            string directory = @"c:\projects\nu";
            using (Record)
            {
                SetupResult.For(fileSystem.IsRooted(directory)).Return(true);
            }
            IProjectEnvironment environment = new ProjectEnvironment("", directory);
            Assert.That(environment.ProjectDirectory, Is.EqualTo(directory));
        }

        [Test]
        public void Should_provide_the_last_folder_name_for_the_project_name_when_provided_a_specific_directory()
        {
            string directory = @"c:\projects\nu";
            using (Record)
            {
                SetupResult.For(fileSystem.DirectorySeparatorChar).Return(Path.DirectorySeparatorChar);
                SetupResult.For(fileSystem.IsRooted(directory)).Return(true);
            }
            IProjectEnvironment environment = new ProjectEnvironment("", directory);
            Assert.That(environment.ProjectName, Is.EqualTo(""));
        }

        [Test]
        public void
            Should_join_current_working_directory_with_supplied_directory_when_supplied_directory_is_not_absolute()
        {
            string directory = @"c:\work";
            string projectName = "test";
            using (Record)
            {
                SetupResult.For(fileSystem.CurrentDirectory).Return(directory);
                SetupResult.For(fileSystem.IsRooted(projectName)).Return(false);
                SetupResult.For(fileSystem.Combine(directory, projectName)).Return(@"c:\work\test");
            }
            using (Playback)
            {
                IProjectEnvironment environment = new ProjectEnvironment(projectName, directory);
                Assert.That(environment.ProjectDirectory, Is.EqualTo(@"c:\work"));
            }
        }
    }


    public class When_building_a_project_manifest_repository : Spec
    {
        [Test]
        public void Should_be_able_to_query_for_a_sepecific_folder_by_name()
        {
            Spec_not_implemented();
        }

        [Test]
        public void Should_be_able_to_retrieve_the_project_manifest_given_the_project_environment()
        {
            Spec_not_implemented();
        }
    }
}