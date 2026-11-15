using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class SayStatementParselet : Parselet<SayStatement> {
	public static readonly SayStatementParselet Instance = new();

	public override SayStatement Read(Cursor cursor) {
		SpeechLiteral speech = cursor.GetSpeech();

		return new SayStatement {Speech = speech};
	}
}