namespace Specs_for_HelpCommand
{
   using System.Collections.Generic;
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

      protected override void Before_each_spec()
      {
         _command = Create<HelpCommand>();
         _container = Mock<IWindsorContainer>();
         _kernel = Mock<IKernel>();
         SetupResult.For(_container.Kernel).Return(_kernel);
         IoC.InitializeContainer(_container);
      }

      [Test]
      public void Retrieve_a_list_of_registered_commands()
      {
         using (Record)
         {
            Expect.Call(_kernel.GetAssignableHandlers(typeof (ICommand))).Return(null);
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