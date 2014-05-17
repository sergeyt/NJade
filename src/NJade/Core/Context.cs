using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace NJade.Core
{
	/// <summary>
	/// Represents context of the templating engine. Implements data scope stack and evaluation of path expressions.
	/// </summary>
	internal sealed class Context
	{
		private sealed class Scope
		{
			public Scope(object target, Scope parent)
			{
				Parent = parent;
				Target = target;
			}

			public Scope Parent { get; private set; }

			public object Target { get; private set; }
		}

		private readonly Dictionary<object,Dictionary<string,object>> _targetValueCache = new Dictionary<object, Dictionary<string, object>>();
		private readonly Stack<Scope> _scopeStack = new Stack<Scope>();

		/// <summary>
		/// Initializes a new instance of the <see cref="Context"/> class.
		/// </summary>
		/// <param name="root">The root data object.</param>
		public Context(object root)
		{
			_scopeStack.Push(new Scope(root, null));
		}

		//TODO: cache expression results per target object
		public object GetValue(string expression)
		{
			if (string.IsNullOrEmpty(expression))
			{
				return null;
			}

			if (expression == "." || expression == "this")
			{
				return _scopeStack.Peek().Target;
			}

			return (from scope in _scopeStack
			        where scope != null
			        select Eval(scope, expression))
				.FirstOrDefault(value => !ReferenceEquals(value, Evaluator.NoValue));
		}

		//TODO: support System.Data collections
		public IEnumerable<object> GetValues(string path)
		{
			object value = GetValue(path);

			if (value is bool)
			{
				if ((bool)value)
				{
					yield return value;
				}
			}
			else if (value is string)
			{
				if (!string.IsNullOrEmpty((string)value))
				{
					yield return value;
				}
			}
			else if (value is IDictionary) // Dictionaries also implement IEnumerable so this has to be checked before it.
			{
				if (((IDictionary)value).Count > 0)
				{
					yield return value;
				}
			}
			else if (value is IEnumerable)
			{
				foreach (var item in ((IEnumerable)value))
				{
					yield return item;
				}
			}
			else if (value is IDataReader)
			{
				var reader = value as IDataReader;
				do
				{
					while (reader.Read())
					{
						yield return new CachedDataRecord(reader);
					}
				} while (reader.NextResult());
			}
			else if (value != null)
			{
				yield return value;
			}
		}

		/// <summary>
		/// Pushes new data scope.
		/// </summary>
		/// <param name="scope">The data scope to push.</param>
		public void Push(object scope)
		{
			if (scope == null) throw new ArgumentNullException("scope");

			var parent = _scopeStack.Count > 0 ? _scopeStack.Peek() : null;
			_scopeStack.Push(new Scope(scope, parent));
		}

		/// <summary>
		/// Pops data scope.
		/// </summary>
		public void Pop()
		{
			_scopeStack.Pop();
		}

		private object Eval(Scope scope, string path)
		{
			object value = null;

			foreach (var item in SplitPath(path))
			{
				if (item.Key == PathKind.Parent)
				{
					scope = scope.Parent;
				}
				else
				{
					value = GetTargetValue(scope.Target, item.Value);

					if (value.IsNullOrNoValue())
					{
						break;
					}

					scope = new Scope(value, scope);
				}
			}

			return value;
		}

		private object GetTargetValue(object target, string name)
		{
			if (string.IsNullOrEmpty(name))
			{
				return null;
			}

			object value;
			Dictionary<string, object> cache;
			if (_targetValueCache.TryGetValue(target, out cache))
			{
				if (cache.TryGetValue(name, out value))
				{
					return value;
				}
			}
			else
			{
				cache = null;
			}

			value = target.Eval(name);

			if (value.IsNullOrNoValue())
			{
				return value;
			}

			if (cache == null)
			{
				cache = new Dictionary<string, object>();
				_targetValueCache.Add(target, cache);
			}

			cache.Add(name, value);

			return value;
		}

		private enum PathKind
		{
			Name,
			Parent,
		}

		private static IEnumerable<KeyValuePair<PathKind,string>> SplitPath(string path)
		{
			int start = 0;
			for (int i = 0; i < path.Length; i++)
			{
				if (path[i] == '.')
				{
					// '../' segment gets parent scope
					if (i + 2 < path.Length && path[i + 1] == '.' && path[i + 2] == '/')
					{
						if (start < i)
						{
							var name = path.Substring(start, i - start);
							yield return new KeyValuePair<PathKind, string>(PathKind.Name, name);
						}
						yield return new KeyValuePair<PathKind, string>(PathKind.Parent, null);
						start = i + 3;
						i += 2;
					}
					else
					{
						var name = path.Substring(start, i - start);
						yield return new KeyValuePair<PathKind, string>(PathKind.Name, name);
						start = i + 1;
					}
				}
			}
			if (start < path.Length)
			{
				var name = path.Substring(start);
				yield return new KeyValuePair<PathKind, string>(PathKind.Name, name);
			}
		}
	}
}
