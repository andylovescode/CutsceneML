namespace CutsceneML.Compiler.Bitcode;

public class BasicBlock {
	public List<Instruction> Instructions = [];

	public override string ToString() =>
		$"{nameof(Instructions)}:\n{string.Join("\n", Instructions)}";
}