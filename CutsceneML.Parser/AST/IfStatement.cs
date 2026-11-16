using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class IfStatement : Statement {
	public required Identifier IfKeyword { get; init; }
	public required Condition Condition { get; init; }
	public required BlockRef Block { get; init; }
	public override ITreeNode[] Children => [IfKeyword, Condition, Block];
}