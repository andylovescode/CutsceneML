using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class BlockDeclarationParselet : Parselet<BlockDeclaration> {
	public static readonly BlockDeclarationParselet Instance = new();

	public override BlockDeclaration Read(Cursor cursor) {
		Identifier blockKeyword = cursor.GetKeyword("block");
		Identifier name = cursor.GetIdentifier();
		BlockBody body = BlockBodyParselet.Instance.Read(cursor);

		return new BlockDeclaration {BlockKeyword = blockKeyword, Name = name, Body = body};
	}
}