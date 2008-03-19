using System;
using System.Collections.Generic;

namespace nu.Utility.Exceptions
{
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