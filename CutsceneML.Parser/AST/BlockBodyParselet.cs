using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class BlockBodyParselet : Parselet<BlockBody> {
	public static readonly BlockBodyParselet Instance = new();

	public override BlockBody Read(Cursor cursor) {
		SymbolLiteral openBracket = cursor.GetSymbol('{');
		Statement[] statements = ListOf(StatementParselet.Instance, '}').Read(cursor);
		SymbolLiteral closeBracket = cursor.GetSymbol('}');

		return new BlockBody {StartBracket = openBracket, Statements = statements, EndBracket = closeBracket};
	}
}