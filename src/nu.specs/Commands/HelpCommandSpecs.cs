namespace Specs_for_HelpCommand
{
   using System.Collections.Generic;
   using Castle.Core;
   using Castle.MicroKernel;
   using Castle.Windsor;
   using nu;
   using nu.Commands;
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
         IoC.InitializeContainer(_container);

         _handlerForFakeCommand1 = Mock<IHandler>();
         _handlerForFakeCommand2 = Mock<IHandler>();

         SetupResult.For(_handlerForFakeCommand1.ComponentModel).Return(
            new ComponentModel("fake-command-1", typeof (ICommand), typeof (FakeCommand1)));
         SetupResult.For(_handlerForFakeCommand1.Resolve(null)).Return(new FakeCommand1());
         LastCall.IgnoreArguments();

         SetupResult.For(_handlerForFakeCommand2.ComponentModel).Return(
               new ComponentModel("fake-command-2", typeof(ICommand), typeof(FakeCommand2)));
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
            Expect.Call(_kernel.GetAssignableHandlers(typeof(ICommand))).Return(_handlers.ToArray());
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
            Expect.Call(_kernel.GetAssignableHandlers(typeof(ICommand))).Return(_handlers.ToArray());
            Get<IConsoleHelper>().WriteHeading(null);
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
            Expect.Call(_kernel.GetAssignableHandlers(typeof(ICommand))).Return(_handlers.ToArray());
            Get<IConsoleHelper>().WriteLine(null);
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
      private HelpCommand command;

      protected override void Before_each_spec()
      {
         command = Create<HelpCommand>();
      }

      public void Display_the_command_name()
      {
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

   [Description("Fake Command 1")]
   public class FakeCommand1 : ICommand
   {
      public void Execute(IEnumerator<IArgument> arguments)
      {
         
      }
   }

   [Description("Fake Command 2")]
   public class FakeCommand2: ICommand
   {
      public void Execute(IEnumerator<IArgument> arguments)
      {

      }
   }
}