namespace CutsceneML.Parser.AST.Impl;

public class FunctionalParselet<TOutput> : Parselet<TOutput> {
	public required Func<Cursor, TOutput> Parser { get; init; }

	public override TOutput Read(Cursor cursor) => Parser(cursor);
}