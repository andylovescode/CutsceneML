using System.Diagnostics;
using System.Runtime.CompilerServices;

using CutsceneML.Model;

namespace CutsceneML.Parser.Lex;

public class Lexer(string src, string fileName, ErrorCollector collector) {
	private readonly char[] chars = src.ToCharArray();
	private int locationCharacter;
	private int locationIndex;
	private int locationLine;

	private Location previousLocation = null!;

	private Location Location => new(
		fileName, locationLine, locationCharacter
	);

	private bool Eof => locationIndex >= chars.Length;

	private void Next() {
		previousLocation = Location;
		locationIndex++;
		locationCharacter++;

		if (!Eof && Peek() == '\n') {
			locationLine++;
			locationCharacter = 0;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	private char Peek() => chars[locationIndex];

	private Token? LexToken() {
		char indicator = Peek();
		Location start = Location;
		Next();

		if (indicator == '"') {
			string value = "";
			while (!Eof && Peek() != '"') {
				value += Peek();
				Next();
			}

			if (Eof) {
				collector.Errors.Add(new Error {
					Start = start,
					End = Location,
					Title = "Speech literal does not have an end",
				});
			} else {
				Next();
			}

			return new SpeechLiteral {
				Value = value,
				Start = start,
				End = previousLocation,
			};
		}

		if (char.IsSymbol(indicator) || char.IsPunctuation(indicator)) {
			return new SymbolLiteral {
				Value = indicator,
				Start = start,
				End = start,
			};
		}

		if (char.IsLetter(indicator) || indicator == '_') {
			string value = indicator.ToString();

			while (!Eof && (char.IsLetterOrDigit(Peek()) || Peek() == '_')) {
				value += Peek();
				Next();
			}

			return new Identifier {
				Value = value,
				Start = start,
				End = previousLocation,
			};
		}

		if (char.IsNumber(indicator)) {
			string value = indicator.ToString();

			while (!Eof && char.IsNumber(Peek())) {
				value += Peek();
				Next();
			}

			if (!Eof && Peek() == '.') {
				value += ".";
				Next();
				while (!Eof && char.IsNumber(Peek())) {
					value += Peek();
					Next();
				}
			}

			return new NumberLiteral {
				Value = double.Parse(value),
				Start = start,
				End = previousLocation,
			};
		}

		throw new UnreachableException("Unexpected identifier " + indicator);
	}

	private void ConsumeWhitespace() {
		while (!Eof && char.IsWhiteSpace(Peek())) {
			Next();
		}
	}

	public IEnumerable<Token> Lex() {
		while (true) {
			ConsumeWhitespace();
			if (Eof) {
				break;
			}

			Token? token = LexToken();
			if (token is not null) {
				yield return token;
			}
		}
	}
}