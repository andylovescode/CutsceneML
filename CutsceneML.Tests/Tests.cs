using CutsceneML.Compiler.Bitcode;
using CutsceneML.Model;
using CutsceneML.Parser.AST;
using CutsceneML.Parser.Lex;

using File = CutsceneML.Parser.AST.File;

namespace CutsceneML.Tests;

public class Tests {
	[Test]
	public void BasicLexing() {
		ErrorCollector collector = new();

		Lexer lexer = new("1 22 3.14 a_bc\n {} \"alpha\"", "main.cml",
			collector);

		foreach (Token token in lexer.Lex()) {
			Console.WriteLine(token + " start: " + token.Start + " end: " +
							  token.End);
		}

		collector.AssertNoErrors();
	}

	[Test]
	public void BasicParsing() {
		ErrorCollector collector = new();
		Lexer lexer = new(
			"block main { albert: \"Hello, my name is albert!\" } block other {}",
			"main.cml", collector);
		Token[] tokens = [.. lexer.Lex()];

		AstParser astParser =
			new() { Tokens = tokens, ErrorCollector = collector };

		File? file = astParser.ParseFile();
		Assert.NotNull(file);

		file.AssertContinuous();
		collector.AssertNoErrors();

		Console.WriteLine(file);
	}

	[Test]
	public void BasicCompilation() {
		ErrorCollector collector = new();
		Lexer lexer = new(
			"block start {\n    albert: \"hello\"\n    if flag has_key {\n        albert: \"wow, nice key!\"\n    }\n}",
			"main.cml", collector);
		Token[] tokens = [.. lexer.Lex()];

		AstParser astParser =
			new() { Tokens = tokens, ErrorCollector = collector };

		File? file = astParser.ParseFile();

		collector.AssertNoErrors();

		Assert.NotNull(file);
		file.AssertContinuous();

		Console.WriteLine(file);

		FundamentalCompiler compiler = new(collector);

		compiler.CompileFile(file);
		collector.AssertNoErrors();

		Console.WriteLine(compiler.Result);
	}
}