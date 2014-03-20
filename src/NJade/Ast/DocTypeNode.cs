namespace NJade.Ast
{
	internal class DocTypeNode : JNode
	{
		public override void Render(RenderContext context)
		{
			context.Output.WriteDocType("html", null, null, null);
		}
	}
}
