using CutsceneML.Model;
using CutsceneML.Parser.AST;

using File = CutsceneML.Parser.AST.File;

namespace CutsceneML.Compiler.Bitcode;

public class FundamentalCompiler(ErrorCollector errorCollector) {
	public readonly Unit Result = new();

	private readonly Dictionary<string, int> symbolTable = new() {
		["start"] = 0,
	};

	private int firstUnallocatedSymbol = 1;

	private int AllocateSymbol() => firstUnallocatedSymbol++;

	private int GetBlockId(string symbol) {
		if (symbolTable.TryGetValue(symbol, out int value)) {
			return value;
		}

		int sym = AllocateSymbol();
		symbolTable[symbol] = sym;
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
					errorCollector.Errors.Add(
						new Error {
							Start = say.Start,
							End = say.End,
							Title = "No actor was declared",
						}
					);
					break;
				case SayStatement say:
					result.Instructions.Add(
						new SpeakInstruction {
							CharacterName = actor,
							Message = say.Speech.Value,
							Location = say.Start,
						}
					);
					break;
				case IfStatement conditional:
					int predicate = CompileCondition(
						conditional.Condition, result
					);
					result.Instructions.Add(
						new ConditionalCallInstruction {
							Location = statement.Start,
							PredicateRegister = predicate,
							BlockId = ResolveBlockRef(conditional.Block),
						}
					);
					break;
				default:
					throw new Exception(
						"Cannot compile statement of " + statement
					);
			}
		}

		return result;
	}

	private int ResolveBlockRef(BlockRef reference) {
		switch (reference) {
			case AnonymousBlockRef anon:
				int id = AllocateSymbol();
				Result.Blocks[id] = CompileBlock(anon.Body);
				return id;
			case NamedBlockRef named:
				return GetBlockId(named.Name.Value);
			default:
				throw new Exception("Unsupported block reference " + reference);
		}
	}

	private int CompileCondition(Condition predicate, BasicBlock block) {
		int register = AllocateSymbol();
		switch (predicate) {
			case FlagCondition flag:
				block.Instructions.Add(
					new LoadFlagInstruction {
						Flag = flag.Name.Value,
						Location = flag.Start,
						Register = register,
					}
				);
				return register;
			default:
				throw new Exception("Unsupported condition " + predicate);
		}
	}

	public void CompileFile(File file) {
		foreach (Declaration declaration in file.Declarations) {
			switch (declaration) {
				case BlockDeclaration block:
					int id = GetBlockId(block.Name.Value);
					if (Result.Blocks.ContainsKey(id)) {
						errorCollector.Errors.Add(
							new Error {
								Start = block.Name.Start,
								End = block.Name.End,
								Title =
									"This block has already been declared in this file",
							}
						);
						break;
					}

					Result.Blocks[id] = CompileBlock(block.Body);
					break;
			}
		}
	}
}