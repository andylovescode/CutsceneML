using CutsceneML.Compiler.Bitcode;
using CutsceneML.Parser.Lex;

using File = CutsceneML.Parser.AST.File;

namespace CutsceneML.Tests;

public class Tests {
	[Test]
	public void BasicLexing() {
		Lexer lexer = new("1 22 3.14 abc\n {} \"alpha\"", "main.cml");

		foreach (Token token in lexer.Lex()) {
			Console.WriteLine(token + " start: " + token.Start + " end: " + token.End);
		}
	}

	[Test]
	public void BasicParsing() {
		Lexer lexer = new("block main { albert: \"Hello, my name is albert!\" } block other {}",
			"main.cml");
		Token[] tokens = [.. lexer.Lex()];

		Assert.IsEmpty(lexer.Errors);

		Parser.AST.Parser parser = new() { Tokens = tokens };

		File? file = parser.ParseFile();
		Assert.NotNull(file);

		parser.AssertNoErrors();
		file.AssertContinuous();

		Console.WriteLine(file);
	}

	[Test]
	public void BasicCompilation() {
		Lexer lexer = new("block main { albert: \"Hello, my name is albert!\" } block other {}",
			"main.cml");
		Token[] tokens = [.. lexer.Lex()];
		Assert.IsEmpty(lexer.Errors);

		Parser.AST.Parser parser = new() { Tokens = tokens };

		File? file = parser.ParseFile();
		Assert.NotNull(file);

		parser.AssertNoErrors();
		file.AssertContinuous();

		Console.WriteLine(file);

		FundamentalCompiler compiler = new FundamentalCompiler();

		compiler.CompileFile(file);

		Console.WriteLine(compiler.Result);
	}
}