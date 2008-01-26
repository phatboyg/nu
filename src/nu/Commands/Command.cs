namespace nu.Commands
{
   public abstract class Command : ICommand
   {
      public virtual void Route(string[] args)
      {
         Parser.ParseArgumentsWithUsage(args, this);
         Execute();
      }

      public abstract void Execute();

      public virtual string Name
      {
         get { return this.GetType().Name.Replace("Command", ""); }
      }
   }
}