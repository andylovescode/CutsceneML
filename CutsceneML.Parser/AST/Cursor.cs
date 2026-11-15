using System.Diagnostics.CodeAnalysis;

using CutsceneML.Parser.Lex;

namespace CutsceneML.Parser.AST;

public class Cursor {
	public List<Error> Errors = [];
	public int Location;
	public required Token[] Tokens { get; init; }
	public bool Eof => Location >= Tokens.Length;

	[DoesNotReturn]
	public void RaiseError(Error error) {
		Errors.Add(error);
		throw new OperationCanceledException("Error raised, short-circuiting");
	}

	[DoesNotReturn]
	public void RaiseError(ITreeNode node, string message) =>
		RaiseError(new Error {Start = node.Start, End = node.End, Title = message});

	public SymbolLiteral GetSymbol(char symbol) {
		Token tkn = Next();

		if (tkn is not SymbolLiteral literal || literal.Value != symbol) {
			RaiseError(tkn, $"Expected a '{symbol}' symbol");
		}

		return (SymbolLiteral)tkn;
	}

	public Identifier GetKeyword(string keyword) {
		Token tkn = Next();

		if (tkn is not Identifier ident || ident.Value != keyword) {
			RaiseError(tkn, $"Expected a '{keyword}' keyword");
		}

		return (Identifier)tkn;
	}

	public Cursor Clone() => new() {Location = Location, Tokens = Tokens, Errors = [.. Errors]};

	public Token Next() => Tokens[Location++];

	public Identifier GetIdentifier() {
		Token tkn = Next();
		if (tkn is not Identifier) {
			RaiseError(tkn, "Expected an identifier");
		}

		return (Identifier)tkn;
	}

	public SpeechLiteral GetSpeech() {
		Token tkn = Next();

		if (tkn is not SpeechLiteral) {
			RaiseError(tkn, "Expected a speech literal");
		}

		return (SpeechLiteral)tkn;
	}
}