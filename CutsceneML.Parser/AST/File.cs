namespace CutsceneML.Parser.AST;

public class File : Tree {
	public required Declaration[] Declarations { get; init; }
	public override ITreeNode[] Children => [.. Declarations];
}