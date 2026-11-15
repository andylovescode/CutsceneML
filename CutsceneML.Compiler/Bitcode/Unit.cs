namespace CutsceneML.Compiler.Bitcode;

public class Unit {
	public Dictionary<int, BasicBlock> Blocks = [];

	public override string ToString() => $"{string.Join("\n----\n", Blocks.Select(it => $"{it.Key}:\n{it.Value}"))}";
}