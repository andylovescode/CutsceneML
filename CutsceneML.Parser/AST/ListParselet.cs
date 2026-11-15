using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class ListParselet<T> : Parselet<T[]> {
	public required Parselet<T> ItemParselet { get; init; }
	public required char EndSymbol { get; init; }

	public override T[] Read(Cursor cursor) {
		List<T> items = [];
		while (!cursor.Eof && !(cursor.Tokens[cursor.Location] is SymbolLiteral lit && lit.Value == EndSymbol)) {
			items.Add(ItemParselet.Read(cursor));
		}

		if (cursor.Eof) {
			cursor.RaiseError(cursor.Tokens[^1], "Unexpected end of file when reading list");
		}

		return [.. items];
	}
}