using CutsceneML.Parser.AST;

using File = CutsceneML.Parser.AST.File;

namespace CutsceneML.Compiler.Bitcode;

public class FundamentalCompiler {
	public readonly Unit Result = new Unit();
	private Dictionary<string, int> SymbolTable = new ();
	private int FirstUnallocatedSymbol;
	public readonly List<Error> Errors = [];

	private int AllocateSymbol() => FirstUnallocatedSymbol++;

	private int GetBlockId(string symbol) {
		if (SymbolTable.TryGetValue(symbol, out int value)) {
			return value;
		}

		int sym = AllocateSymbol();
		SymbolTable[symbol] = sym;
		return sym;
	}

	private BasicBlock CompileBlock(BlockBody body) {
		BasicBlock result = new();
		string? actor = null;
		
		foreach (Statement statement in body.Statements) {
			switch (statement) {
				case ChangeActorStatement actorStatement:
					actor = actorStatement.ActorName.Value;
					break;
				case SayStatement say when actor is null:
					Errors.Add(new Error {
						Start = say.Start,
						End = say.End,
						Title = "No actor was declared"
					});
					continue;
				case SayStatement say:
					result.Instructions.Add(new SpeakInstruction {
						CharacterName = actor,
						Message = say.Speech.Value,
						Location = say.Start
					});
					break;
			}
		}

		return result;
	}

	public void CompileFile(File file) {
		foreach (Declaration declaration in file.Declarations) {
			switch (declaration) {
				case BlockDeclaration block:
					int id = GetBlockId(block.Name.Value);
					if (Result.Blocks.ContainsKey(id)) {
						Errors.Add(new Error {
							Start = block.Name.Start,
							End = block.Name.End,
							Title = "This block has already been declared in this file"
						});
						break;
					}
					Result.Blocks[id] = CompileBlock(block.Body);
					break;
			}
		}
	}
}