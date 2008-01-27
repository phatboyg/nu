namespace nu
{
    using System.Collections.Generic;
    using Commands;
    using Utility;

    public class Dispatcher : IDispatcher
    {
        private readonly IArgumentMapFactory _argumentMapFactory;
        private readonly IArgumentParser _argumentParser;

        private string _commandName = "help";

        public Dispatcher(IArgumentParser argumentParser, IArgumentMapFactory argumentMapFactory)
        {
            _argumentParser = argumentParser;
            _argumentMapFactory = argumentMapFactory;
        }

        [DefaultArgument(DefaultValue = "help")]
        public string CommandName
        {
            get { return _commandName; }
            set { _commandName = value; }
        }

        #region IDispatcher Members

        public void Forward(string[] args)
        {
            IList<IArgument> argumentList = _argumentParser.Parse(args);

            IEnumerator<IArgument> argumentEnumerator = argumentList.GetEnumerator();
            
            IArgumentMap dispatcherMap = _argumentMapFactory.CreateMap(this);

            dispatcherMap.ApplyTo(this, argumentEnumerator);

            ICommand command = IoC.Resolve<ICommand>(_commandName);

            IArgumentMap commandMap = _argumentMapFactory.CreateMap(command);

            commandMap.ApplyTo(command, argumentEnumerator);

            command.Execute(argumentEnumerator);
        }

        #endregion
    }
}