namespace nu.Utility
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Reflection;
    using System.Text;

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
                        _targets.Add(new DefaultArgumentTarget(attribute, property));
                    }
                    else
                    {
                        _targets.Add(new ArgumentTarget(attribute, property));
                    }
                }
            }
        }

        #region IArgumentMap Members

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

        public string Usage
        {
            get
            {
                StringBuilder sb = new StringBuilder();

                _targets.ForEach(
                    delegate(ArgumentTarget target)
                        {
                            if (target is DefaultArgumentTarget)
                            {
                                sb.AppendFormat("{1}<{0}>{2} ", target.Property.Name, target.Attribute.Required ? "" : "[", target.Attribute.Required ? "" : "]");
                            }
                            else if (target.Attribute.Required)
                                sb.AppendFormat("-{0} ", target.Attribute.Key);
                            else
                                sb.AppendFormat("[-{0}] ", target.Attribute.Key);
                        });

                sb.Append(Environment.NewLine + Environment.NewLine);

                _targets.ForEach(
                    delegate(ArgumentTarget target)
                        {
                            if (target is DefaultArgumentTarget)
                            {
                                sb.AppendFormat("{0,-20}{1}" + Environment.NewLine, "<" + target.Property.Name + ">",
                                                target.Attribute.Description);
                            }
                            else
                            {
                                sb.AppendFormat("{0,-20}{1}" + Environment.NewLine, target.Attribute.Key,
                                                target.Attribute.Description);
                            }
                        });

                return sb.ToString();
            }
        }

        #endregion
    }
}