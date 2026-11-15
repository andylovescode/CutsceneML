namespace CutsceneML.Parser.Lex;

public class SymbolLiteral : Token {
	public required char Value { get; init; }

	public override string ToString() => $"'{Value}'";
}