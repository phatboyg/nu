using System;
using ngem.core;
using NUnit.Framework;
using Rhino.Mocks;

namespace ngem.client
{
   [TestFixture]
   public class DispatcherTests : TestBase
   {
      [Test]
      public void Should_look_for_commands_in_a_central_location()
      {
         ICommandRegistry registry = Mocks.DynamicMock<ICommandRegistry>();
         ICommand mockCommand = Mocks.DynamicMock<ICommand>();
         Dispatcher dispatch = new Dispatcher(registry);

         using (Mocks.Record()) Expect.Call(registry.FindCommand("blah")).Return(mockCommand);

         using (Mocks.Playback())
         {
            string[] args = new string[] {"blah"};
            dispatch.Forward(args);
         }
      }

      [Test]
      public void When_no_arguments_are_present_should_execute_the_help_command()
      {
         ICommandRegistry registry = Mocks.DynamicMock<ICommandRegistry>();
         ICommand mockCommand = Mocks.DynamicMock<ICommand>();
         Dispatcher dispatch = new Dispatcher(registry);

         using (Mocks.Record()) Expect.Call(registry.FindCommand("help")).Return(mockCommand).Repeat.Twice();

         using (Mocks.Playback())
         {
            dispatch.Forward(null);

            string[] args = new string[] {};
            dispatch.Forward(args);
         }
      }

      [Test]
      public void Should_remove_the_fronting_dash_from_the_command_argument_before_lookup()
      {
         ICommandRegistry registry = Mocks.DynamicMock<ICommandRegistry>();
         ICommand mockCommand = Mocks.DynamicMock<ICommand>();
         Dispatcher dispatch = new Dispatcher(registry);

         using (Mocks.Record()) Expect.Call(registry.FindCommand("blah")).Return(mockCommand);

         using (Mocks.Playback())
         {
            string[] args = new string[] {"-blah"};
            dispatch.Forward(args);
         }
      }

      [Test]
      public void Should_execute_the_command()
      {
         ICommandRegistry registry = Mocks.DynamicMock<ICommandRegistry>();
         ICommand mockCommand = Mocks.DynamicMock<ICommand>();
         Dispatcher dispatch = new Dispatcher(registry);

         using (Mocks.Record())
         {
            SetupResult.For(registry.FindCommand("blah")).Return(mockCommand);
            mockCommand.Execute();
         }

         using (Mocks.Playback())
         {
            string[] args = new string[] { "blah" };
            dispatch.Forward(args);
         }
      }
   }
}