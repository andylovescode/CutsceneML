namespace CutsceneML.Compiler.Bitcode;

public class Unit {
	public Dictionary<int, BasicBlock> Blocks = [];

	public override string ToString() {
		string result = "";

		foreach ((int id, BasicBlock block) in Blocks) {
			result += $"block {id}:\n";
			result = block.Instructions.Aggregate(result,
				(current, inst) =>
					$"{current}\t{inst} ; {inst.Location}\n");
		}

		return result;
	}
}