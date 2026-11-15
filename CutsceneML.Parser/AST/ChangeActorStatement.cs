using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class ChangeActorStatement : Statement {
	public required Identifier ActorName { get; init; }
	public required SymbolLiteral Colon { get; init; }

	public override ITreeNode[] Children => [ActorName, Colon];
}