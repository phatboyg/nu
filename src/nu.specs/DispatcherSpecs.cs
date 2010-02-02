namespace Specs_for_Dispatcher
{
   using Castle.MicroKernel;
   using Castle.Windsor;
   using nu;
   using nu.Commands;
   using nu.core.Commands;
   using nu.Model.ArgumentParsing;
   using nu.Utility;
   using NUnit.Framework;
   using Rhino.Mocks;
   using XF.Specs;

   [TestFixture]
   public class When_a_command_cannot_be_found : Spec
   {
      private Dispatcher _dispatcher;
      private string[] _bogusArguments;
      private IWindsorContainer _mockContainer;
      private IOldCommand _mockHelpOldCommand;

      protected override void Before_each_spec()
      {
         _dispatcher = Create<Dispatcher>();
         _bogusArguments = new string[] {"no-such-command"};
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
            .Return(new ArgumentMap(typeof (Dispatcher)));

         SetupResult.For(Get<IArgumentMapFactory>().CreateMap(_mockHelpOldCommand))
            .Return(new ArgumentMap(typeof (IOldCommand)));
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
   }
}