// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace nu.core
{
	using System;
	using Castle.Core;
	using Castle.MicroKernel;
	using Castle.MicroKernel.Registration;
	using Castle.Windsor;
	using Commands;
	using Model.ArgumentParsing;
	using Model.Package;
	using Model.Project.Persistence;
	using Model.Project.Transformation;
	using Model.Template;
	using nu.Model.Package;
	using nu.Model.Project;
	using nu.Model.Project.Transformation;
	using SubSystems.FileSystem;
	using Utility;

	public class NuBootstrapper
	{
		public static void Configure(IWindsorContainer container)
		{
			//allows us to take a dependency on arrays
			container.Kernel.Resolver.AddSubResolver(new ArrayResolver(container.Kernel));

			//
			container.AddComponent<Dispatcher>("dispatcher");

			//argument parsing

			container.AddComponentLifeStyle<IArgumentParser, ArgumentParser>("argumentParser", LifestyleType.Transient);
			container.AddComponentLifeStyle<IArgumentMapFactory, ArgumentMapFactory>("argumentMapFactory", LifestyleType.Transient);

			//helper shims
			container.AddComponentLifeStyle<IConsole, ConsoleHelper>("consoleHelper", LifestyleType.Transient);
			container.AddComponentLifeStyle<IPath, PathAdapter>("pathAdapter", LifestyleType.Transient);
			container.AddComponentLifeStyle<IFileSystem, DotNetFileSystem>("fileSystem", LifestyleType.Transient);

			//templating
			container.AddComponentLifeStyle<ITemplateProcessor, NVelocityTemplateProcessor>("templateProcessor",
				LifestyleType.Transient);

			//package repository
			container.AddComponent<IPackageRepository, LocalPackageRepository>("package.repository");

			//project stuff
			container.AddComponentLifeStyle<IProjectManifestStore, XmlProjectManifestStore>("xmlProjectStore", LifestyleType.Transient);
			container.AddComponentLifeStyle<IProjectManifestRepository, ProjectManifestRepository>("projectManifestRepository", LifestyleType.Transient);

			//default package commands
			container.AddComponent<IOldCommand, HelpCommand>("help");
			container.Register(
				Component.For<IOldCommand>().ImplementedBy<NewProjectCommand>()
					.Named("project")
					.Parameters(
					Parameter.ForKey("rootTemplateDirectory").Eq("a"), //TODO: correct this
					Parameter.ForKey("defaultTemplate").Eq("b"))); //TODO: Correct this


			SetupNewProject(container);
			container.AddComponent<IOldCommand, ListCommand>("list");
			container.AddComponent<IOldCommand, InstallCommand>("install");
		}

		static void SetupNewProject(IWindsorContainer container)
		{
			container.AddComponentLifeStyle<ITransformationElement, FolderTransformationElement>("folderTransformation",
				LifestyleType.Transient);

			container.AddComponentLifeStyle<ITransformationElement, FileTransformationElement>("fileTransformation", LifestyleType.Transient);

			container.AddComponentLifeStyle<IProjectTransformationPipeline, ProjectTransformationPipeline>("transformationPipeline", LifestyleType.Transient);
		}
	}

	public class ArrayResolver :
		ISubDependencyResolver
	{
		readonly IKernel _kernel;

		public ArrayResolver(IKernel kernel)
		{
			_kernel = kernel;
		}

		#region ISubDependencyResolver Members

		public object Resolve(CreationContext context, ISubDependencyResolver parentResolver, ComponentModel model,
		                      DependencyModel dependency)
		{
			return _kernel.ResolveAll(dependency.TargetType.GetElementType(), null);
		}

		public bool CanResolve(CreationContext context, ISubDependencyResolver parentResolver, ComponentModel model,
		                       DependencyModel dependency)
		{
			Type targetType = dependency.TargetType;
			return targetType != null &&
			       targetType.IsClass &&
			       targetType.IsArray &&
			       targetType.GetElementType().IsInterface;
		}

		#endregion
	}
}