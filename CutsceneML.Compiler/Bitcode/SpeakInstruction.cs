namespace CutsceneML.Compiler.Bitcode;

public class SpeakInstruction : Instruction {
	public required string CharacterName { get; init; }
	public required string Message { get; init; }

	public override string ToString() =>
		$"SPEAK {nameof(CharacterName)}: {CharacterName}, {nameof(Message)}: {Message}";
}