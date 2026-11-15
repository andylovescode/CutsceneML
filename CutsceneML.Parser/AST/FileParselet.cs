namespace CutsceneML.Parser.AST;

public class FileParselet : Parselet<File> {
	public static readonly FileParselet Instance = new();

	public override File Read(Cursor cursor) {
		List<Declaration> decls = [];

		while (!cursor.Eof) {
			decls.Add(OneOf(BlockDeclarationParselet.Instance).Read(cursor));
		}

		return new File {Declarations = [.. decls]};
	}
}