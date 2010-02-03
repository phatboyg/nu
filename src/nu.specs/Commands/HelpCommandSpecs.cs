namespace Specs_for_HelpCommand
{
   using System.Collections.Generic;
   using Castle.Core;
   using Castle.MicroKernel;
   using Castle.Windsor;
   using nu.core.Commands;
   using nu.core.Model.ArgumentParsing;
   using nu.core.SubSystems.Locator;
   using nu.Utility;
   using NUnit.Framework;
   using Rhino.Mocks;
   using XF.Specs;

   [TestFixture]
   public class When_executing_without_a_command_argument : Spec
   {
      private HelpCommand _command;
      private IWindsorContainer _container;
      private IKernel _kernel;
      private List<IHandler> _handlers;
      private IHandler _handlerForFakeCommand1;
      private IHandler _handlerForFakeCommand2;

      protected override void Before_each_spec()
      {
         _command = Create<HelpCommand>();

         _container = Mock<IWindsorContainer>();
         _kernel = Mock<IKernel>();
         SetupResult.For(_container.Kernel).Return(_kernel);
         WLocator.InitializeContainer(_container);

         _handlerForFakeCommand1 = Mock<IHandler>();
         _handlerForFakeCommand2 = Mock<IHandler>();

         SetupResult.For(_handlerForFakeCommand1.ComponentModel).Return(
            new ComponentModel("fake-command-1", typeof (IOldCommand), typeof (FakeCommand1)));
         SetupResult.For(_handlerForFakeCommand1.Resolve(null)).Return(new FakeCommand1());
         LastCall.IgnoreArguments();

         SetupResult.For(_handlerForFakeCommand2.ComponentModel).Return(
            new ComponentModel("fake-command-2", typeof (IOldCommand), typeof (FakeCommand2)));
         SetupResult.For(_handlerForFakeCommand2.Resolve(null)).Return(new FakeCommand2());
         LastCall.IgnoreArguments();

         _handlers = new List<IHandler>();
         _handlers.Add(_handlerForFakeCommand1);
         _handlers.Add(_handlerForFakeCommand2);
      }

      [Test]
      public void Retrieve_a_list_of_registered_commands()
      {
         using (Record)
         {
            Expect.Call(_kernel.GetAssignableHandlers(typeof (IOldCommand))).Return(_handlers.ToArray());
         }
         using (Playback)
         {
            _command.Execute(null);
         }
      }

      [Test]
      public void Print_an_available_commands_heading()
      {
         using (Record)
         {
            Expect.Call(_kernel.GetAssignableHandlers(typeof (IOldCommand))).Return(_handlers.ToArray());
            Get<IConsole>().WriteHeading(null);
            LastCall.IgnoreArguments();
         }
         using (Playback)
         {
            _command.Execute(null);
         }
      }

      [Test]
      public void Print_a_line_for_each_command()
      {
         using (Record)
         {
            Expect.Call(_kernel.GetAssignableHandlers(typeof (IOldCommand))).Return(_handlers.ToArray());
            Get<IConsole>().WriteLine(null);
            LastCall.IgnoreArguments().Repeat.Twice();
         }
         using (Playback)
         {
            _command.Execute(null);
         }
      }
   }

   [TestFixture]
   public class When_executing_with_a_command_argument : Spec
   {
      private HelpCommand _command;
      private FakeCommand1 _commandWeWantHelpFor;
      private IWindsorContainer _container;
      private IArgumentMap _argumentMap;

      protected override void Before_each_spec()
      {
         _command = Create<HelpCommand>();

         _container = Mock<IWindsorContainer>();
         WLocator.InitializeContainer(_container);

         _commandWeWantHelpFor = new FakeCommand1();

         SetupResult.For(_container.Resolve<IOldCommand>("fake-command-1")).Return(_commandWeWantHelpFor);

         _argumentMap = Mock<IArgumentMap>();
         SetupResult.For(Get<IArgumentMapFactory>().CreateMap(_commandWeWantHelpFor)).Return(_argumentMap);
         SetupResult.For(_argumentMap.Usage).Return("usage");
      }

      [Test]
      public void Display_the_command_name()
      {
         using (Record)
         {
            Get<IConsole>().WriteHeading("Command: fake-command-1");
         }
         using (Playback)
         {
            _command.CommandName = "fake-command-1";
            _command.Execute(null);
         }
      }

      public void Display_the_command_description()
      {
      }

      public void Display_the_command_arguments()
      {
      }

      public void Display_examples_of_the_command_usage()
      {
      }
   }

   [TestFixture]
   public class When_executing_with_an_invalid_command_argument : Spec
   {
      private HelpCommand _command;
      private IWindsorContainer _container;

      protected override void Before_each_spec()
      {
         _command = Create<HelpCommand>();

         _container = Mock<IWindsorContainer>();
         WLocator.InitializeContainer(_container);

         SetupResult.For(_container.Resolve<IOldCommand>("fake-command-1"))
            .Throw(new ComponentNotFoundException(typeof (IOldCommand)));
      }

      [Test]
      public void Display_a_command_not_found_message_if_unable_to_locate_a_command()
      {
         using (Record)
         {
            Get<IConsole>().WriteError("command 'fake-command-1' not found");
         }
         using (Playback)
         {
            _command.CommandName = "fake-command-1";
            _command.Execute(null);
         }
      }
   }

   [Description("Fake Command 1")]
   public class FakeCommand1 : IOldCommand
   {
      public void Execute(IEnumerable<IArgument> arguments)
      {
      }
   }

   [Description("Fake Command 2")]
   public class FakeCommand2 : IOldCommand
   {
      public void Execute(IEnumerable<IArgument> arguments)
      {
      }
   }
}