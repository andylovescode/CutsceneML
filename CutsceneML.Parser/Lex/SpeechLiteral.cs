namespace CutsceneML.Parser.Lex;

public class SpeechLiteral : Token {
	public required string Value { get; init; }

	public override string ToString() => $"\"{Value}\"";
}