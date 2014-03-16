using System;
using System.IO;

namespace NJade
{
	public class JadeViewEngine
    {
	    public Func<object, string> Compile(TextReader input)
	    {
		    if (input == null) throw new ArgumentNullException("input");

		    throw new NotImplementedException();
	    }
    }
}
