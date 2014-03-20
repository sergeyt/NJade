using System.Collections.Generic;
using System.Linq;

namespace NJade.Ast
{
	internal class JadeTemplate : JNode
	{
		private IEnumerable<JNode> _kids;

		public JadeTemplate(IEnumerable<JNode> kids)
		{
			_kids = kids ?? Enumerable.Empty<JNode>();
		}

		public override void Render(RenderContext context)
		{
			foreach (var kid in _kids)
			{
				kid.Render(context);
			}
		}
	}
}
