using System.Collections.Generic;
using NJade.Core;
using NUnit.Framework;
#if NUNIT

namespace NJade.Tests
{
	[TestFixture]
	public class EvalTests
	{
		[Test]
		public void IsFalse()
		{
			object value = null;
			Assert.IsTrue(value.IsFalse());

			Assert.IsTrue(false.IsFalse());
			Assert.IsFalse(true.IsFalse());

			Assert.IsTrue("".IsFalse());

			Assert.IsTrue(new int[0].IsFalse());
			Assert.IsFalse(new[] {1}.IsFalse());
		}

		[TestCase(null)]
		[TestCase("abc")]
		[TestCase(1)]
		[TestCase(false)]
		[TestCase(true)]
		public void Dictionary(object value)
		{
			var data = new Dictionary<string, object>
			           	{
			           		{"Value", value}
			           	};
			var result = data.Eval("Value");
			Assert.AreEqual(value, result);
		}

		#region Reflection

		[TestCase(null)]
		[TestCase("abc")]
		[TestCase(1)]
		[TestCase(false)]
		[TestCase(true)]
		public void Property(object value)
		{
			var data = new {Value = value};
			var result = data.Eval("Value");
			Assert.AreEqual(value, result);
		}

		[TestCase(null)]
		[TestCase("abc")]
		[TestCase(1)]
		[TestCase(false)]
		[TestCase(true)]
		public void Method(object value)
		{
			var result = new Data1(value).Eval("Value");
			Assert.AreEqual(value, result);
		}

		[TestCase(null)]
		[TestCase("abc")]
		[TestCase(1)]
		[TestCase(false)]
		[TestCase(true)]
		public void Field(object value)
		{
			var result = new Data2(value).Eval("Value");
			Assert.AreEqual(value, result);
		}

		private class Data1
		{
			private readonly object _value;

			public Data1(object value)
			{
				_value = value;
			}

			public object Value()
			{
				return _value;
			}
		}

		private class Data2
		{
			public readonly object Value;

			public Data2(object value)
			{
				Value = value;
			}
		}

		#endregion
	}
}
#endif