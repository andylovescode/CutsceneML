using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class ChoiceStatement : Statement {
	public required Identifier ChoiceKeyword { get; init; }
	public required SymbolLiteral OpeningBrace { get; init; }
	public required Choice[] Choices { get; init; }
	public required SymbolLiteral ClosingBrace { get; init; }

	public override ITreeNode[] Children => [
		ChoiceKeyword, OpeningBrace, .. Choices, ClosingBrace,
	];
}