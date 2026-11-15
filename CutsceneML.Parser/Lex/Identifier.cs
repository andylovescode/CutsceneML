namespace CutsceneML.Parser.Lex;

public class Identifier : Token {
	public required string Value { get; init; }

	public override string ToString() => $"'{Value}'";
}