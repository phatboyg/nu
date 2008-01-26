namespace nu
{
   using Commands;

   public class Dispatcher : IDispatcher
   {

      public void Forward(string[] args)
      {
         string commandName = args[0];
         ICommand command = IoC.Resolve<ICommand>(commandName);
      }
      
   }
}