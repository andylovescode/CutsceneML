using CutsceneML.Model;
using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class AstParser {
	public required Token[] Tokens { get; init; }
	public required ErrorCollector ErrorCollector { get; init; }

	public File? ParseFile() {
		Cursor cursor = new() { Tokens = Tokens };

		try {
			File result = Parselets.File.Read(cursor);

			ErrorCollector.Errors.AddRange(cursor.Errors);

			return result;
		} catch (OperationCanceledException) {
			ErrorCollector.Errors.AddRange(cursor.Errors);

			return null;
		}
	}
}