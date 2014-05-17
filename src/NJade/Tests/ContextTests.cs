#if NUNIT
using NJade.Core;
using NUnit.Framework;

namespace NJade.Tests
{
	/// <summary>
	/// Tests for <see cref="Context"/>.
	/// </summary>
	[TestFixture]
	public class ContextTests
	{
		[Test]
		public void CheckValueCaching()
		{
			const string name = "abc";
			var root = new Item(name);
			var context = new Context(root);
			for (int i = 0; i < 2; i++)
			{
				Assert.AreEqual(name, context.GetValue("Name"));
				Assert.AreEqual(1, root.NameCalls);
			}
		}

		private sealed class Item
		{
			private readonly string _name;
			private int _nameCalls;

			public Item(string name)
			{
				_name = name;
			}

			public int NameCalls
			{
				get { return _nameCalls; }
			}

			public string Name
			{
				get
				{
					++_nameCalls;
					return _name;
				}
			}
		}
	}
}
#endif