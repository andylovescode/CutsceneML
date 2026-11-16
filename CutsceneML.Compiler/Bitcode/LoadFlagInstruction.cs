namespace CutsceneML.Compiler.Bitcode;

public class LoadFlagInstruction : Instruction {
	public required string Flag { get; init; }
	public required int Register { get; init; }
	public override string ToString() => $"LOAD_FLAG {Flag} -> {Register}";
}