using System.Diagnostics;
using System.Runtime.CompilerServices;

using CutsceneML.Parser.AST;

namespace CutsceneML.Parser.Lex;

public class Lexer(string src, string fileName) {
    private int locationIndex = 0;
    private int locationLine = 0;
    private int locationCharacter = 0;
    private char[] chars = src.ToCharArray();
    private Location Location => new(
        fileName, locationLine, locationCharacter
    );

    public List<Error> Errors = [];

    private Location previousLocation = null!;

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
    private char Peek() {
        return chars[ locationIndex ];
    }

    private bool Eof => locationIndex >= chars.Length;

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
                Errors.Add(new Error() {
                    Start = start,
                    End = Location,
                    Title = "Speech literal does not have an end"
                });
            } else {
                Next();
            }

            return new SpeechLiteral {
                Value = value,
                Start = start,
                End = previousLocation
            };
        } else if (char.IsSymbol(indicator) || char.IsPunctuation(indicator)) {
            return new SymbolLiteral {
                Value = indicator,
                Start = start,
                End = start
            };
        } else if (char.IsLetter(indicator)) {
            string value = indicator.ToString();

            while (!Eof && char.IsLetterOrDigit(Peek())) {
                value += Peek();
                Next();
            }

            return new Identifier {
                Value = value,
                Start = start,
                End = previousLocation
            };
        } else if (char.IsNumber(indicator)) {
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
                End = previousLocation
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
        Errors.Clear();
        while (true) {
            ConsumeWhitespace();
            if (Eof) break;
            Token? token = LexToken();
            if (token is not null) {
                yield return token;
            }
        }
    }
}