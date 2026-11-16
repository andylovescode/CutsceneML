using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class BlockBody : Tree {
	public required SymbolLiteral StartBracket { get; init; }
	public required Statement[] Statements { get; init; }
	public required SymbolLiteral EndBracket { get; init; }

	public override ITreeNode[] Children =>
		[StartBracket, .. Statements, EndBracket];
}