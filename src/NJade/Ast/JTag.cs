using System.Collections.Generic;
using System.Linq;

namespace NJade.Ast
{
	internal class JTag : JNode
	{
		private readonly string _name;
		private readonly IEnumerable<JNode> _kids;

		public JTag(string name, IEnumerable<JNode> kids)
		{
			_name = name;
			_kids = kids ?? Enumerable.Empty<JNode>();
		}

		public override IEnumerable<JNode> Children()
		{
			return _kids;
		}

		public override void Render(RenderContext context)
		{
			context.Output.WriteStartElement(_name);

			foreach (var kid in _kids)
			{
				kid.Render(context);
			}

			context.Output.WriteEndElement();
		}
	}
}
