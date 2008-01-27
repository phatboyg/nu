namespace nu.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Text;

    public class ArgumentMapFactory : IArgumentMapFactory
    {
        #region IArgumentMapFactory Members

        public IArgumentMap CreateMap(object obj)
        {
            return new ArgumentMap(obj.GetType());
        }

        #endregion
    }
}