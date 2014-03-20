using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace NJade.Ast
{
	internal abstract class JNode
	{
		public virtual IEnumerable<JNode> Children()
		{
			return Enumerable.Empty<JNode>();
		}

		public abstract void Render(RenderContext context);
	}

	internal class RenderContext
	{
		public object Data { get; private set; }
		public XmlWriter Output { get; private set; }

		public string EvalString(string expression)
		{
			throw new System.NotImplementedException();
		}
	}
}
