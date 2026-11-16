namespace CutsceneML.Compiler.Bitcode;

public class ConditionalCallInstruction : Instruction {
	public required int BlockId { get; init; }
	public required int PredicateRegister { get; init; }

	public override string ToString() =>
		$"COND_CALL to {BlockId} using {PredicateRegister}";
}