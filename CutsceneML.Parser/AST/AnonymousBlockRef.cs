namespace CutsceneML.Parser.AST;

public class AnonymousBlockRef : BlockRef {
	public required BlockBody Body { get; init; }
	public override ITreeNode[] Children => [Body];
}