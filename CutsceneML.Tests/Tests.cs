using System.Diagnostics;

using CutsceneML.Compiler.Bitcode;
using CutsceneML.Model;
using CutsceneML.Parser.AST.Impl;
using CutsceneML.Parser.Lex;

using File = CutsceneML.Parser.AST.File;

namespace CutsceneML.Tests;

public class Tests {
	[Test]
	public void BasicLexing() {
		ErrorCollector collector = new();

		Lexer lexer = new(
			"1 22 3.14 a_bc\n {} \"alpha\"", "main.cml", collector
		);

		foreach (Token token in lexer.Lex()) {
			Console.WriteLine(
				token + " start: " + token.Start + " end: " + token.End
			);
		}

		collector.AssertNoErrors();
	}

	[Test]
	public void BasicParsing() {
		ErrorCollector collector = new();
		Lexer lexer = new(
			"block main { albert: \"Hello, my name is albert!\" } block other {}",
			"main.cml", collector
		);
		Token[] tokens = [.. lexer.Lex()];

		AstParser astParser = new() {
			Tokens = tokens,
			ErrorCollector = collector,
		};

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
			"block start {\n    albert: \"hello\"\n    if flag has_key {\n        albert: \"wow, nice key!\"\n    }\n    choice {\n        \"Do nothing\" {}\n        \"Grab Key\" {\n            albert: \"Did you take my key?\"\n        }\n    }\n}",
			"main.cml", collector
		);
		Token[] tokens = [.. lexer.Lex()];

		AstParser astParser = new() {
			Tokens = tokens,
			ErrorCollector = collector,
		};

		File? file = astParser.ParseFile();

		collector.AssertNoErrors();

		Assert.That(file, Is.Not.Null);
		file.AssertContinuous();

		Console.WriteLine(file);

		FundamentalCompiler compiler = new(collector);

		compiler.CompileFile(file);
		collector.AssertNoErrors();

		Console.WriteLine(compiler.Result);
	}

	[Test]
	public void PerformanceTest() {
		Stopwatch stopwatch = new();
		stopwatch.Start();
		const int testsRuns = 10000;

		for (int i = 0; i < testsRuns; i++) {
			ErrorCollector collector = new();
			Lexer lexer = new(
				"block start {\n    albert: \"hello\"\n    albert: \"hihihi\"\n    if flag has_key {\n        albert: \"wow, nice key!\"\n    }\n    if flag has_key {\n        albert: \"i really like it\"\n    }\n    if flag has_key other_key\n}\n\nblock other_key {\n    albert: \"its the best key\"\n}",
				"main.cml", collector
			);
			Token[] tokens = [.. lexer.Lex()];

			AstParser astParser = new() {
				Tokens = tokens,
				ErrorCollector = collector,
			};

			File? file = astParser.ParseFile();
			collector.AssertNoErrors();
			Assert.That(file, Is.Not.Null);

			FundamentalCompiler compiler = new(collector);
			compiler.CompileFile(file);
			collector.AssertNoErrors();
		}

		stopwatch.Stop();
		double microsPerRun = stopwatch.Elapsed.TotalMicroseconds / testsRuns;
		Assert.That(microsPerRun, Is.LessThan(200));

		Console.WriteLine(Math.Round(microsPerRun) + " micros");
	}
}