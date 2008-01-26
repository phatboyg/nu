namespace nu.Commands
{
    using System;
    using Castle.MicroKernel;

    /// <summary>
   /// Lists all currently registered commands if a name is supplied and their usage.
   /// </summary>
   public class HelpCommand : Command
   {
       public override void Execute()
       {
           Console.WriteLine("Commands currently registered:" + Environment.NewLine);

           foreach (IHandler handler in IoC.Container.Kernel.GetAssignableHandlers(typeof (ICommand)))
           {
               ICommand cmd = (ICommand) handler.Resolve(CreationContext.Empty);
               string commandName = cmd.Name;
               if (!String.IsNullOrEmpty(commandName))
               {
                   Console.WriteLine("{0} command:", commandName);
                   Console.WriteLine(Parser.ArgumentsUsage(cmd.GetType()));
               }
           }
       }

       public override string Name 
       {
           get {return "Help";}
       }
   }
}