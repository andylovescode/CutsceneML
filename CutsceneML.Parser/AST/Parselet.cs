namespace CutsceneML.Parser.AST;

public abstract class Parselet<TOutput> : IParses<TOutput> {
	public abstract TOutput Read(Cursor cursor);
}

public static class Parselet {
	public static OneOfParselet<TUnion> OneOf<TUnion>(
		params IParses<TUnion>[] parselets) =>
		new() { Subparselets = parselets };

	public static ListParselet<TList> ListOf<TList>(
		Parselet<TList> itemParselet, char endSymbol) =>
		new() { ItemParselet = itemParselet, EndSymbol = endSymbol };

	public static FunctionalParselet<TOutput>
		Of<TOutput>(Func<Cursor, TOutput> parser) =>
		new() { Parser = parser };
}