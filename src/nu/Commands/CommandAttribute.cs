namespace nu.Commands
{
    using System;

    [AttributeUsage(AttributeTargets.Class)]
    public class CommandAttribute : Attribute
    {
        private string _description;

        /// <summary>
        /// A description for the command
        /// </summary>
        public string Description
        {
            get { return _description; }
            set { _description = value; }
        }
    }
}