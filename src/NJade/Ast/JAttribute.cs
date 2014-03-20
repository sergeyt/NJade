namespace NJade.Ast
{
	internal class JAttribute : JNode
	{
		private readonly string _name;
		private readonly string _expression;

		public JAttribute(string name, string expression)
		{
			_name = name;
			_expression = expression;
		}

		public override void Render(RenderContext context)
		{
			var value = context.EvalString(_expression);
			context.Output.WriteAttributeString(_name, value);
		}
	}
}
