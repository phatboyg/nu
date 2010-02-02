namespace nu.core.Commands
{
	using System.Collections.Generic;
	using Model.ArgumentParsing;

	public interface IOldCommand
	{
		void Execute(IEnumerable<IArgument> arguments);
	}
}