namespace nu.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;

    public class ArgumentMapFactory : IArgumentMapFactory
    {
        #region IArgumentMapFactory Members

        public IArgumentMap CreateMap(object obj)
        {
            return new ArgumentMap(obj.GetType());
        }

        #endregion
    }

    public class ArgumentMap : IArgumentMap
    {
        private readonly List<ArgumentTarget> _targets = new List<ArgumentTarget>();
        private readonly Type _type;

        public ArgumentMap(Type type)
        {
            _type = type;

            foreach (PropertyInfo property in _type.GetProperties())
            {
                object[] attributes = property.GetCustomAttributes(typeof (ArgumentAttribute), true);
                if (attributes.Length == 1)
                {
                    ArgumentAttribute attribute = (ArgumentAttribute) attributes[0];

                    if (attribute is DefaultArgumentAttribute)
                    {
                        _targets.Add(new ArgumentTarget(attribute, property));
                    }
                    else
                    {
                        _targets.Add(new ArgumentTarget(attribute, property));
                    }
                }
            }
        }

        #region IArgumentMap<T> Members

        public void ApplyTo(object obj, IEnumerator<IArgument> arguments)
        {
            for (int index = 0; index < _targets.Count && arguments.MoveNext(); index++)
            {
                object value;

                if (_targets[index].Property.PropertyType == typeof (bool))
                {
                    value = bool.Parse(arguments.Current.Value);
                }
                else
                {
                    value = arguments.Current.Value;
                }

                _targets[index].Property.SetValue(obj, value, BindingFlags.Default, null, null,
                                                  CultureInfo.InvariantCulture);
            }
        }

        #endregion
    }
}