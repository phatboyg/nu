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
            UnitOfWork.RegisterItem<IPath>(new PathAdapter());
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
                AProjectEnvironment environment = new AProjectEnvironment(directory);
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
                AProjectEnvironment environment = new TemplateProjectEnvironment(templateDirectory);
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


    public class AProjectEnvironment
    {
        protected readonly string suppliedDirectory;
        protected const string PROJECT_MANIFEST_DIRECTORY = ".nu";
        protected const string PROJECT_MANIFEST_FILE = "project.nu";

        public AProjectEnvironment()
        {

        }

        public AProjectEnvironment(string directory)
        {
            suppliedDirectory = directory;
        }

        public virtual String ProjectDirectory
        {
            get
            {
                if (!String.IsNullOrEmpty(suppliedDirectory))
                {
                    if (FileSystem.IsRooted(suppliedDirectory))
                        return suppliedDirectory;
                    else
                    {
                        string path = Path.Combine(FileSystem.CurrentDirectory, suppliedDirectory);   
                        return path;
                    }
                }
                else
                    return FileSystem.CurrentDirectory;
            }
        }

        protected static IFileSystem FileSystem
        {
            get { return UnitOfWork.GetItem<IFileSystem>(); }
        }

        protected static IPath Path
        {
            get { return UnitOfWork.GetItem<IPath>(); }
        }

        public virtual String ProjectName
        {
            get
            {
                int startIdx = ProjectDirectory.LastIndexOf(FileSystem.DirectorySeparatorChar.ToString()) + 1;
                return ProjectDirectory.Substring(startIdx);
            }
        }

        public virtual string ManifestPath
        {
            get
            {
                return Path.Combine(ProjectDirectory, 
                    Path.Combine(PROJECT_MANIFEST_DIRECTORY, PROJECT_MANIFEST_FILE));
            }
        }
    }

    public class TemplateProjectEnvironment : AProjectEnvironment
    {

        public TemplateProjectEnvironment()
        {
        }

        public TemplateProjectEnvironment(string directory) : base(directory)
        {
        }

        public override string ManifestPath
        {
            get
            {
                return Path.Combine(FileSystem.ExecutingDirectory,
                    Path.Combine(suppliedDirectory, PROJECT_MANIFEST_FILE));
            }
        }
    }

    public class ProjectManifestStore
    {
        // should be able to take a project environment and load a manifest
        // should be able to take a project environment and persist a manifest

        private bool _intialized;

        public bool Initialized
        {
            get { return _intialized; }
        }

        public void Initialize()
        {
            _intialized = true;
        }

        private IFileSystem FileSystem
        {
            get { return UnitOfWork.GetItem<IFileSystem>(); }
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
