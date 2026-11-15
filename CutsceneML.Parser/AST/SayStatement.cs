using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class SayStatement : Statement {
	public required SpeechLiteral Speech { get; init; }
	public override ITreeNode[] Children => [Speech];
}