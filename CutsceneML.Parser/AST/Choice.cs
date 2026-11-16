using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class Choice : Tree {
	public required SpeechLiteral Text { get; init; }
	public required BlockRef Response { get; init; }

	public override ITreeNode[] Children => [Text, Response];
}