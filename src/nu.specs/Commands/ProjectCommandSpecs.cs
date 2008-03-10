using System;
using System.IO;
using nu;
using nu.Commands;
using nu.Model.Project;
using nu.Utility;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using XF.Specs;

namespace Specs_for_ProjectCommand
{
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
            UnitOfWork.Reset();
            UnitOfWork.RegisterItem<IFileSystem>(fileSystem);
            UnitOfWork.RegisterItem<IPath>(new PathAdapter());
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
                IProjectEnvironment environment = new ProjectEnvironment();
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
                IProjectEnvironment environment = new ProjectEnvironment();
                Assert.That(environment.ProjectName, Is.EqualTo("fun"));
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
            IProjectEnvironment environment = new ProjectEnvironment(directory);
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
            IProjectEnvironment environment = new ProjectEnvironment(directory);
            Assert.That(environment.ProjectName, Is.EqualTo("nu"));
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
                IProjectEnvironment environment = new ProjectEnvironment(projectName);
                Assert.That(environment.ProjectDirectory, Is.EqualTo(@"c:\work\test"));
            }
        }

        [Test]
        public void Should_be_able_to_render_the_project_manifest_path_from_the_current_working_directory()
        {
            string directory = @"c:\work";
            using (Record)
            {
                SetupResult.For(fileSystem.CurrentDirectory).Return(directory);
            }
            using (Playback)
            {
                IProjectEnvironment environment = new ProjectEnvironment(directory);
                Assert.That(environment.ManifestPath, Is.EqualTo(@"c:\work\.nu\project.nu"));
            }
        }

        [Test]
        public void Should_render_the_template_manifest_path_from_the_current_directory_and_template_path()
        {
            string executingDirectory = @"c:\work";
            string templateDirectory = @"project\cs-20";
            using (Record)
            {
                SetupResult.For(fileSystem.ExecutingDirectory).Return(executingDirectory);
            }
            using (Playback)
            {
                IProjectEnvironment environment = new TemplateProjectEnvironment(templateDirectory);
                Assert.That(environment.ManifestPath, Is.EqualTo(@"c:\work\project\cs-20\project.nu"));
            }
        }
    }


    public class When_building_a_unit_of_work : Spec
    {
        protected override void Before_each_spec()
        {
            UnitOfWork.Reset();
        }

        [Test]
        public void Should_be_able_to_register_an_item_with_a_generic_interface()
        {
            string name = "nick";
            UnitOfWork.RegisterItem<string>(name);
            Assert.That(UnitOfWork.GetItem<string>(), Is.EqualTo(name));
        }

        [Test, ExpectedException(typeof (ArgumentException))]
        public void Should_throw_an_argument_exception_when_the_same_type_is_registered_more_than_once()
        {
            string name = "nick";
            UnitOfWork.RegisterItem<string>(name);
            UnitOfWork.RegisterItem<string>(name);
        }

        [Test, ExpectedException(typeof (ArgumentException))]
        public void Should_throw_an_exception_when_requesting_a_type_that_has_not_been_registered()
        {
            UnitOfWork.GetItem<string>();
        }

        [Test]
        public void Should_be_able_to_clear_all_registration_from_the_unit_of_work()
        {
            string name = "nick";
            UnitOfWork.RegisterItem<string>(name);
            UnitOfWork.Reset();
            UnitOfWork.RegisterItem<string>(name);
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