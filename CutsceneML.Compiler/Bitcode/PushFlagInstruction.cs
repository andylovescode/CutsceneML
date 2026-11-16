namespace CutsceneML.Compiler.Bitcode;

public class PushFlagInstruction : Instruction {
	public required string Flag { get; init; }
	public override string ToString() => "PUSH_FLAG " + Flag;
}