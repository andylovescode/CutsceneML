using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class FlagCondition : Condition {
	public required Identifier FlagKeyword { get; init; }
	public required Identifier Name { get; init; }
	public override ITreeNode[] Children => [FlagKeyword, Name];
}