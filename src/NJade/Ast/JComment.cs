namespace NJade.Ast
{
	internal class JComment : JNode
	{
		private readonly string _text;

		public JComment(string text)
		{
			_text = text;
		}

		public override void Render(RenderContext context)
		{
			// buffered comment is not rendered
			if (_text.StartsWith("-")) return;

			context.Output.WriteComment(_text);
		}
	}
}
