namespace nu.Model.ArgumentParsing.Exceptions
{
    using System;
    using System.Collections.Generic;

    public class MissingRequiredArgumentsException : ApplicationException
    {
        private readonly IList<ArgumentTarget> _requiredArgs;

        public MissingRequiredArgumentsException(IList<ArgumentTarget> requiredArgs)
        {
            _requiredArgs = requiredArgs;
        }

        public IList<ArgumentTarget> ArgumentsMissing
        {
            get { return _requiredArgs; }
        }
    }
}