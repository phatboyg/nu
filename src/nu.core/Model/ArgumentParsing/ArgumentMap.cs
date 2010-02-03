// Copyright 2007-2008 The Apache Software Foundation.
//  
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use 
// this file except in compliance with the License. You may obtain a copy of the 
// License at 
// 
//     http://www.apache.org/licenses/LICENSE-2.0 
// 
// Unless required by applicable law or agreed to in writing, software distributed 
// under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
// CONDITIONS OF ANY KIND, either express or implied. See the License for the 
// specific language governing permissions and limitations under the License.
namespace nu.core.Model.ArgumentParsing
{
	using System;
	using System.Collections.Generic;
	using System.Globalization;
	using System.Reflection;
	using System.Text;
	using Exceptions;

	public class ArgumentMap : IArgumentMap
	{
		readonly Dictionary<string, ArgumentTarget> _namedArgs = new Dictionary<string, ArgumentTarget>();
		readonly List<ArgumentTarget> _requiredArgs = new List<ArgumentTarget>();
		readonly Type _type;
		readonly List<ArgumentTarget> _unnamedArgs = new List<ArgumentTarget>();

		public ArgumentMap(Type type)
		{
			_type = type;

			foreach (PropertyInfo property in _type.GetProperties())
			{
				object[] attributes = property.GetCustomAttributes(typeof(ArgumentAttribute), true);
				if (attributes.Length == 1)
				{
					var attribute = (ArgumentAttribute)attributes[0];
					var defaultAttribute = attribute as DefaultArgumentAttribute;

					ArgumentTarget arg = (defaultAttribute == null)
					                     	? new ArgumentTarget(attribute, property)
					                     	: new DefaultArgumentTarget(attribute, property);

					if (string.IsNullOrEmpty(arg.Attribute.Key))
						_unnamedArgs.Add(arg);
					else
						_namedArgs.Add(arg.Attribute.Key, arg);

					if (arg.Attribute.Required)
					{
						_requiredArgs.Add(arg);
					}
				}
			}
		}

		#region IArgumentMap Members

		public string Usage
		{
			get
			{
				var sb = new StringBuilder();

				var allArgs = new List<ArgumentTarget>();
				allArgs.AddRange(_namedArgs.Values);
				allArgs.AddRange(_unnamedArgs);

				foreach (ArgumentTarget target in allArgs)
				{
					if (target is DefaultArgumentTarget)
					{
						sb.AppendFormat("{1}<{0}>{2} ", target.Property.Name, target.Attribute.Required ? "" : "[",
							target.Attribute.Required ? "" : "]");
					}
					else if (target.Attribute.Required)
						sb.AppendFormat("-{0} ", target.Attribute.Key);
					else
						sb.AppendFormat("[-{0}] ", target.Attribute.Key);
				}

				sb.Append(Environment.NewLine + Environment.NewLine);

				foreach (ArgumentTarget target in allArgs)
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
				}

				return sb.ToString();
			}
		}

		public IEnumerable<IArgument> ApplyTo(object obj, IEnumerable<IArgument> arguments)
		{
			var unused = new List<IArgument>();

			int unnamedIndex = 0;

			foreach (IArgument arg in arguments)
			{
				if (string.IsNullOrEmpty(arg.Key))
				{
					if (unnamedIndex < _unnamedArgs.Count)
					{
						PropertyInfo unnamedProperty = _unnamedArgs[unnamedIndex++].Property;
						ApplyValueToProperty(unnamedProperty, obj, arg.Value);
						RemoveRequiredPropertyTracking(unnamedProperty);
					}
					else
					{
						unused.Add(arg);
					}
				}
				else if (_namedArgs.ContainsKey(arg.Key))
				{
					PropertyInfo namedProperty = _namedArgs[arg.Key].Property;
					ApplyValueToProperty(namedProperty, obj, arg.Value);
					RemoveRequiredPropertyTracking(namedProperty);
				}
				else
				{
					unused.Add(arg);
				}
			}

			if (_requiredArgs.Count > 0)
				throw new MissingRequiredArgumentsException(_requiredArgs);

			return unused;
		}

		#endregion

		public void ApplyValueToProperty(PropertyInfo property, object obj, string argumentValue)
		{
			object value;

			if (property.PropertyType == typeof(bool))
			{
				value = bool.Parse(argumentValue);
			}
			else
			{
				value = argumentValue;
			}

			property.SetValue(obj, value, BindingFlags.Default, null, null, CultureInfo.InvariantCulture);
		}

		void RemoveRequiredPropertyTracking(PropertyInfo appliedProperty)
		{
			_requiredArgs.ForEach(delegate(ArgumentTarget target)
				{
					if (string.Compare(appliedProperty.Name, target.Property.Name, true) == 0)
					{
						_requiredArgs.Remove(target);
					}
				});
		}
	}
}