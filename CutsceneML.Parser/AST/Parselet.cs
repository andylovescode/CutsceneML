namespace CutsceneML.Parser.AST;

public abstract class Parselet<TOutput> : IParses<TOutput> {
	public abstract TOutput Read(Cursor cursor);

	public OneOfParselet<TUnion> OneOf<TUnion>(params Parselet<TUnion>[] parselets) =>
		new() { Subparselets = parselets };

	public ListParselet<TList> ListOf<TList>(Parselet<TList> itemParselet, char endSymbol) =>
		new() { ItemParselet = itemParselet, EndSymbol = endSymbol };
}