using CutsceneML.Model;
using CutsceneML.Parser.AST;

namespace CutsceneML.Parser.Lex;

public closed class Token : ITreeNode {
	public required Location Start { get; init; }
	public required Location End { get; init; }
}