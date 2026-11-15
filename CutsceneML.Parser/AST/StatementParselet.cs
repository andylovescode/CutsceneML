namespace CutsceneML.Parser.AST;

public class StatementParselet {
	public static readonly OneOfParselet<Statement> Instance = new() {
		Subparselets = [ChangeActorStatementParselet.Instance, SayStatementParselet.Instance],
	};
}