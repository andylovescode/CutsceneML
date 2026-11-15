namespace CutsceneML.Parser.Lex;

public class NumberLiteral : Token {
	public required double Value { get; init; }

	public override string ToString() => $"{Value}";
}