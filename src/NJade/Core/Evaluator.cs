using System;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace NJade.Core
{
	//DONE: support basic System.Data objects
	//TODO: support indexers, functions and other C# expressions
	//TODO: use DynamicMethod to optimize expression evaluation

	internal static class Evaluator
	{
		public static bool IsFalse(this object value)
		{
			if (value is bool)
			{
				return (bool)value == false;
			}
			if (value is string)
			{
				return ((string)value).Length == 0;
			}
			var set = value as IEnumerable;
			if (set != null)
			{
				return !set.GetEnumerator().MoveNext();
			}
			return value.IsNullOrNoValue();
		}

		public static readonly object NoValue = new object();

		public static object Eval(this object target, string name)
		{
			return Getters.Select(getter => getter(target, name)).Where(value => value != NextGetter).First();
		}

		private static readonly object NextGetter = new object();

		private static readonly Func<object, string, object>[] Getters =
			{
				DictionaryGetter,
				XmlGetter,
				XLinqGetter,
				SystemDataGetter,
				PropertyDescriptorGetter,
				PropertyGetter,
				FieldGetter,
				MethodGetter,
				NoValueGetter
			};

		#region Xml

		private static object XmlGetter(object target, string name)
		{
			var node = target as XmlNode;
			if (node == null)
			{
				return NextGetter;
			}

			if (name[0] == '@')
			{
				if (node.Attributes != null)
				{
					var attr = node.Attributes.GetNamedItem(name.Substring(1));
					return attr != null ? attr.Value : NoValue;
				}
			}
			else
			{
				var list = node.SelectNodes(name);
				if (list != null && list.Count > 0)
				{
					return list;
				}
			}

			return NoValue;
		}

		private static object XLinqGetter(object target, string expression)
		{
			var node = target as XNode;
			if (node != null)
			{
				var e = target as XElement;
				if (e != null)
				{
					if (expression[0] == '@')
					{
						var attr = e.Attribute(expression.Substring(1));
						return attr != null ? attr.Value : NoValue;
					}
				}
				return node.XPathEvaluate(expression);
			}

			return NextGetter;
		}

		#endregion

		private static object DictionaryGetter(object target, string name)
		{
			var dictionary = target as IDictionary;
			return dictionary == null
			       	? NextGetter
			       	: (dictionary.Contains(name)
			       	   	? dictionary[name]
			       	   	: NoValue);
		}

		#region System.Data

		private static object SystemDataGetter(object target, string name)
		{
			var record = target as IDataRecord;
			if (record != null)
			{
				//TODO: FieldCount
				int ordinal = record.GetOrdinal(name);
				return ordinal >= 0 ? record.GetValue(ordinal) : NoValue;
			}

			var row = target as DataRow;
			if (row != null)
			{
				try
				{
					return row[name];
				}
				catch (Exception)
				{
					return NoValue;
				}
			}

			return NextGetter;
		}

		#endregion

		#region Reflection

		private const BindingFlags DefaultBindingFlags = BindingFlags.Public | BindingFlags.Instance;

		private static object PropertyDescriptorGetter(object target, string name)
		{
			var typeDescriptor = target as ICustomTypeDescriptor;
			if (typeDescriptor == null)
			{
				return NextGetter;
			}

			var descriptor = typeDescriptor.GetProperties()[name];
			return descriptor != null ? descriptor.GetValue(target) : NoValue;
		}

		private static object MethodGetter(object target, string name)
		{
			var methods = target.GetType().GetMember(name, MemberTypes.Method, DefaultBindingFlags);

			var method = (from MethodInfo m in methods
			              where m.ReturnType != typeof(void) && m.GetParameters().Length == 0
			              select m).FirstOrDefault();

			return method != null ? method.Invoke(target, null) : NextGetter;
		}

		private static object PropertyGetter(object target, string name)
		{
			var property = target.GetType().GetProperty(name, DefaultBindingFlags);
			return property == null ? NextGetter : (property.CanRead ? property.GetValue(target, null) : NoValue);
		}

		private static object FieldGetter(object target, string name)
		{
			var field = target.GetType().GetField(name, DefaultBindingFlags);
			return field != null ? field.GetValue(target) : NextGetter;
		}

		#endregion

		private static object NoValueGetter(object target, string name)
		{
			return NoValue;
		}

		public static bool IsNullOrNoValue(this object value)
		{
			return value == null || ReferenceEquals(value, NoValue);
		}
	}
}
