using CutsceneML.Model;

namespace CutsceneML.Parser.AST;

public interface ITreeNode {
	Location Start { get; }
	Location End { get; }
}