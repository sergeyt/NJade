using System;
using System.IO;

namespace NJade
{
	public static class Jade
    {
		public static Func<object, string> Compile(TextReader input)
		{
			if (input == null) throw new ArgumentNullException("input");

			throw new NotImplementedException();
		}
    }
}
