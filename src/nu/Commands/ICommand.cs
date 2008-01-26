namespace nu.Commands
{
    public interface ICommand
    {
        string Name{ get;}
        void Route(string[] args);
        void Execute();
    }
}