using System;

namespace nu.Utility
{
    /// <summary>
    /// Specifies the target field or property as an argument
    /// </summary>
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class ArgumentAttribute : 
        Attribute
    {
        private bool _required;
        private string _defaultValue;

        /// <summary>
        /// True is the argument is required to exist
        /// </summary>
        public bool Required
        {
            get { return _required; }
            set { _required = value; }
        }

        public string DefaultValue
        {
            get { return _defaultValue; }
            set { _defaultValue = value; }
        }
    }

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class DefaultArgumentAttribute : 
        ArgumentAttribute
    {
    }
}