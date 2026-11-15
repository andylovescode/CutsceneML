using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class Parser {
	public List<Error> Errors = [];
	public required Token[] Tokens { get; init; }

	public void AssertNoErrors() {
		if (Errors.Count > 0) {
			throw new Exception(
				"Unexpected errors: "
				+ string.Join(
					"\n", Errors.Select(it => $"{it.Title} {it.Start}")
				)
			);
		}
	}

	public File? ParseFile() {
		Cursor cursor = new() {Tokens = Tokens};

		try {
			File result = FileParselet.Instance.Read(cursor);

			Errors.AddRange(cursor.Errors);

			return result;
		} catch (OperationCanceledException) {
			Errors.AddRange(cursor.Errors);

			return null;
		}
	}
}