using nu.Commands;

namespace Specs_for_ProjectCommand
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using nu;
    using nu.Model.Project;
    using nu.Utility;
    using NUnit.Framework.SyntaxHelpers;
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

    }


    public class When_building_a_project_environment : Spec
    {
        private IFileSystem fileSystem;

        protected override void Before_each_spec()
        {
            fileSystem = Mock<IFileSystem>();
            UnitOfWork.Reset();
            UnitOfWork.RegisterItem<IFileSystem>(fileSystem);
        }

        [Test]
        public void Should_default_to_the_current_executing_directory_when_not_supplied()
        {
            using(Record)
            {
                SetupResult.For(fileSystem.CurrentDirectory).Return(@"c:\work\is\fun");
            }
            using(Playback)
            {
                AProjectEnvironment environment = new AProjectEnvironment();
                Assert.That(environment.ProjectDirectory, Is.EqualTo(fileSystem.CurrentDirectory));   
            }
        }

        [Test]
        public void Should_provide_the_last_folder_name_for_the_project_name_when_using_the_default_executing_directory()
        {
            using(Record)
            {
                SetupResult.For(fileSystem.DirectorySeparatorChar).Return(Path.DirectorySeparatorChar);
                SetupResult.For(fileSystem.CurrentDirectory).Return(@"c:\work\is\fun");
            }
            using(Playback)
            {
                AProjectEnvironment environment = new AProjectEnvironment();
                Assert.That(environment.ProjectName, Is.EqualTo("fun"));
            }
        }

        [Test]
        public void Should_provide_the_directory_supplied_to_the_project_environment_for_the_project_directory()
        {
            string directory = @"c:\projects\nu";
            using(Record)
            {
                SetupResult.For(fileSystem.IsRooted(directory)).Return(true);
            }
            AProjectEnvironment environment = new AProjectEnvironment(directory);
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
            AProjectEnvironment environment = new AProjectEnvironment(directory);
            Assert.That(environment.ProjectName, Is.EqualTo("nu"));
        }

        [Test]
        public void Should_join_current_working_directory_with_supplied_directory_when_supplied_directory_is_not_absolute()
        {
            string directory = @"c:\work";
            string projectName = "test";
            using(Record)
            {
                SetupResult.For(fileSystem.CurrentDirectory).Return(directory);
                SetupResult.For(fileSystem.IsRooted(projectName)).Return(false);
                SetupResult.For(fileSystem.Combine(directory, projectName)).Return(@"c:\work\test");
            }
            using(Playback)
            {
                AProjectEnvironment environment = new AProjectEnvironment(projectName);
                Assert.That(environment.ProjectDirectory, Is.EqualTo(@"c:\work\test"));
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

            [Test, ExpectedException(typeof(ArgumentException))]
            public void Should_throw_an_argument_exception_when_the_same_type_is_registered_more_than_once()
            {
                string name = "nick";
                UnitOfWork.RegisterItem<string>(name);
                UnitOfWork.RegisterItem<string>(name);
            }

            [Test, ExpectedException(typeof(ArgumentException))]
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

    internal class AProjectEnvironment
    {
        private readonly string suppliedDirectory;

        public AProjectEnvironment()
        {

        }

        public AProjectEnvironment(string directory)
        {
            suppliedDirectory = directory;
        }

        public String ProjectDirectory
        {
            get
            {
                if (!String.IsNullOrEmpty(suppliedDirectory))
                {
                    if (FileSystem.IsRooted(suppliedDirectory))
                        return suppliedDirectory;
                    else
                    {
                        string path = FileSystem.Combine(FileSystem.CurrentDirectory, suppliedDirectory);   
                        return path;
                    }
                }
                else
                    return FileSystem.CurrentDirectory;
            }
        }

        private static IFileSystem FileSystem
        {
            get { return UnitOfWork.GetItem<IFileSystem>(); }
        }

        public String ProjectName
        {
            get
            {
                int startIdx = ProjectDirectory.LastIndexOf(FileSystem.DirectorySeparatorChar.ToString()) + 1;
                return ProjectDirectory.Substring(startIdx);
            }
        }
    }

    public static class UnitOfWork
    {
        private static readonly IDictionary<Type, Object> registry = new Dictionary<Type, Object>();

        public static void RegisterItem<TEntity>(TEntity entity)
        {
            if(registry.ContainsKey(typeof(TEntity)))
                throw new ArgumentException("Type:{0} has already been registered.", typeof(TEntity).ToString());
            registry.Add(typeof (TEntity), entity);
        }

        public static TEntity GetItem<TEntity>()
        {
            if(!registry.ContainsKey(typeof(TEntity)))
                throw new ArgumentException("Type {0} has not been registered.", typeof(TEntity).ToString());
            return (TEntity)registry[typeof (TEntity)];
        }

        public static void Reset()
        {
            registry.Clear();
        }
    }

    

}