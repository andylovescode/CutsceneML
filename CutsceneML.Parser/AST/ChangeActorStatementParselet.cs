using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class ChangeActorStatementParselet : Parselet<ChangeActorStatement> {
	public static readonly ChangeActorStatementParselet Instance = new();

	public override ChangeActorStatement Read(Cursor cursor) {
		Identifier actorName = cursor.GetIdentifier();
		SymbolLiteral colon = cursor.GetSymbol(':');

		return new ChangeActorStatement {ActorName = actorName, Colon = colon};
	}
}