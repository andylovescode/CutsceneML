namespace CutsceneML.Parser.AST;

public struct Error {
    public required Location Start { get; init; }
    public required Location End { get; init; }
    public required string Title { get; init; }
}