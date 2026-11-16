using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class Parselets {
	public static readonly Parselet<SayStatement> SayStatement =
		Parselet.Of(cursor => {
			SpeechLiteral speech = cursor.GetSpeech();

			return new SayStatement { Speech = speech };
		});

	public static readonly Parselet<ChangeActorStatement> ChangeActorStatement =
		Parselet.Of(cursor => {
			Identifier actorName = cursor.GetIdentifier();
			SymbolLiteral colon = cursor.GetSymbol(':');

			return new ChangeActorStatement {
				ActorName = actorName,
				Colon = colon,
			};
		});

	public static readonly Parselet<BlockBody> BlockBody =
		Parselet.Of(cursor => {
			SymbolLiteral openBracket = cursor.GetSymbol('{');
			Statement[] statements =
				Parselet.ListOf(Statement!, '}').Read(cursor);
			SymbolLiteral closeBracket = cursor.GetSymbol('}');

			return new BlockBody {
				StartBracket = openBracket,
				Statements = statements,
				EndBracket = closeBracket,
			};
		});

	public static readonly Parselet<BlockDeclaration> BlockDeclaration =
		Parselet.Of<BlockDeclaration>(cursor => {
			Identifier blockKeyword = cursor.GetKeyword("block");
			Identifier name = cursor.GetIdentifier();
			BlockBody body = BlockBody.Read(cursor);

			return new BlockDeclaration {
				BlockKeyword = blockKeyword,
				Name = name,
				Body = body,
			};
		});

	public static readonly Parselet<Declaration> Declaration =
		Parselet.OneOf<Declaration>(BlockDeclaration);

	public static readonly Parselet<AnonymousBlockRef> AnonymousBlockRef =
		Parselet.Of<AnonymousBlockRef>(cursor => {
			BlockBody body = BlockBody.Read(cursor);
			return new AnonymousBlockRef { Body = body };
		});

	public static readonly Parselet<NamedBlockRef> NamedBlockRef =
		Parselet.Of<NamedBlockRef>(cursor => {
			Identifier ident = cursor.GetIdentifier();
			return new NamedBlockRef { Name = ident };
		});

	public static readonly Parselet<BlockRef> BlockRef =
		Parselet.OneOf<BlockRef>(AnonymousBlockRef, NamedBlockRef);

	public static readonly Parselet<FlagCondition> FlagCondition =
		Parselet.Of<FlagCondition>(cursor => {
			Identifier flagKeyword = cursor.GetKeyword("flag");
			Identifier flagName = cursor.GetIdentifier();

			return new FlagCondition {
				FlagKeyword = flagKeyword,
				Name = flagName,
			};
		});

	public static readonly Parselet<Condition> Condition =
		Parselet.OneOf<Condition>(FlagCondition);

	public static readonly Parselet<IfStatement> IfStatement =
		Parselet.Of<IfStatement>(cursor => {
			Identifier ifKeyword = cursor.GetKeyword("if");
			Condition condition = Condition.Read(cursor);
			BlockRef block = BlockRef.Read(cursor);
			return new IfStatement {
				Condition = condition,
				Block = block,
				IfKeyword = ifKeyword,
			};
		});

	public static readonly Parselet<Statement> Statement =
		Parselet.OneOf<Statement>(SayStatement, ChangeActorStatement,
			IfStatement);

	public static readonly Parselet<File> File = Parselet.Of(cursor => {
		List<Declaration> decls = [];

		while (!cursor.Eof) {
			decls.Add(Declaration.Read(cursor));
		}

		return new File { Declarations = [.. decls] };
	});
}