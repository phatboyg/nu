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
namespace Specs_for_Dispatcher
{
	using Castle.MicroKernel;
	using Castle.Windsor;
	using nu.core;
	using nu.core.Commands;
	using nu.core.Model.ArgumentParsing;
	using nu.core.SubSystems.Locator;
	using nu.Utility;
	using NUnit.Framework;
	using Rhino.Mocks;
	using XF.Specs;

	[TestFixture]
	public class When_a_command_cannot_be_found : Spec
	{
		Dispatcher _dispatcher;
		string[] _bogusArguments;
		IWindsorContainer _mockContainer;
		IOldCommand _mockHelpOldCommand;

		protected override void Before_each_spec()
		{
			_dispatcher = Create<Dispatcher>();
			_bogusArguments = new[] {"no-such-command"};
			_mockHelpOldCommand = Mock<IOldCommand>();

			_mockContainer = Mock<IWindsorContainer>();

			SetupResult.For(_mockContainer.Resolve<IOldCommand>("no-such-command"))
				.Throw(new ComponentNotFoundException("no-such-command"));

			SetupResult.For(_mockContainer.Resolve<IOldCommand>("help"))
				.Return(_mockHelpOldCommand);

			WLocator.InitializeContainer(_mockContainer);

			SetupResult.For(Get<IArgumentParser>().Parse(_bogusArguments))
				.Return(new ArgumentParser().Parse(_bogusArguments));

			SetupResult.For(Get<IArgumentMapFactory>().CreateMap(_dispatcher))
				.Return(new ArgumentMap(typeof(Dispatcher)));

			SetupResult.For(Get<IArgumentMapFactory>().CreateMap(_mockHelpOldCommand))
				.Return(new ArgumentMap(typeof(IOldCommand)));
		}

		[Test]
		public void Execute_the_help_command()
		{
			using (Record)
			{
				_mockHelpOldCommand.Execute(null);
				LastCall.IgnoreArguments();
			}
			using (Playback)
			{
				_dispatcher.Forward(_bogusArguments);
			}
		}

		[Test]
		public void Print_an_unknown_command_message()
		{
			using (Record)
			{
				Get<IConsole>().WriteError("command 'no-such-command' not found");
			}
			using (Playback)
			{
				_dispatcher.Forward(_bogusArguments);
			}
		}
	}
}