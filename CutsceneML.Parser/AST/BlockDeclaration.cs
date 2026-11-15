using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class BlockDeclaration : Declaration {
	public required Identifier BlockKeyword { get; init; }
	public required Identifier Name { get; init; }
	public required BlockBody Body { get; init; }

	public override ITreeNode[] Children => [BlockKeyword, Name, Body];
}