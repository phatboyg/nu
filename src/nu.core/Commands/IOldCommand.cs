namespace nu.core.Commands
{
	using System.Collections.Generic;
	using nu.Model.ArgumentParsing;

    public interface IOldCommand
	{
		void Execute(IEnumerable<IArgument> arguments);
	}
}