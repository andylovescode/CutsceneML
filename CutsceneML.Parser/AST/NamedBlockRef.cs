using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class NamedBlockRef : BlockRef {
	public required Identifier Name { get; init; }

	public override ITreeNode[] Children => [Name];
}