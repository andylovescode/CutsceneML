namespace CutsceneML.Compiler.Bitcode;

public class ShowChooserInstruction : Instruction {
	public required (int block, string speech)[] Choices { get; init; }

	public override string ToString() => $"SHOW_CHOICER {string.Join(", ", Choices.Select(it => $"'{it.speech}' -> {it.block}"))}";
}